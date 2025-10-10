using Cysharp.Threading.Tasks;
using System;
using System.Collections;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using UnityEditor;
using UnityEngine;
using UnityEngine.AddressableAssets;
using UnityEngine.Networking;
public enum CsvType
{
    PlayerUnit,
    MonsterSkillData,
    Monster,
    PlayerSkillData,
    StageFirstReward,
    StageReward,
    Floor
}

public class CsvDownloader
{
    private CsvLoadData _csvLoadData;

    public static event Action OnDataSetupCompleted;

    private UnitSkill[] _monsterSkills;
    private UnitSkill[] _playerSkills;
    private UnitData[] _monsterUnitDatas;
    private UnitAttackData[] _attackDatas;
    private LevelUpData _levelUpData;

    public CsvDownloader(CsvLoadData csvLoadData)
    {
        _csvLoadData = csvLoadData;
    }

    /// <summary>
    /// 데이터 다운로드 및 세팅
    /// </summary>
    public async UniTask DownloadDataAsync()
    {
        _monsterSkills = await Manager.Resources.LoadAll<UnitSkill>("SkillData");
        _playerSkills = await Manager.Resources.LoadAll<UnitSkill>("SkillData_Player");
        _monsterUnitDatas = await Manager.Resources.LoadAll<UnitData>("EnemyUnitData");
        _attackDatas = await Manager.Resources.LoadAll<UnitAttackData>("AttackData");
        _levelUpData = await Addressables.LoadAssetAsync<LevelUpData>("Data/LevelUpData");

        List<UniTask> tasks = new List<UniTask>(10);

        foreach (var csvData in _csvLoadData.CsvDatas)
        {
            tasks.Add(LoadCSV(csvData.GetURL(), GetSetupMethod(csvData.CsvType), csvData.StartLine));
        }

        await UniTask.WhenAll(tasks);

        Debug.Log("[파싱 종료]");
        
        OnDataSetupCompleted?.Invoke();
    }

    /// <summary>
    /// CSV 다운로드 + 파싱
    /// </summary>
    private async UniTask LoadCSV(string url, Action<string[][]> onParsed, int startLine = 1)
    {
        using UnityWebRequest req = UnityWebRequest.Get(url);

        await req.SendWebRequest().ToUniTask();

        if (!string.IsNullOrEmpty(req.error))
        {
            Debug.LogError($"TSV 다운로드 실패: {url}, Error: {req.error}");
            return;
        }

        string raw = req.downloadHandler.text.Trim();
        string[] lines = raw.Split('\n');
        List<string[]> parsed = new();

        for (int i = startLine - 1; i < lines.Length; i++)
        {
            string[] row = lines[i].Trim().Split('\t');
            parsed.Add(row);
        }

        onParsed?.Invoke(parsed.ToArray());
    }

    private Action<string[][]> GetSetupMethod(CsvType csvType)
    {
        switch(csvType)
        {
            case CsvType.PlayerUnit:
                return PlayerUnitStatSetup;
            case CsvType.PlayerSkillData:
                return PlayerSkillSetup;
            case CsvType.Monster: 
                return MonsterSetup;
            case CsvType.MonsterSkillData:
                return MonsterSkillSetup;
            case CsvType.StageFirstReward:
                return StageFirstRewardSetup;
            case CsvType.StageReward:
                return StageRewardSetup;
            case CsvType.Floor:
                return FloorRewardSetup;
            default:
                Debug.LogError($"알 수 없는 CSV 이름: {csvType.ToString()}");
                return null;
        }
    }

    private void PlayerUnitStatSetup(string[][] data)
    {
        UnitData[] unitDatas = Manager.Data.PlayerUnitDatas;

        foreach (var row in data)
        {
            int id = int.Parse(row[0]);
            UnitData unitData = Array.Find(unitDatas, u => u.ID == id);

            if (unitData == null)
            {
                Debug.LogWarning($"UnitData with ID {id} not found.");
                continue;
            }

            unitData.Grade = Enum.TryParse(row[1], out Grade grade) ? grade : Grade.NORMAL;                        
            unitData.Cost = int.TryParse(row[2], out int cost) ? cost : 0;
            unitData.Name = row[3];
            unitData.Description = row[4];
            unitData.PerferredLine = int.TryParse(row[5], out int line) ? line : 0;
            unitData.ClassSynergy = Enum.TryParse(row[6], out ClassType classSynergy) ? classSynergy : ClassType.TANK;
            unitData.Synergy = Enum.TryParse(row[7], out Synergy synergy) ? synergy : Synergy.KINGDOM;

            AnimationType attackAnimation = Enum.TryParse(row[8], out AnimationType attackAnim) ? attackAnim : AnimationType.Magic_Attack;
            AnimationType skillAnimation = Enum.TryParse(row[9], out AnimationType skillAnim) ? skillAnim : AnimationType.Magic_Attack;

            unitData.AnimatiorData.AttackAnimationType = attackAnimation;
            unitData.AnimatiorData.SkillAnimationType = skillAnimation;

            int attackID = int.TryParse(row[10], out int _attackID) ? _attackID : 0;
            unitData.AttackData = Array.Find(_attackDatas, a => a.ID == attackID);

            UnitStats stat = new UnitStats
            {
                AttackRange = int.TryParse(row[11], out int attackRange) ? attackRange * 1.5f : 1,
                AttackSpeed = float.TryParse(row[13], out float attackSpeed) ? attackSpeed : 1f,
                ManaGain = int.TryParse(row[14], out int manaGain) ? manaGain : 0,
                PhysicalDamage = int.TryParse(row[15], out int physicalAttack) ? physicalAttack : 0,
                MagicDamage = int.TryParse(row[16], out int magicAttack) ? magicAttack : 0,
                PhysicalDefense = int.TryParse(row[17], out int physicalDefense) ? physicalDefense : 0,
                MagicDefense = int.TryParse(row[18], out int magicDefense) ? magicDefense : 0,
                CritChance = int.TryParse(row[19], out int critRate) ? critRate : 0,
                MaxHealth = int.TryParse(row[20], out int hp) ? hp : 0,
                MaxMana = int.TryParse(row[21], out int mp) ? mp : 0,
                MoveSpeed = 1.5f,
                AttackCount = 1
            };

            unitData.UnitStats = new UnitStats[4];            
            unitData.UnitStats[0] = stat;

            for(int i = 1; i < unitData.UnitStats.Length; i++)
            {
                unitData.UnitStats[i] = unitData.UnitStats[i-1].StatMultiply(1.5f);
            }
            
            unitData.LevelUpData = _levelUpData;

            string synergyText = unitData.Synergy.ToString();
            string synergyName = $"{char.ToUpper(synergyText[0])}{synergyText.Substring(1).ToLower()}";
            int lastDigit = Mathf.Abs(id % 10);

            unitData.AddressableAddress = $"{synergyName}{lastDigit}";
            unitData.Icon = Manager.Resources.SpriteLoad($"{unitData.AddressableAddress}_Icon");
            unitData.Skill.Icon = Manager.Resources.SpriteLoad($"{unitData.AddressableAddress}_Skillicon");

            //#if UNITY_EDITOR
            //            unitData.name = $"{unitData.Synergy.ToString()}_{id}";
            //            EditorUtility.SetDirty(unitData);

            //            string path = AssetDatabase.GetAssetPath(unitData);
            //            AssetDatabase.RenameAsset(path, unitData.Name);
            //            AssetDatabase.SaveAssets();
            //#endif
        }
    }

    private void MonsterSetup(string[][] data)
    {
        foreach (var row in data)
        {
            int id = int.Parse(row[0]);
            UnitData unitData = Array.Find(_monsterUnitDatas, u => u.ID == id);

            if (unitData == null)
            {
                Debug.LogWarning($"UnitData with ID {id} not found.");
                continue;
            }

            unitData.Name = row[2];

            UnitStats stat = new UnitStats
            {
                AttackRange = float.TryParse(row[1], out float attackRange) ? attackRange * 1.5f : 1,
                AttackSpeed = float.TryParse(row[3], out float attackSpeed) ? attackSpeed : 1f,
                ManaGain = int.TryParse(row[4], out int manaGain) ? manaGain : 0,
                PhysicalDamage = int.TryParse(row[5], out int physicalAttack) ? physicalAttack : 0,
                MagicDamage = int.TryParse(row[6], out int magicAttack) ? magicAttack : 0,
                PhysicalDefense = int.TryParse(row[7], out int physicalDefense) ? physicalDefense : 0,
                MagicDefense = int.TryParse(row[8], out int magicDefense) ? magicDefense : 0,
                CritChance = int.TryParse(row[9], out int critRate) ? critRate : 0,
                MaxHealth = int.TryParse(row[10], out int hp) ? hp : 0,
                MaxMana = int.TryParse(row[11], out int mp) ? mp : 0,

                MoveSpeed = 1.5f,
                AttackCount = 1
            };

            unitData.Skill = Array.Find(_monsterSkills, u => u.ID == int.Parse(row[12]));
            unitData.AttackData = Array.Find(_attackDatas, a => a.ID == int.Parse(row[14]));

            unitData.UnitStats = new UnitStats[4];
            unitData.PerferredLine = Mathf.RoundToInt(stat.AttackRange / 1.5f);
            unitData.AddressableAddress = $"Monster_{unitData.ID}";

            unitData.Icon = Manager.Resources.SpriteLoad($"{unitData.AddressableAddress}_Icon");

            unitData.UnitStats[0] = stat;
            unitData.UnitStats[1] = stat;
            unitData.UnitStats[2] = stat;
            unitData.UnitStats[3] = stat;            
        }
    }

    private void MonsterSkillSetup(string[][] data)
    {
        foreach (var row in data)
        {
            int id = int.Parse(row[0]);

            UnitSkill skill = Array.Find(_monsterSkills, u => u.ID == id);

            if (skill == null) continue;

            skill.SkillName = row[1];
            skill.Description = row[14];

            skill.ManaCost = int.TryParse(row[2], out int manaCost) ? manaCost : 60;
            
            if(skill is AttackSkill attackSkill)
            {
                attackSkill.DamageType = Enum.TryParse(row[4], out DamageType damageType) ? damageType : DamageType.Physical;
            }

            skill.PhysicalPower = float.TryParse(row[6], out float power) ? power : 1;
            skill.AbilityPower = float.TryParse(row[7], out float abilityPower) ? abilityPower : 100;

            skill.Icon = Manager.Resources.SpriteLoad($"{skill.ID}_Skillicon");
        }
    }

    private void PlayerSkillSetup(string[][] data)
    {
        foreach (var row in data)
        {
            if (!int.TryParse(row[0], out int skillID)) continue;
            if (!int.TryParse(row[1], out int charID)) continue;

            UnitSkill skillData = Array.Find(_playerSkills, s => s.ID == skillID);
            if (skillData == null) continue;

            if (int.TryParse(row[2], out int manaCost))
                skillData.ManaCost = manaCost;

            skillData.SkillName = row[3];
            skillData.Description = row[4];

            if (int.TryParse(row[8], out int power))
                skillData.PhysicalPower = power;

            if (int.TryParse(row[9], out int abilityPower))
                skillData.AbilityPower = abilityPower;

            if (skillData is AttackSkill attackSkillData)
            {
                if (int.TryParse(row[6], out int damageType))
                    attackSkillData.DamageType = (DamageType)damageType;
            }
            else if (skillData is BuffSkill buffSkill)
            {
                if (int.TryParse(row[13], out int duration))
                    buffSkill.BuffEffectData.Duration = duration;

                if (int.TryParse(row[14], out int tickInterval))
                    buffSkill.BuffEffectData.TickInterval = tickInterval;
            }

            UnitData unitData = Array.Find(Manager.Data.PlayerUnitDatas, p => p.ID == charID);
            if (unitData != null)
            {
                unitData.Skill = skillData;
            }
        }
    }

    private void StageFirstRewardSetup(string[][] data)
    {        
        foreach (var row in data)
        {
            string[] numbers = row[0].Split('-');

            int region = int.Parse(numbers[0]);
            int stage = int.Parse(numbers[1]);

            StageRewardData[] stageRewardDatas = Manager.Data.StageDatas.GetStage(region).StageFirstRewardDatas;

            if (stageRewardDatas == null)
            {
                Manager.Data.StageDatas.GetStage(region).StageFirstRewardDatas = new StageRewardData[4];
                stageRewardDatas = Manager.Data.StageDatas.GetStage(region).StageFirstRewardDatas;
            }

            OutGameRewardData[] outGameRewardDatas = new OutGameRewardData[3];

            outGameRewardDatas[0].RewardType = OutGameRewardType.Gold;
            outGameRewardDatas[0].Amount = int.TryParse(row[1], out int gold) ? gold : 0;

            outGameRewardDatas[1].RewardType = OutGameRewardType.Diamond;
            outGameRewardDatas[1].Amount = int.TryParse(row[2], out int diamond) ? diamond : 0;

            outGameRewardDatas[2].RewardType = OutGameRewardType.Exp;
            outGameRewardDatas[2].Amount = int.TryParse(row[3], out int exp) ? exp : 0;

            int idx = stage - 1;
            Debug.Log(row[0]);

            // 배열이 null이면 새로 생성
            if (stageRewardDatas == null || stageRewardDatas.Length < 4)
            {
                stageRewardDatas = new StageRewardData[4];
                Manager.Data.StageDatas.GetStage(region).StageFirstRewardDatas = stageRewardDatas;
            }

            // 해당 인덱스에 객체가 없으면 초기화
            if (stageRewardDatas[idx] == null)
            {
                stageRewardDatas[idx] = new StageRewardData();
            }

            stageRewardDatas[idx].StageNumber = stage;
            stageRewardDatas[idx].RewardDatas = outGameRewardDatas;
        }
    }

    private void StageRewardSetup(string[][] data)
    {
        foreach (var row in data)
        {
            string[] numbers = row[0].Split('-');

            int region = int.Parse(numbers[0]);
            int stage = int.Parse(numbers[1]);

            StageRewardData[] stageRewardDatas = Manager.Data.StageDatas.GetStage(region).StageRewardDatas;

            if (stageRewardDatas == null)
            {
                Manager.Data.StageDatas.GetStage(region).StageRewardDatas = new StageRewardData[4];
                stageRewardDatas = Manager.Data.StageDatas.GetStage(region).StageRewardDatas;
            }

            OutGameRewardData[] outGameRewardDatas = new OutGameRewardData[3];

            outGameRewardDatas[0].RewardType = OutGameRewardType.Gold;
            outGameRewardDatas[0].Amount = int.TryParse(row[1], out int gold) ? gold : 0;

            outGameRewardDatas[1].RewardType = OutGameRewardType.Diamond;
            outGameRewardDatas[1].Amount = int.TryParse(row[2], out int diamond) ? diamond : 0;

            outGameRewardDatas[2].RewardType = OutGameRewardType.Exp;
            outGameRewardDatas[2].Amount = int.TryParse(row[3], out int exp) ? exp : 0;

            stageRewardDatas[stage-1].StageNumber = stage;
            stageRewardDatas[stage-1].RewardDatas = outGameRewardDatas;
        }
    }

    private void FloorRewardSetup(string[][] data)
    {
        int count = 0;
        foreach (var row in data)
        {
            int floor = int.Parse(row[0]);

            StageInGameRewardData stageFloorRewardData = new StageInGameRewardData();

            stageFloorRewardData.Floor = floor;

            stageFloorRewardData.StageFloorRewardTypes[0].RewardType = InGameRewardType.Energy;
            stageFloorRewardData.StageFloorRewardTypes[0].Amount = int.TryParse(row[1], out int energy) ? energy : 0;

            stageFloorRewardData.StageFloorRewardTypes[1].RewardType = InGameRewardType.Silver;
            stageFloorRewardData.StageFloorRewardTypes[1].Amount = int.TryParse(row[2], out int silver) ? silver : 0;

            Manager.Data.StageGameData.FloorRewardDatas[count] = stageFloorRewardData;

            count++;
        }
    }

    //private void CreateMonsterUnitData(string[][] data)
    //{
    //    foreach (var row in data)
    //    {
    //        UnitData unitData = ScriptableObject.CreateInstance<UnitData>();

    //        unitData.ID = int.Parse(row[0]);

    //        UnitStats stat = new UnitStats
    //        {
    //            AttackRange = int.TryParse(row[1], out int attackRange) ? attackRange : 1,
    //            AttackSpeed = float.TryParse(row[3], out float attackSpeed) ? attackSpeed : 1f,
    //            ManaGain = int.TryParse(row[4], out int manaGain) ? manaGain : 0,
    //            PhysicalDamage = int.TryParse(row[5], out int physicalAttack) ? physicalAttack : 0,
    //            MagicDamage = int.TryParse(row[6], out int magicAttack) ? magicAttack : 0,
    //            PhysicalDefense = int.TryParse(row[7], out int physicalDefense) ? physicalDefense : 0,
    //            MagicDefense = int.TryParse(row[8], out int magicDefense) ? magicDefense : 0,
    //            CritChance = int.TryParse(row[9], out int critRate) ? critRate : 0,
    //            MaxHealth = int.TryParse(row[10], out int hp) ? hp : 0,
    //            MaxMana = int.TryParse(row[11], out int mp) ? mp : 0,

    //            MoveSpeed = 1.5f,
    //            AttackCount = 1
    //        };

    //        unitData.Skill = Array.Find(_unitSkills, u => u.ID == int.Parse(row[12]));
    //        unitData.AttackData = Array.Find(_attackDatas, a => a.ID == int.Parse(row[14]));

    //        unitData.UnitStats = new UnitStats[4];

    //        unitData.UnitStats[0] = stat;
    //        unitData.UnitStats[1] = stat;
    //        unitData.UnitStats[2] = stat;
    //        unitData.UnitStats[3] = stat;

    //        unitData.Name = unitData.ID.ToString(); // 임시

    //        // 에셋 저장 경로
    //        string assetPath = $"Assets/Workspace/HSD/Datas/MonsterUnitData/Monster_{unitData.ID}.asset";

    //        // 중복 체크
    //        if (!System.IO.File.Exists(assetPath))
    //        {
    //            AssetDatabase.CreateAsset(unitData, assetPath);
    //        }
    //        else
    //        {
    //            Debug.LogWarning($"Monster_{unitData.ID}.asset already exists, skipping...");
    //        }
    //    }

    //    AssetDatabase.SaveAssets();
    //    AssetDatabase.Refresh();
    //}
}

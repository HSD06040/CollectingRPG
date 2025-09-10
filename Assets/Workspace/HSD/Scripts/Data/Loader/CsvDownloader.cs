using Cysharp.Threading.Tasks;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEditor;
using UnityEngine;
using UnityEngine.Networking;
public enum CsvType
{
    UnitStat,
    Skill,
    Monster
}

public class CsvDownloader
{
    private CsvLoadData _csvLoadData;

    public static event Action OnDataSetupCompleted;

    private UnitSkill[] _unitSkills;
    private UnitData[] _monsterUnitDatas;
    private UnitAttackData[] _attackDatas;

    public CsvDownloader(CsvLoadData csvLoadData)
    {
        _csvLoadData = csvLoadData;
    }

    /// <summary>
    /// 데이터 다운로드 및 세팅
    /// </summary>
    public async UniTask DownloadDataAsync()
    {
        _unitSkills = await Manager.Resources.LoadAll<UnitSkill>("SkillData");
        _monsterUnitDatas = await Manager.Resources.LoadAll<UnitData>("EnemyUnitData");
        _attackDatas = await Manager.Resources.LoadAll<UnitAttackData>("AttackData");

        List<UniTask> tasks = new List<UniTask>(10);

        foreach (var csvData in _csvLoadData.CsvDatas)
        {
            //tasks.Add(LoadCSV(csvData.GetURL(), GetSetupMethod(csvData.CsvType), csvData.StartLine));
        }

        await LoadCSV(_csvLoadData.CsvDatas[2].GetURL(), GetSetupMethod(CsvType.Monster));

        await UniTask.WhenAll(tasks);

        Debug.Log("끝!");
        
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
            Debug.LogError($"CSV 다운로드 실패: {url}, Error: {req.error}");
            return;
        }

        string raw = req.downloadHandler.text.Trim();
        string[] lines = raw.Split('\n');
        List<string[]> parsed = new();

        for (int i = startLine - 1; i < lines.Length; i++)
        {
            string[] row = lines[i].Trim().Split(',');
            parsed.Add(row);
        }

        onParsed?.Invoke(parsed.ToArray());
    }

    private Action<string[][]> GetSetupMethod(CsvType csvType)
    {
        switch(csvType)
        {
            case CsvType.UnitStat:
                return UnitStatSetup;
            case CsvType.Skill:
                return UnitSkillSetup;
            case CsvType.Monster: 
                return MonsterSetup;
            default:
                Debug.LogError($"알 수 없는 CSV 이름: {csvType.ToString()}");
                return null;
        }
    }

    private void UnitStatSetup(string[][] data)
    {
        UnitData[] unitDatas = Manager.Data.UnitDataDic.Values.ToArray();
 
        foreach (var row in data)
        {
            int id = int.Parse(row[0]);
            UnitData unitData = Array.Find(unitDatas, u => u.ID == id);
            Debug.Log($"Setting up UnitData ID: {id}");
            if (unitData == null)
            {
                Debug.LogWarning($"UnitData with ID {id} not found.");
                continue;
            }

            unitData.Grade = Enum.TryParse(row[1], out Grade grade) ? grade : Grade.NORMAL;                        
            unitData.Cost = int.TryParse(row[2], out int cost) ? cost : 0;
            unitData.PerferredLine = int.TryParse(row[3], out int line) ? line : 0;
            unitData.ClassSynergy = Enum.TryParse(row[4], out ClassType classSynergy) ? classSynergy : ClassType.TANK;
            unitData.Synergy = Enum.TryParse(row[5], out Synergy synergy) ? synergy : Synergy.KINGDOM;

            UnitStats stat = new UnitStats
            {
                AttackRange = int.TryParse(row[6], out int attackRange) ? attackRange : 1,
                AttackSpeed = float.TryParse(row[8], out float attackSpeed) ? attackSpeed : 1f,
                ManaGain = int.TryParse(row[9], out int manaGain) ? manaGain : 0,
                PhysicalDamage = int.TryParse(row[10], out int physicalAttack) ? physicalAttack : 0,
                MagicDamage = int.TryParse(row[11], out int magicAttack) ? magicAttack : 0,
                PhysicalDefense = int.TryParse(row[12], out int physicalDefense) ? physicalDefense : 0,
                MagicDefense = int.TryParse(row[13], out int magicDefense) ? magicDefense : 0,
                CritChance = int.TryParse(row[14], out int critRate) ? critRate : 0,
                MaxHealth = int.TryParse(row[15], out int hp) ? hp : 0,
                MaxMana = int.TryParse(row[16], out int mp) ? mp : 0,
                MoveSpeed = 1.5f,
                AttackCount = 1
            };

            unitData.UnitStats = new UnitStats[4];

            unitData.UnitStats[0] = stat;
            unitData.UnitStats[1] = stat;
            unitData.UnitStats[2] = stat;
            unitData.UnitStats[3] = stat;

            unitData.Name = id.ToString(); // 임시
        }
    }

    private void MonsterSetup(string[][] data)
    {
        foreach (var row in data)
        {
            int id = int.Parse(row[0]);
            UnitData unitData = Array.Find(_monsterUnitDatas, u => u.ID == id);
            Debug.Log($"Setting up UnitData ID: {id}");
            if (unitData == null)
            {
                Debug.LogWarning($"UnitData with ID {id} not found.");
                continue;
            }

            UnitStats stat = new UnitStats
            {
                AttackRange = int.TryParse(row[1], out int attackRange) ? attackRange : 1,
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

            unitData.Skill = Array.Find(_unitSkills, u => u.ID == int.Parse(row[12]));
            unitData.AttackData = Array.Find(_attackDatas, a => a.ID == int.Parse(row[14]));

            unitData.UnitStats = new UnitStats[4];

            unitData.UnitStats[0] = stat;
            unitData.UnitStats[1] = stat;
            unitData.UnitStats[2] = stat;
            unitData.UnitStats[3] = stat;

            unitData.Name = id.ToString(); // 임시
        }
    }

    private void UnitSkillSetup(string[][] data)
    {
        foreach (var row in data)
        {
            
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

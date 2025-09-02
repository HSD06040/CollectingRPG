using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using UnityEngine;
using Firebase.Database;

public class PlayerMailBoxController : MonoBehaviour
{
    private List<MailData> _mail;
    private DatabaseReference _mailRef;

    public event Action<List<MailData>> OnMailboxUpdated;
    public List<MailData> Mail { get { return _mail; } }


    private void Start()
    {
        InitAsync();
    }

    private void OnEnable()
    {
        StartListeningToMailData();
    }

    private void OnDisable()
    {
        StopListeningToMailData();
    }

    private async void InitAsync()
    {
        // TODO: [CYH] 로그인 씬 호출
        await Manager.DB.SyncMailsOnLoginAsync();

        List<MailData> userMailDB = await LoadAsync();
        RefreshUI(userMailDB);
        Debug.Log("[PlayerMailBoxController] InitAsync");
    }

    private void RefreshUI(List<MailData> mailList)
    {
        Debug.Log("[PlayerMailBoxController] RefreshUI");
        _mail = mailList;

        // ReceivedDate 기준 내림차순 정렬
        _mail.Sort((a, b) => b.ReceivedDate.CompareTo(a.ReceivedDate));
        OnMailboxUpdated?.Invoke(_mail);
    }

    private async Task<List<MailData>> LoadAsync()
    {
        // DB에서 UserMail + MasterMail 리스트 로드
        List<MailData> mails = await Manager.DB.LoadUserMailsAsync();
        return mails;
    }

    /// <summary>
    /// 강제 새로고침
    /// </summary>
    public async Task RefreshAsync()
    {
        var loaded = await LoadAsync();
        RefreshUI(loaded);
    }

    /// <summary>
    /// 
    /// </summary>
    /// <param name="mailId"></param>
    /// <returns></returns>
    public async Task ReceiveRewardAsync(string mailId)
    {
        MailData mail = _mail?.Find(m => m.MailId == mailId);

        if(mail == null)
        {
            List<MailData> loadedMail = await LoadAsync();
            mail = loadedMail?.Find(m => m.MailId == mailId);

            if (mail == null)
            {
                Debug.LogWarning($"[ReceiveReward] mail == null : {mailId}");
                return;
            }
        }

        // IsReceived == false일 때 보상 지급
        if (!mail.IsReceived)
        {
            Task goldTask = Manager.DB.AddGoldAsync(mail.Gold);
            Task diaTask = Manager.DB.AddDiamondAsync(mail.Diamond);
            await Task.WhenAll(goldTask, diaTask);
        }

        // 해당 메일 IsReceived == true 업데이트
       // await Manager.DB.SetMailIsReceivedAsync(mailId, true);
        
        // 메일 삭제
        DeleteMail(mailId);
    }

    public async Task ReceiveAllAsync()
    {
        if (_mail == null || _mail.Count == 0) return;

        long currentTime = DateTimeOffset.UtcNow.ToUnixTimeMilliseconds();

        // 조건: 수령 안 했고, 만료도 안 된 메일만
        List<MailData> unReceivedMailList = _mail.FindAll(m => !m.IsReceived && !m.IsExpired(currentTime));

        if (unReceivedMailList.Count == 0)
        {
            Debug.Log("[ReceiveAllAsync] 받을 메일 없음");
            return;
        }

        // 하나씩 순차 처리 (DB 이벤트/리스너 겹침 방지에 안전)
        foreach (var mail in unReceivedMailList)
        {
            await ReceiveRewardAsync(mail.MailId);
            // 필요시 프레임 양보
            await Task.Yield();
        }

        // 마지막에 한 번만 새로고침
        await RefreshAsync();
    }

    /// <summary>
    /// 유저 메일 DB - mailId의 IsExpired = false로 변경하는 메서드
    /// </summary>
    /// <param name="mailId">메일 ID</param>
    public async void SetIsExpired(string mailId)
    {
        string uid = FirebaseManager.Auth.CurrentUser.UserId;
        await Manager.DB.SetMailIsExpireddAsync(mailId, false);
    }

    /// <summary>
    /// 유저 메일 DB에서 해당 mailId를 삭제하는 메서드
    /// </summary>
    /// <param name="mailId">메일 ID</param>
    public async Task DeleteMail(string mailId)
    {
        await Manager.DB.DeleteMailAsync(mailId);
    }

    private void StartListeningToMailData()
    {
        string uid = FirebaseManager.Auth.CurrentUser.UserId;
        _mailRef = FirebaseManager.DataReference.Child("UserData").Child(uid).Child("MailData");

        _mailRef.ValueChanged += OnMailChanged;
    }

    private void StopListeningToMailData()
    {
        _mailRef.ValueChanged -= OnMailChanged;
    }

    private async void OnMailChanged(object sender, ValueChangedEventArgs changeEvent)
    {
        if (changeEvent.DatabaseError != null)
        {
            Debug.LogError($"[Mail DB Error] {changeEvent.DatabaseError.Message}");
            return;
        }

        List<MailData> mailList = await Manager.DB.LoadUserMailsAsync();
        Debug.Log("[PlayerMailBoxController] OnMailChanged 실행");
        // ReceivedDate 기준 내림차순 정렬
        //mailList.Sort((a, b) => b.ReceivedDate.CompareTo(a.ReceivedDate));

        var loaded = await LoadAsync();
        RefreshUI(loaded);
    }
}
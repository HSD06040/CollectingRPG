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

    public async void ReceiveReward(string mailId)
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

    public async void DeleteMail(string mailId)
    {
        // 해당 mailId 삭제
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

        OnMailboxUpdated?.Invoke(mailList);
        var loaded = await LoadAsync();
        RefreshUI(loaded);
    }
}

using Firebase.Database;
using Firebase.Extensions;
using UnityEngine;
using UnityEngine.UI;


public class DBService : MonoBehaviour
{

    public InputField inputSignUpID;
    public InputField inputSignUpPWD;

    public Text inputNickName;

    public InputField inputSignInID;
    public InputField inputSignInPWD;

    public GameObject noticeMessageObject;
    public Text noticeMessageText;

    public TitleUIManager titleUIManager;

    public GameObject signUIObject;

    public class PlayerInfo
    {
        public string nickName;
        public string pwd;

        public int gold;

        public string useWeapon;

        public int ATKUP;
        public int HPUP;
        public int ATKSPDUP;
        public int SPDUP;

        public int connectGameCnt;
        public PlayerInfo() { }

        public PlayerInfo(string pwd,string nickName)
        {
            this.pwd = pwd;
            this.nickName = nickName;

            ATKUP = 0;
            HPUP = 0;
            ATKSPDUP = 0;
            SPDUP = 0;

            gold = 5000;
            useWeapon = "SHOTGUN";

            connectGameCnt = 0;
        }

        public void getCoin(int value)
        {
            this.gold += value;
        }
    }



    DatabaseReference reference;

    private void OnEnable()
    {
        reference = FirebaseDatabase.DefaultInstance.RootReference;
    }

    public void checkIDAndCreate() // ȸ������
    {
        string id = inputSignUpID.text;
        string pwd = inputSignUpPWD.text;

        if (id == "" || pwd == "" || inputNickName.text == "")
        {
            noticeMessage("�߸��� �Է��Դϴ�.");
            return;
        }

        reference.Child("users").Child(id).GetValueAsync().ContinueWithOnMainThread(task =>
        {
            if (task.IsCompleted)
            {
                DataSnapshot snapshot = task.Result;
                if (snapshot.Exists)
                {
                    noticeMessage("�̹� �����ϴ� ���̵��Դϴ�.");
                }
                else
                {
                    noticeMessage("���̵� ���� �Ϸ�!");
                    PlayerInfo playerInfo = new PlayerInfo(pwd, inputNickName.text);
                    string json = JsonUtility.ToJson(playerInfo);
                    reference.Child("users").Child(id).SetRawJsonValueAsync(json);
                }
            }
            else
            {
                noticeMessage("���� ���� ����");
            }
        });
    }

    public void LogInCheckIDPWD()
    {

        string id = inputSignInID.text;
        string pwd = inputSignInPWD.text;

        reference.Child("users").Child(id).GetValueAsync().ContinueWithOnMainThread(task =>
        {
            if (task.IsCompleted)
            {
                DataSnapshot snapshot = task.Result;
                if (snapshot.Exists)
                {
                    Debug.Log(snapshot.Child("pwd").GetRawJsonValue() + " , " + pwd);



                    if(snapshot.Child("pwd").GetRawJsonValue() == '"' + pwd + '"')
                    {
                        noticeMessage("�α��� ����\n������ �����մϴ�.");
                        PlayerPrefs.SetString("PlayerID", id);
                        signUIObject.SetActive(false);
                        titleUIManager.signSuccess();

                    }
                    else
                    {
                        noticeMessage("�н����尡 Ʋ���ϴ�.");
                    }
                }
                else
                {
                    noticeMessage("�������� �ʴ� ���̵� �Դϴ�.");
                }
            }
            else
            {
                Debug.LogError("���� ���� ����");
            }
        });
    }

    public void noticeMessage(string message)
    {
        noticeMessageText.text = message;
        noticeMessageObject.SetActive(true);
    }

}

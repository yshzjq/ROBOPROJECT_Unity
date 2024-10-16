
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

    public void checkIDAndCreate() // 회원가입
    {
        string id = inputSignUpID.text;
        string pwd = inputSignUpPWD.text;

        if (id == "" || pwd == "" || inputNickName.text == "")
        {
            noticeMessage("잘못된 입력입니다.");
            return;
        }

        reference.Child("users").Child(id).GetValueAsync().ContinueWithOnMainThread(task =>
        {
            if (task.IsCompleted)
            {
                DataSnapshot snapshot = task.Result;
                if (snapshot.Exists)
                {
                    noticeMessage("이미 존재하는 아이디입니다.");
                }
                else
                {
                    noticeMessage("아이디 생성 완료!");
                    PlayerInfo playerInfo = new PlayerInfo(pwd, inputNickName.text);
                    string json = JsonUtility.ToJson(playerInfo);
                    reference.Child("users").Child(id).SetRawJsonValueAsync(json);
                }
            }
            else
            {
                noticeMessage("서버 접속 실패");
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
                        noticeMessage("로그인 성공\n게임을 시작합니다.");
                        PlayerPrefs.SetString("PlayerID", id);
                        signUIObject.SetActive(false);
                        titleUIManager.signSuccess();

                    }
                    else
                    {
                        noticeMessage("패스워드가 틀립니다.");
                    }
                }
                else
                {
                    noticeMessage("존재하지 않는 아이디 입니다.");
                }
            }
            else
            {
                Debug.LogError("서버 접속 실패");
            }
        });
    }

    public void noticeMessage(string message)
    {
        noticeMessageText.text = message;
        noticeMessageObject.SetActive(true);
    }

}

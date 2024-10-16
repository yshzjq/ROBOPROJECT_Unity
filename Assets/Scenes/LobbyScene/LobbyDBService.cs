using Firebase;
using Firebase.Database;
using Firebase.Extensions;

using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class LobbyDBService : MonoBehaviour
{

    public Text nickNameText;

    public Text LifeValueText;
    public Text ATKValueText;
    public Text ATKSPDText;
    public Text SpeedText;

    public Text weaponNameText;

    public Text playerGetGoldText;

    public string[] weaponNames;
    public GameObject[] weaponObject;

    public GameObject firstReward;

    public GameObject abilityUI;

    DatabaseReference reference;


    private void OnEnable()
    {
        reference = FirebaseDatabase.DefaultInstance.RootReference;

        string id = PlayerPrefs.GetString("PlayerID");

        reference.Child("users").Child(id).GetValueAsync().ContinueWithOnMainThread(task =>
        {
            if (task.IsCompleted)
            {
                

                DataSnapshot snapshot = task.Result;

                if(snapshot.Child("connectGameCnt").GetRawJsonValue() == "0")
                {
                    firstReward.SetActive(true);
                    reference.Child("users").Child(id).Child("connectGameCnt").SetRawJsonValueAsync("1");
                }

                nickNameText.text = snapshot.Child("nickName").Value.ToString();

                weaponNameText.text = snapshot.Child("useWeapon").GetRawJsonValue().Replace("\"", "").Trim(); ;
                playerGetGoldText.text = snapshot.Child("gold").GetRawJsonValue();


                int num = 0;
                foreach (var weaponname in weaponNames)
                {

                    if (weaponname == weaponNameText.text)
                    {
                        weaponObject[num].SetActive(true);
                    }
                    num++;
                }

                abilityUI.gameObject.SetActive(true);

            }
            else
            {
                SceneManager.LoadScene("Title Scene"); // 연결이 끊길때
            }
        });

        
    }


}

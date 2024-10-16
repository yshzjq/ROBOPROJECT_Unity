using Firebase.Database;
using Firebase.Extensions;

using UnityEngine;
using UnityEngine.UI;

using UnityEngine.SceneManagement;
public class VictoryManager : MonoBehaviour
{
    public Text getScoreText;
    public Text getGoldText;

    int gold;
    int score;

    DatabaseReference reference;

    void OnEnable()
    {
        reference = FirebaseDatabase.DefaultInstance.RootReference;

        gold = PlayerPrefs.GetInt("getScore");
        score = PlayerPrefs.GetInt("getCoin");

        getGoldText.text = "  Gold : " + gold;
        getScoreText.text = "Score : " + score;

    }


    void Start()
    {
        reference.Child("users").Child(PlayerPrefs.GetString("PlayerID")).GetValueAsync().ContinueWithOnMainThread(task =>
        {
            if (task.IsCompleted)
            {
                DataSnapshot snapshot = task.Result;
                if (snapshot.Exists)
                {
                    // 돈을 더하자
                    gold += int.Parse(snapshot.Child("gold").GetRawJsonValue());
                    reference.Child("users").Child(PlayerPrefs.GetString("PlayerID")).Child("gold").SetRawJsonValueAsync(gold.ToString());

                }
                else
                {

                }
            }
            else
            {
                //서버 접속 오류
                SceneManager.LoadScene("Title Scene");
            }
        });
    }

    public void RestartBTN()
    {
        SceneManager.LoadScene("Main Scene");
    }

    public void QUITBTN()
    {
        SceneManager.LoadScene("Lobby Scene");
    }
}

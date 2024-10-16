
using Firebase.Database;
using Firebase.Extensions;

using UnityEngine;

using UnityEngine.SceneManagement;

public class SelectManager : MonoBehaviour
{

    public GameObject[] btns;
    public string[] weapons;

    private string useWeapon;

    public DatabaseReference reference;

    private void OnEnable()
    {
        reference = FirebaseDatabase.DefaultInstance.RootReference;

        reference.Child("users").Child(PlayerPrefs.GetString("PlayerID")).GetValueAsync().ContinueWithOnMainThread(task =>
        {
            if (task.IsCompleted)
            {

                DataSnapshot snapshot = task.Result;

                for (int i = 0; i < btns.Length; i++)
                {
                    if (snapshot.Child("useWeapon").GetRawJsonValue() == '"' + weapons[i]+ '"') btns[i].SetActive(false);
                    else btns[i].SetActive(true);
                }

            }
            else
            {
                SceneManager.LoadScene("Title Scene"); // 연결이 끊길때
            }
        });
    }


    public void SelectBtn(int weaponNum)
    {
        foreach (var btn in btns)
        {
            btn.SetActive(true);
        }
        btns[weaponNum].gameObject.SetActive(false);

        reference.Child("users").Child(PlayerPrefs.GetString("PlayerID")).Child("useWeapon").SetRawJsonValueAsync('"' + weapons[weaponNum] + '"');

    }

    public void GotoLobby()
    {
        SceneManager.LoadScene("Lobby Scene");
    }
}

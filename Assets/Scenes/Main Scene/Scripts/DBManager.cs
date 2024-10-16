using Firebase.Database;

using UnityEngine;

using Firebase.Extensions;


public class DBManager : MonoBehaviour
{
    public int playerADDATK;
    public int playerADDHP;
    public float playerADDATKSPD;
    public float playerADDSPD;

    public string PlayerBaseWeapon;

    


    public static DBManager instance 
    {
        get
        {
            if(m_instance == null)
            {
               m_instance = FindObjectOfType<DBManager>();
            }

            return m_instance;
        }
    }
    private static DBManager m_instance;

    DatabaseReference reference;

    private void OnEnable()
    {
        reference = FirebaseDatabase.DefaultInstance.RootReference;

        reference.Child("users").Child(PlayerPrefs.GetString("PlayerID")).GetValueAsync().ContinueWithOnMainThread(task =>
        {
            if (task.IsCompleted)
            {
                DataSnapshot snapshot = task.Result;
                if (snapshot.Exists)
                {
                    PlayerPrefs.SetInt("AttackLevel", int.Parse(snapshot.Child("ATKUP").GetRawJsonValue()));
                    PlayerPrefs.SetInt("HPLevel",int.Parse(snapshot.Child("HPUP").GetRawJsonValue()));
                    PlayerPrefs.SetInt("SpeedLevel", int.Parse(snapshot.Child("SPDUP").GetRawJsonValue()));
                    PlayerPrefs.SetInt("ATKSpeedLevel", int.Parse(snapshot.Child("ATKSPDUP").GetRawJsonValue()));

                    PlayerPrefs.SetString("UseWeapon", snapshot.Child("useWeapon").GetRawJsonValue().Trim('"').ToString());
                }
                else
                {
                    Debug.Log("존재 노");
                }
            }
            else
            {
                Debug.LogError("서버 접속 실패");
            }
        });
    }
}

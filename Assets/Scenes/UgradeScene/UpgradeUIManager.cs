using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

using Firebase.Database;
using Firebase.Extensions;


public class UpgradeUIManager : MonoBehaviour
{

    public GameObject[] maxObject;

    // 싱글턴
    public static UpgradeUIManager instance
    {
        get
        {
            if(m_instance == null)
            {
                m_instance = FindObjectOfType<UpgradeUIManager>();
            }
            return m_instance;
        }
    }
    private static UpgradeUIManager m_instance;

    public GameObject noticeMessageText;

    //게임 내 저장되어 있는 레벨들
    private int attackLevel;
    private int hpLevel;
    private int atkSpeedLevel;
    private int speedLevel;

    private int[] levelArray;

    // 현재 레벨 텍스트
    public Text attackLevelText;
    public Text hpLevelText;
    public Text atkSpeedLevelText;
    public Text speedLevelText;

    private Text[] levelTexts;

    public Text attackLevelNeedGoldText;
    public Text hpLevelNeedGoldText;
    public Text atkSpeedNeedGoldText;
    public Text speedNeedGoldText;

    private Text[] needGoldTexts;

    // 가지고 있는 돈 텍스트
    public Text playerGetMoney;

    //업그레이드 종류
    public string[] upgradeString;


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
                    //여기서 값을 할당

                    attackLevel = int.Parse(snapshot.Child(upgradeString[0]).GetRawJsonValue());
                    hpLevel = int.Parse(snapshot.Child(upgradeString[1]).GetRawJsonValue());
                    atkSpeedLevel = int.Parse(snapshot.Child(upgradeString[2]).GetRawJsonValue());
                    speedLevel = int.Parse(snapshot.Child(upgradeString[3]).GetRawJsonValue());

                    if (attackLevel >= 5) maxObject[0].SetActive(true);
                    if (hpLevel >= 5) maxObject[1].SetActive(true);
                    if (atkSpeedLevel >= 5) maxObject[2].SetActive(true);
                    if (speedLevel >= 5) maxObject[3].SetActive(true);

                    playerGetMoney.text = snapshot.Child("gold").GetRawJsonValue().Replace("\"", "").Trim();

                    attackLevelText.text = "LEVEL " + attackLevel.ToString();
                    hpLevelText.text = "LEVEL " + hpLevel.ToString();
                    atkSpeedLevelText.text = "LEVEL " + atkSpeedLevel.ToString();
                    speedLevelText.text = "LEVEL " + speedLevel.ToString();

                    attackLevelNeedGoldText.text = "Gold : " + 100 * Mathf.Pow(2, attackLevel) + "G";
                    hpLevelNeedGoldText.text = "Gold : " + 100 * Mathf.Pow(2, hpLevel) + "G";
                    atkSpeedNeedGoldText.text = "Gold : " + 100 * Mathf.Pow(2, atkSpeedLevel) + "G";
                    speedNeedGoldText.text = "Gold : " + 100 * Mathf.Pow(2, speedLevel) + "G";

                    levelArray = new int[4] { attackLevel, hpLevel, atkSpeedLevel, speedLevel };
                    needGoldTexts = new Text[4] { attackLevelNeedGoldText, hpLevelNeedGoldText, atkSpeedNeedGoldText, speedNeedGoldText };
                    levelTexts = new Text[4] { attackLevelText, hpLevelText, atkSpeedLevelText, speedLevelText };
                }
                else
                {
                    //존재하지 않음
                    //그런 경우는 없긴하다.
                }
            }
            else
            {
                //서버 접속 오류
                SceneManager.LoadScene("Title Scene");
            }
        });

        


        

    }

 


    public void UpgradeAbility(int abilityType)
    {



        reference.Child("users").Child(PlayerPrefs.GetString("PlayerID")).GetValueAsync().ContinueWithOnMainThread(task =>
        {
            if (task.IsCompleted)
            {
                DataSnapshot snapshot = task.Result;
                if (snapshot.Exists)
                {
                    //돈을 계산해보자
                    int playerGetMoneyValue = int.Parse(snapshot.Child("gold").GetRawJsonValue());
                    int needGetMoney = (int)(100 * Mathf.Pow(2, levelArray[abilityType]));

                    if (playerGetMoneyValue >= needGetMoney) // 조건 충족시
                    {
                        levelArray[abilityType] += 1; //레벨 업

                        reference.Child("users").Child(PlayerPrefs.GetString("PlayerID")).Child("gold").SetRawJsonValueAsync((playerGetMoneyValue - needGetMoney).ToString()); // DB 골드 갱신

                        reference.Child("users").Child(PlayerPrefs.GetString("PlayerID")).Child(upgradeString[abilityType]).SetRawJsonValueAsync(levelArray[abilityType].ToString()); //DB 레벨 갱신

                        if (levelArray[abilityType] >= 5) // 5이상시 잠금
                        {
                            maxObject[abilityType].SetActive(true);
                        }

                        levelTexts[abilityType].text = "LEVEL " + levelArray[abilityType];
                        needGoldTexts[abilityType].text = "Gold : " + 100 * Mathf.Pow(2, levelArray[abilityType]) + "G";
                        playerGetMoney.text = (playerGetMoneyValue - needGetMoney).ToString();


                    }
                    else
                    {
                        NoticeMessages("돈이 부족합니다.");
                    }
                }
                else
                {
                    //존재하지 않음
                    //그런 경우는 없긴하다.
                }
            }
            else
            {
                //서버 접속 오류
                SceneManager.LoadScene("Title Scene");
            }
        });
    }

    public void NoticeMessages(string str) // 안내 문자
    {
        noticeMessageText.GetComponentInChildren<Text>().text = str;
        noticeMessageText.SetActive(true);
    }

    public void gotoLobbyScene()
    {
        SceneManager.LoadScene("Lobby Scene");
    }
}
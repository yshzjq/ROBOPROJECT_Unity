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

    // �̱���
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

    //���� �� ����Ǿ� �ִ� ������
    private int attackLevel;
    private int hpLevel;
    private int atkSpeedLevel;
    private int speedLevel;

    private int[] levelArray;

    // ���� ���� �ؽ�Ʈ
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

    // ������ �ִ� �� �ؽ�Ʈ
    public Text playerGetMoney;

    //���׷��̵� ����
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
                    //���⼭ ���� �Ҵ�

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
                    //�������� ����
                    //�׷� ���� �����ϴ�.
                }
            }
            else
            {
                //���� ���� ����
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
                    //���� ����غ���
                    int playerGetMoneyValue = int.Parse(snapshot.Child("gold").GetRawJsonValue());
                    int needGetMoney = (int)(100 * Mathf.Pow(2, levelArray[abilityType]));

                    if (playerGetMoneyValue >= needGetMoney) // ���� ������
                    {
                        levelArray[abilityType] += 1; //���� ��

                        reference.Child("users").Child(PlayerPrefs.GetString("PlayerID")).Child("gold").SetRawJsonValueAsync((playerGetMoneyValue - needGetMoney).ToString()); // DB ��� ����

                        reference.Child("users").Child(PlayerPrefs.GetString("PlayerID")).Child(upgradeString[abilityType]).SetRawJsonValueAsync(levelArray[abilityType].ToString()); //DB ���� ����

                        if (levelArray[abilityType] >= 5) // 5�̻�� ���
                        {
                            maxObject[abilityType].SetActive(true);
                        }

                        levelTexts[abilityType].text = "LEVEL " + levelArray[abilityType];
                        needGoldTexts[abilityType].text = "Gold : " + 100 * Mathf.Pow(2, levelArray[abilityType]) + "G";
                        playerGetMoney.text = (playerGetMoneyValue - needGetMoney).ToString();


                    }
                    else
                    {
                        NoticeMessages("���� �����մϴ�.");
                    }
                }
                else
                {
                    //�������� ����
                    //�׷� ���� �����ϴ�.
                }
            }
            else
            {
                //���� ���� ����
                SceneManager.LoadScene("Title Scene");
            }
        });
    }

    public void NoticeMessages(string str) // �ȳ� ����
    {
        noticeMessageText.GetComponentInChildren<Text>().text = str;
        noticeMessageText.SetActive(true);
    }

    public void gotoLobbyScene()
    {
        SceneManager.LoadScene("Lobby Scene");
    }
}
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class FirstReward : MonoBehaviour
{

    private int firstConnectGame;

    public GameObject noticeRewardMessage;

    public Text weaponUIText;

    public GameObject[] eqquipedWeapon;
    public string[] weaponName;

    private void Awake()
    {
       // firstConnectGame = PlayerPrefs.GetInt("gameConnectCount");

        //if (firstConnectGame == 0)
        //{
        //    PlayerPrefs.SetString("EquippedSpecialWeapon",weaponName[0]);
        //}
    }
    private void OnEnable()
    {
       // weaponUIText.text = PlayerPrefs.GetString("EquippedSpecialWeapon");
    }

    private void Start()
    {
        
        //if(firstConnectGame == 0 )
        //{
        //    PlayerPrefs.SetInt("PlayerGetMoney", PlayerPrefs.GetInt("PlayerGetMoney") + 5000);

        //    PlayerPrefs.SetString("EquippedSpecialWeapon", weaponName[0]);

        //    noticeRewardMessage.SetActive(true);
        //}

        //firstConnectGame++;
        //PlayerPrefs.SetInt("gameConnectCount", firstConnectGame);
    }
}

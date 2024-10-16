using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class LobbySceneManager : MonoBehaviour
{
    AudioSource audio;
    public AudioClip clickAudio;

    public GameObject[] useWeaponObject;
    public GameObject[] roboWeaponObject;
    string[] weaponName;

    private void Awake()
    {
        audio = GetComponent<AudioSource>();
    }

    public void goToLevelScene()
    {
        clickbtnSound();
        SceneManager.LoadScene("Level Scene");
    }

    public void goToWeaponChangeScene()
    {
        clickbtnSound();
        SceneManager.LoadScene("WeaponChangeScene");
    }

    public void goToUpgradeScene()
    {
        clickbtnSound();
        SceneManager.LoadScene("Upgrade Scene");
    }

    void clickbtnSound()
    {
        audio.PlayOneShot(clickAudio);
    }
}

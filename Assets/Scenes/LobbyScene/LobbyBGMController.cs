using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LobbyBGMController : MonoBehaviour
{
    AudioSource backmusic;

    void Awake()
    {
        backmusic = FindObjectOfType<LobbyBGMController>().GetComponent<AudioSource>(); //배경음악 저장해둠
        if (backmusic.isPlaying) return; //배경음악이 재생되고 있다면 패스
        else
        {
            backmusic.Play();
            DontDestroyOnLoad(gameObject); //배경음악 계속 재생하게(이후 버튼매니저에서 조작)
        }
    }

    public void BackGroundMusicOffButton() //배경음악 키고 끄는 버튼
    {
        backmusic = FindObjectOfType<LobbyBGMController>().GetComponent<AudioSource>();

        if (backmusic != null)
        {
            backmusic.Pause();
            Destroy(backmusic.gameObject);
        }
    }
}

using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LobbyBGMController : MonoBehaviour
{
    AudioSource backmusic;

    void Awake()
    {
        backmusic = FindObjectOfType<LobbyBGMController>().GetComponent<AudioSource>(); //������� �����ص�
        if (backmusic.isPlaying) return; //��������� ����ǰ� �ִٸ� �н�
        else
        {
            backmusic.Play();
            DontDestroyOnLoad(gameObject); //������� ��� ����ϰ�(���� ��ư�Ŵ������� ����)
        }
    }

    public void BackGroundMusicOffButton() //������� Ű�� ���� ��ư
    {
        backmusic = FindObjectOfType<LobbyBGMController>().GetComponent<AudioSource>();

        if (backmusic != null)
        {
            backmusic.Pause();
            Destroy(backmusic.gameObject);
        }
    }
}

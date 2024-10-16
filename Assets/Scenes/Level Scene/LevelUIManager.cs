using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class LevelUIManager : MonoBehaviour
{
    int selectChapter = 0;

    public string[] chapterScene;
    LobbyBGMController backmusic;

    private void Awake()
    {
        backmusic = FindObjectOfType<LobbyBGMController>();
    }

    public void SelectChapter(int chapterNum)
    {
        selectChapter = chapterNum;
    }

    public void gotoChapter()
    {
        if (selectChapter == 0)
        {
            return;
        }
        else
        {
            backmusic.BackGroundMusicOffButton();
            SceneManager.LoadScene(chapterScene[selectChapter - 1]);
        }
    }
    
    public void goLobbyScene()
    {
        SceneManager.LoadScene("Lobby Scene");
    }
}

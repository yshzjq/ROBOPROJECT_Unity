using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class UIManagerScripts : MonoBehaviour
{

    // 싱글톤 접근용 프로퍼티
    public static UIManagerScripts instance
    {
        get
        {
            if (m_instance == null)
            {
                m_instance = FindObjectOfType<UIManagerScripts>();
            }

            return m_instance;
        }
    }

    private static UIManagerScripts m_instance; // 싱글톤이 할당될 변수

    //public Text ammoText; // 탄약 표시용 텍스트
    public Text scoreText; // 점수 표시용 텍스트
    public Text goldText;
    public Text stageText; // 적 웨이브 표시용 텍스트
    public Text enemyCountText;

    public GameObject youDIeUI;

    public GameObject victoryUI;

    private float scoreBlank = 6;
    private float coinBlank = 6;

    //게임 퍼즈 UI
    public GameObject PauseUI;

    public GameObject BossHPUI;

    // 탄약 텍스트 갱신
    /*public void UpdateAmmoText(int magAmmo, int remainAmmo)
    {
        ammoText.text = magAmmo + "/" + remainAmmo;
    }*/

    private void OnEnable()
    {
        PauseUI.SetActive(false);
        BossHPUI.SetActive(false);
    }

    // 점수 텍스트 갱신
    public void UpdateScoreText(int newScore)
    {
        scoreText.text = "Score : ";
        while (true)
        {
            if (newScore / Mathf.Pow(10f, scoreBlank) >= 1)
            {
                break;
            }
            scoreBlank--;
            scoreText.text += "0";
            
        }
        scoreText.text += newScore;
        scoreBlank = 6;
    }

    public void UpdateCoinText(int newCoin)
    {
        goldText.text = "Coin : ";
        while (true)
        {
            if (newCoin / Mathf.Pow(10f, coinBlank) >= 1)
            {
                break;
            }
            coinBlank--;
            goldText.text += "0";

        }
        goldText.text += newCoin;
        coinBlank = 6;
    }

    




    // 적 웨이브 텍스트 갱신
    public void UpdateWaveText(int stages)
    {
        stageText.text = "Stage " + stages;
    }

    public void UpdateEnemyCountText(int enemyCount)
    {
        enemyCountText.text = "Left Enemy : " + enemyCount;
    }

    // 게임 오버 UI 활성화

    public void SetActiveYouDieUI(bool active)
    {
        youDIeUI.SetActive(active);
    }

    public void SetActiveVictoryUI(bool active)
    {
        GameManager.instance.RecordValues();

        victoryUI.SetActive(active);
    }

    // 게임 재시작
    public void GameRestart()
    {
        Time.timeScale = 1f;
        SceneManager.LoadScene(SceneManager.GetActiveScene().name);
    }

    public void PauseUIAppear()
    {
        PauseUI.SetActive(true);
        Time.timeScale = 0f;
    }

    public void PauseUIDisAppear()
    {
        Time.timeScale = 1f;
        PauseUI.SetActive(false);
    }

    public void ExitGame()
    {
        SceneManager.LoadScene("Lobby Scene");
    }

    public Slider BossHPUIAppear()
    {
        BossHPUI.SetActive(true);
        return BossHPUI.GetComponent<Slider>();
    }
}

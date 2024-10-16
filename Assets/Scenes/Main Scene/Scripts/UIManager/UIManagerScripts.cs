using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class UIManagerScripts : MonoBehaviour
{

    // �̱��� ���ٿ� ������Ƽ
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

    private static UIManagerScripts m_instance; // �̱����� �Ҵ�� ����

    //public Text ammoText; // ź�� ǥ�ÿ� �ؽ�Ʈ
    public Text scoreText; // ���� ǥ�ÿ� �ؽ�Ʈ
    public Text goldText;
    public Text stageText; // �� ���̺� ǥ�ÿ� �ؽ�Ʈ
    public Text enemyCountText;

    public GameObject youDIeUI;

    public GameObject victoryUI;

    private float scoreBlank = 6;
    private float coinBlank = 6;

    //���� ���� UI
    public GameObject PauseUI;

    public GameObject BossHPUI;

    // ź�� �ؽ�Ʈ ����
    /*public void UpdateAmmoText(int magAmmo, int remainAmmo)
    {
        ammoText.text = magAmmo + "/" + remainAmmo;
    }*/

    private void OnEnable()
    {
        PauseUI.SetActive(false);
        BossHPUI.SetActive(false);
    }

    // ���� �ؽ�Ʈ ����
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

    




    // �� ���̺� �ؽ�Ʈ ����
    public void UpdateWaveText(int stages)
    {
        stageText.text = "Stage " + stages;
    }

    public void UpdateEnemyCountText(int enemyCount)
    {
        enemyCountText.text = "Left Enemy : " + enemyCount;
    }

    // ���� ���� UI Ȱ��ȭ

    public void SetActiveYouDieUI(bool active)
    {
        youDIeUI.SetActive(active);
    }

    public void SetActiveVictoryUI(bool active)
    {
        GameManager.instance.RecordValues();

        victoryUI.SetActive(active);
    }

    // ���� �����
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

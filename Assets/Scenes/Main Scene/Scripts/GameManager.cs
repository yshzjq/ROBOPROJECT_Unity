using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.SceneManagement;

public class GameManager : MonoBehaviour
{
    public static GameManager instance
    {
        get
        {
            if (m_instance == null)
            {
                m_instance = FindObjectOfType<GameManager>();
            }

            return m_instance;
        }
    }

    private static GameManager m_instance; // 싱글톤이 할당될 static 변수

    private int score = 0; // 현재 게임 점수
    int gold = 0;
    public bool isGameover { get; private set; } // 게임 오버 상태

    private void Awake()
    {
        // 씬에 싱글톤 오브젝트가 된 다른 GameManager 오브젝트가 있다면
        if (instance != this)
        {
            // 자신을 파괴
            Destroy(gameObject);
        }
    }

    private void Start()
    {
        PlayerPrefs.SetInt("getScore", 0);
        PlayerPrefs.SetInt("getCoin", 0);

        // 플레이어 캐릭터의 사망 이벤트 발생시 게임 오버
        FindObjectOfType<PlayerController>().onDeath += EndGame;
    }

    // 점수를 추가하고 UI 갱신
    public void AddScore(int newScore)
    {
        // 게임 오버가 아닌 상태에서만 점수 증가 가능
        if (!isGameover)
        {
            // 점수 추가
            score += newScore;
            // 점수 UI 텍스트 갱신
            UIManagerScripts.instance.UpdateScoreText(score);
        }
    }

    public void AddCoin(int newCoin)
    {
        if (!isGameover)
        {
            // 점수 추가
            gold += newCoin;
            // 점수 UI 텍스트 갱신
            UIManagerScripts.instance.UpdateCoinText(gold);
        }
    }

    public void RecordValues()
    {
        PlayerPrefs.SetInt("getScore", score);
        PlayerPrefs.SetInt("getCoin", gold);
    }

    // 게임 오버 처리
    public void EndGame()
    {
        RecordValues();



        // 게임 오버 상태를 참으로 변경
        isGameover = true;
        // 게임 오버 UI를 활성화
        UIManagerScripts.instance.SetActiveYouDieUI(true);

        StartCoroutine("goToGameOver");
    }

    IEnumerator goToGameOver()
    {
        yield return new WaitForSeconds(3f);

        SceneManager.LoadScene("GameOverScene");

    }
}

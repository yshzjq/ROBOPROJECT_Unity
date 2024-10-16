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

    private static GameManager m_instance; // �̱����� �Ҵ�� static ����

    private int score = 0; // ���� ���� ����
    int gold = 0;
    public bool isGameover { get; private set; } // ���� ���� ����

    private void Awake()
    {
        // ���� �̱��� ������Ʈ�� �� �ٸ� GameManager ������Ʈ�� �ִٸ�
        if (instance != this)
        {
            // �ڽ��� �ı�
            Destroy(gameObject);
        }
    }

    private void Start()
    {
        PlayerPrefs.SetInt("getScore", 0);
        PlayerPrefs.SetInt("getCoin", 0);

        // �÷��̾� ĳ������ ��� �̺�Ʈ �߻��� ���� ����
        FindObjectOfType<PlayerController>().onDeath += EndGame;
    }

    // ������ �߰��ϰ� UI ����
    public void AddScore(int newScore)
    {
        // ���� ������ �ƴ� ���¿����� ���� ���� ����
        if (!isGameover)
        {
            // ���� �߰�
            score += newScore;
            // ���� UI �ؽ�Ʈ ����
            UIManagerScripts.instance.UpdateScoreText(score);
        }
    }

    public void AddCoin(int newCoin)
    {
        if (!isGameover)
        {
            // ���� �߰�
            gold += newCoin;
            // ���� UI �ؽ�Ʈ ����
            UIManagerScripts.instance.UpdateCoinText(gold);
        }
    }

    public void RecordValues()
    {
        PlayerPrefs.SetInt("getScore", score);
        PlayerPrefs.SetInt("getCoin", gold);
    }

    // ���� ���� ó��
    public void EndGame()
    {
        RecordValues();



        // ���� ���� ���¸� ������ ����
        isGameover = true;
        // ���� ���� UI�� Ȱ��ȭ
        UIManagerScripts.instance.SetActiveYouDieUI(true);

        StartCoroutine("goToGameOver");
    }

    IEnumerator goToGameOver()
    {
        yield return new WaitForSeconds(3f);

        SceneManager.LoadScene("GameOverScene");

    }
}

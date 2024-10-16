using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class TitleUIManager : MonoBehaviour
{
    AudioSource audio;
    public AudioClip gotoLobbyAudio;

    public CanvasRenderer fadeInOutObject;
    public GameObject titleObject;

    public Text gameStartText;

    public float fadeInDelayTime;

    public float textBlinkDelayTIme;

    private float fadeValue;

    bool fadeInComplete = false;

    bool gameStartTouch = false;


    //·Î±×ÀÎ
    public GameObject titleUI;
    public GameObject signInUI;
    public GameObject signUpUi;

    private void Awake()
    {
        audio = GetComponent<AudioSource>();
        fadeInOutObject.gameObject.SetActive(true);
        titleObject.SetActive(false);
    }

    private void Start()
    {
        fadeValue = 1f;

        StartCoroutine("FadeInTitle");
    }

    private void Update()
    {
        if(fadeInComplete == true && Input.GetKeyDown(KeyCode.Mouse0))
        {
            gameStartTouch = true;
            return;
        }
    }

    IEnumerator FadeInTitle()
    {

        yield return new WaitForSeconds(2f);

        while (fadeValue > 0f)
        {
            yield return new WaitForSeconds(fadeInDelayTime);
            fadeInOutObject.SetAlpha(fadeValue);
            fadeValue-=Time.deltaTime;

        }
        fadeInOutObject.gameObject.SetActive(false);


        titleObject.SetActive(true);

        yield return new WaitForSeconds(5f);
        StartCoroutine("StartTextBlink");

        yield return new WaitForSeconds(1f);
        fadeInComplete = true;
        
    }

    IEnumerator StartTextBlink()
    {
        float textAlphaValue = 1f;
        bool textFade = false;
        while (true)
        {
            if(fadeInComplete == true && gameStartTouch == true)
            {
                audio.PlayOneShot(gotoLobbyAudio);

                gameStartText.color = new Color(gameStartText.color.r, gameStartText.color.g, gameStartText.color.b,0f);
                for (int i = 0;i<2;i++)
                {
                    yield return new WaitForSeconds(0.3f);
                    gameStartText.color = new Color(gameStartText.color.r, gameStartText.color.g, gameStartText.color.b, 1f);
                    yield return new WaitForSeconds(0.3f);
                    gameStartText.color = new Color(gameStartText.color.r, gameStartText.color.g, gameStartText.color.b, 0f);
                }
                yield return new WaitForSeconds(0.3f);
                gameStartText.color = new Color(gameStartText.color.r, gameStartText.color.g, gameStartText.color.b, 1f);
                yield return new WaitForSeconds(1f);

                titleUI.SetActive(false);
                signInUI.gameObject.SetActive(true);

                break;
            }
            else
            {
                if (textFade == false)
                {

                    textAlphaValue += Time.deltaTime;
                    if (textAlphaValue >= 1f) textFade = true;
                }
                else
                {
                    textAlphaValue -= Time.deltaTime;
                    if (textAlphaValue <= 0f) textFade = false;
                }
                yield return new WaitForSeconds(textBlinkDelayTIme);

                gameStartText.color = new Color(gameStartText.color.r, gameStartText.color.g, gameStartText.color.b, textAlphaValue);
                
            }
        }
    }

    public IEnumerator FadeOutTitle()
    {
        fadeInOutObject.gameObject.SetActive(true);
        while (fadeValue <= 1f)
        {
            yield return new WaitForSeconds(fadeInDelayTime);
            fadeInOutObject.SetAlpha(fadeValue);
            fadeValue += Time.deltaTime;
        }

        SceneManager.LoadScene("Lobby Scene");
    }

    public void signSuccess()
    {
        titleObject.GetComponent<Animator>().SetTrigger("TitleDie");
        StartCoroutine("FadeOutTitle");
    }
}

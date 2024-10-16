using System.Collections;
using System.Collections.Generic;
using TMPro;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.UI;

public class PlayerController : MovingMachine
{
    public Slider HPSliderUI;

    private SkinnedMeshRenderer[] playerSkin;

    private float lastDamagedTime;

    public AudioClip deathClip;
    public AudioClip hitClip;
    public AudioClip itemGetSoundClip;

    private AudioSource playerAudio;
    private Animator playerAnimator;
    private PlayerInputMove playerInputMove;

    private CapsuleCollider playerDamagedCollider;

    public float damagedBetTime;

    // 플레이어 초기 능력치
    public int basePlayerHP;

    public Color damagedColor;

    // HP바 변경
    public Image backColor;
    public Image hpColor;

    //플레이어 능력치
    int player_hp { 
        get
        { 
            m_player_hp = basePlayerHP + (int)(basePlayerHP *(PlayerPrefs.GetInt("HPLevel")*0.1));
            return m_player_hp;
        } 
    }
    int m_player_hp;

    

    

    private void Awake()
    {
        playerAnimator = GetComponent<Animator>();
        playerAudio = GetComponent<AudioSource>();
        playerInputMove = GetComponent<PlayerInputMove>();

        playerSkin = GetComponentsInChildren<SkinnedMeshRenderer>();
        playerDamagedCollider = GetComponent<CapsuleCollider>();
    }

    public override void OnEnable()
    {
        start_HP = player_hp;


        base.OnEnable();

        lastDamagedTime = 0f;

        HPSliderUI.gameObject.SetActive(true);

        HPSliderUI.maxValue = start_HP;

        HPSliderUI.value = current_HP;

        playerInputMove.enabled = true;
    }

    public override void Damaged(int value)
    {

        

        if(damagedBetTime + lastDamagedTime < Time.time )
        {
            

            lastDamagedTime = Time.time;

            if (isDead == false)
            {
               // playerAudio.PlayOneShot(hitClip);
            }

            base.Damaged(value);

            HPSliderUI.value = current_HP;

            float changeValue = ((float)current_HP / (float)start_HP);

            hpColor.color = new Color(1f, changeValue, changeValue, 1f);
            backColor.color = new Color(1f, changeValue, changeValue, 0.3f);
            
            Debug.Log(hpColor.color + "  " + backColor.color);

            if(isDead == false) StartCoroutine(DamagedSkined(8, damagedBetTime));
        }

    }

    IEnumerator DamagedSkined(int count,float time)
    {
        playerDamagedCollider.excludeLayers = LayerMask.NameToLayer("Bullet");
        for(int i = 0;i<count;i++)
        { 
            playerDamagedColor();
            yield return new WaitForSeconds(time/count);
            playerBaseColor();
            yield return new WaitForSeconds(time/count);
        }
        playerDamagedCollider.excludeLayers -= LayerMask.NameToLayer("Bullet");
    }


    void playerDamagedColor()
    {
        foreach (var _playerSkin in playerSkin)
        {
            _playerSkin.material.SetColor("Color_c18aea2e3ad54319abb53f299507b005",damagedColor);
        }
    }

    void playerBaseColor()
    {
        foreach (var _playerSkin in playerSkin)
        {
            _playerSkin.material.SetColor("Color_c18aea2e3ad54319abb53f299507b005", new Color(1f,1f,1f,1f));
        }
    }



    public override void Die()
    {
        base.Die();

        HPSliderUI.gameObject.SetActive(false);

        playerAudio.PlayOneShot(deathClip);
        playerAnimator.SetTrigger("isDie");
    }

}

using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
public class CircleSlider : MonoBehaviour
{


	public Color newColor;
	public Color waitColor;
    public bool b=true;
	 Image image;

	public Image grimImage;
	 public float dashCoolDown = 5f;

	  public PlayerInputMove playerInputMove;

     float time =5f;
  
     public Text progress;

	
  
  void Start()
  {
	  
	image = GetComponent<Image>();
  }
  
    void Update()
    {
		if(b)
		{
			time+=Time.deltaTime;
			image.fillAmount= time / dashCoolDown;
			if(progress)
			{
				grimImage.color = waitColor;
				progress.text = (int)(dashCoolDown - time)+"S";
			}
			
			if(time>dashCoolDown)
			{
				grimImage.color = newColor;
                progress.text = "";
				playerInputMove.DashReady();
                b = false;
			}
		}
	}
	
	public void InputBtnDash()
	{
		if (b == false)
		{
			playerInputMove.InputDashButton();
			time = 0;
			b = true;
			
		}
	}
	
}

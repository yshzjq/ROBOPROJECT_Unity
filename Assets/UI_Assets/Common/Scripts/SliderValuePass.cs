using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class SliderValuePass : MonoBehaviour {

	Text progress;

	public int maxValue;

	// Use this for initialization
	void Awake () {
		progress = GetComponent<Text>();

	}
	
	public void UpdateProgress (float content) {

        progress.text = Mathf.Round(content).ToString() + " / " + maxValue;

	}


}

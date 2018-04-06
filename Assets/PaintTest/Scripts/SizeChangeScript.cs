using System;
using UnityEngine;
using UnityEngine.UI;

public class SizeChangeScript : MonoBehaviour {

	// Use this for initialization
	private void Awake () {
		FindObjectOfType<ColorPallete>().Fired += OnFiredInChangeSize;
	}

	private void OnFiredInChangeSize(Color newColor){
		this.GetComponent<Image>().color = newColor;
		// Debug.Log("From Size Change");
	}
}

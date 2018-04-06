using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using System.IO;

public class Screenshot : MonoBehaviour {

	[SerializeField] private CanvasGroup _bottom;
	[SerializeField] private CanvasGroup _me;
	private bool _takeScreenshot;
	private SceneController _reference;
	private void Start(){
		_reference = GameObject.FindGameObjectWithTag("SceneController").GetComponent<SceneController>();
		this.GetComponent<Button>().onClick.AddListener(() =>{
			_bottom.alpha = 0;
			_me.alpha = 0;
			_takeScreenshot = true;
			_reference.Screencapture = true;
		});
	}
	// onGUI - multiple times per frame
	private void OnGUI(){
		if(_takeScreenshot){

			string screenshotFilename = "screenshot.png";
			string screenShotPath = Application.persistentDataPath + "/" + screenshotFilename;
   
       		if (File.Exists(screenShotPath)) File.Delete(screenShotPath);
			ScreenCapture.CaptureScreenshot(screenshotFilename);
			var debugPath = Application.dataPath;
            int index = debugPath.LastIndexOf("/");
            debugPath = debugPath.Substring(0, index);
            screenShotPath = debugPath + "/screenshot.png";
			if (File.Exists(screenShotPath))
            		Debug.Log(debugPath);

			_takeScreenshot = false;
			_bottom.alpha = 1;
			_me.alpha = 1;
			_reference.Screencapture = false;
		}
	}
}

using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using System.IO;

public class Screenshot : MonoBehaviour {

	// [SerializeField] private CanvasGroup _bottom;
	// [SerializeField] private CanvasGroup _me;
	// private bool _takeScreenshot;
	// private PaintingScript _reference;
	public delegate Texture2D getTex();
	public static event getTex screenShotEvent;	
	private void Start(){
		// _reference = GameObject.FindGameObjectWithTag("PAINT").GetComponent<PaintingScript>();
		this.GetComponent<Button>().onClick.AddListener(() =>{
			// _bottom.alpha = 0;
			// _me.alpha = 0;
			// _takeScreenshot = true;
			// _reference.Screencapture = true;
			// PaintingScript.getTexture.in
			if(screenShotEvent != null){
				callback (screenShotEvent());
		}
		});
	}
	private void callback(Texture2D tex){
		StartCoroutine(UploadPNG(tex));
	}
	private IEnumerator UploadPNG(Texture2D tex)
    {
		byte[] bytes = tex.EncodeToPNG();
        // We should only read the screen buffer after rendering is complete
        yield return null;

        // Object.Destroy(tex);

        // For testing purposes, also write to a file in the project folder
        File.WriteAllBytes(Application.dataPath + "/../SavedScreen.png", bytes);
		Debug.Log(Application.dataPath + "/../SavedScreen.png");

    }
	// onGUI - multiple times per frame
	// private void OnGUI(){
	// 	// if(_takeScreenshot){

	// 	// 	string screenshotFilename = "screenshot.png";
	// 	// 	string screenShotPath = Application.persistentDataPath + "/" + screenshotFilename;
   
    //    	// 	if (File.Exists(screenShotPath)) File.Delete(screenShotPath);
	// 	// 	ScreenCapture.CaptureScreenshot(screenshotFilename);
	// 	// 	var debugPath = Application.dataPath;
    //     //     int index = debugPath.LastIndexOf("/");
    //     //     debugPath = debugPath.Substring(0, index);
    //     //     screenShotPath = debugPath + "/screenshot.png";
	// 	// 	if (File.Exists(screenShotPath))
    //     //     		Debug.Log(debugPath);

	// 	// 	_takeScreenshot = false;
	// 	// 	_bottom.alpha = 1;
	// 	// 	_me.alpha = 1;
	// 	// 	_reference.Screencapture = false;
	// 	// }
	// }
}

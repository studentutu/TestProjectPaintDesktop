using UnityEngine;
using UnityEngine.UI;
using System.Linq;
using System;

public class REDscript : MonoBehaviour {
	private InputField myInput;
	public static event System.Action<int> redEvent;

	void Start () {
		myInput = this.GetComponent<InputField>();
		myInput.onValueChanged.AddListener( delegate {onValueChanged();});
		myInput.text = "255";
	}
	private void onValueChanged(){
		int x;
		if(myInput.text.All(char.IsDigit)){
			if( Int32.TryParse(myInput.text, out x)){
					OnRedEvent(x);
			}
			
		}
		
	}
	
	protected virtual void OnRedEvent(int variable){
		// notify all
		if(redEvent != null){
			redEvent(variable);
		}
	}
}

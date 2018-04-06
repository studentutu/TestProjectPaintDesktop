using UnityEngine;
using UnityEngine.UI;
using System.Linq;
using System;
public class BlueScript : MonoBehaviour {

	private InputField myInput;
	// public delegate void MyDel(int variable);
	public static event System.Action<int> BlueEvent;

	void Start () {
		myInput = this.GetComponent<InputField>();
		myInput.onValueChanged.AddListener( delegate {onValueChanged();});
		myInput.text = "255";
	}
	private void onValueChanged(){
		int x;
		if(myInput.text.All(char.IsDigit)){
			if( Int32.TryParse(myInput.text, out x)){
					OnEvent(x);
			}
			
		}
		
	}
	
	protected virtual void OnEvent(int variable){
		// notify all
		if(BlueEvent != null){
			BlueEvent(variable);
		}
	}
}

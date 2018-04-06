using UnityEngine;
using UnityEngine.UI;
using System.Linq;
using System;
public class GreenScript : MonoBehaviour {

	private InputField myInput;
	// public delegate void MyDel(int variable);
	public static event System.Action<int> GreeEvent;

	void Start () {
		myInput = this.GetComponent<InputField>();
		myInput.onValueChanged.AddListener( delegate {onValueChanged();});
		myInput.text = "255";
	}
	private void onValueChanged(){
		int x;
		if(myInput.text.All(char.IsDigit)){
			if( Int32.TryParse(myInput.text, out x)){
					OnGreenEvent(x);
			}
			
		}
		
	}
	
	protected virtual void OnGreenEvent(int variable){
		// notify all
		if(GreeEvent != null){
			GreeEvent(variable);
		}
	}
}

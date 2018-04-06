using UnityEngine;
using UnityEngine.UI;
using System.Linq;
using System;

public class REDscript : MonoBehaviour {
	private InputField myInput;
	public delegate void MyDel(int variable);
	public event MyDel redEvent;

	void Start () {
		myInput = this.GetComponent<InputField>();
		myInput.onValueChanged.AddListener( delegate {OrdinaryFunc();});
	}
	private void OrdinaryFunc(){
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

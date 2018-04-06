using UnityEngine;
using UnityEngine.UI;
using System.Linq;
using System;
public class BlueScript : MonoBehaviour {

	private InputField myInput;
	public delegate void MyDel(int variable);
	public event MyDel BlueEvent;

	void Start () {
		myInput = this.GetComponent<InputField>();
		myInput.onValueChanged.AddListener( delegate {OrdinaryFunc();});
	}
	private void OrdinaryFunc(){
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

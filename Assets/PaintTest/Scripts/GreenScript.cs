using UnityEngine;
using UnityEngine.UI;
using System.Linq;
using System;
public class GreenScript : MonoBehaviour {

	private InputField myInput;
	public delegate void MyDel(int variable);
	public event MyDel GreeEvent;

	void Start () {
		myInput = this.GetComponent<InputField>();
		myInput.onValueChanged.AddListener( delegate {OrdinaryFunc();});
	}
	private void OrdinaryFunc(){
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

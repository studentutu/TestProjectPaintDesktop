using System.Collections;
using System;
using UnityEngine;
using UnityEngine.UI;
public class ColorPallete : MonoBehaviour {
	#region Data
		// private Color _temp;
	#endregion

	public delegate void FiredHandler(Color color );
	public event FiredHandler Fired;

	private void Start(){
		this.GetComponent<Button>().onClick.AddListener(InternalFire);
	}
	private void InternalFire(){
		// Debug.Log(" In Publisher");
		StartCoroutine(wait());
		var _temp = new Color(1,1, 0);
		// ending
		OnFired(_temp );
	}
	private IEnumerator wait(){
		yield return new WaitForSeconds(3f);
	}
	protected virtual void OnFired(Color _temp){
		if(Fired != null){
			Fired(_temp);
		}
	}
}

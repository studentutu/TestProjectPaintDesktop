using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PaintGM : MonoBehaviour {

	
	// Frames per second
	private void Update () {
		
		if(Input.touchCount > 0 && Input.GetTouch(0).phase == TouchPhase.Moved
			|| Input.GetMouseButton(0) ){
			
			Plane planeOfObject = new Plane(Camera.main.transform.forward *-1, this.transform.position);
			Ray myRay = Camera.main.ScreenPointToRay(Input.mousePosition);
			float rayDistance;
			if(planeOfObject.Raycast(myRay, out rayDistance)){
				this.transform.position = myRay.GetPoint(rayDistance);
			}
		}

	}
	
}

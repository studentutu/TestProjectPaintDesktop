using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;
public class SceneController : MonoBehaviour {

	#region SerializeField
	[SerializeField] private Color _colorPicker;
	[SerializeField] private Button _colorPickerBTN;
	[Space]
	[SerializeField] private Button _paintMode;
	[SerializeField] private Transform _prefab;
	[SerializeField] private GameObject _parent;
	#region UI raycast
	private GraphicRaycaster m_Raycaster;
    private PointerEventData m_PointerEventData;
    private EventSystem m_EventSystem;
	#endregion
	[Space]
	[SerializeField] private Button _eraser;
	[SerializeField] private TrailRenderer _paintedPoint;
	[Space]
	[SerializeField] private Slider _brushSizeSlider;
	[Space]
	#endregion
	#region  Cursor
	private Vector2 mouse;
    private int w = 16;
    private int h = 16;
    [SerializeField] private Texture2D cursor;
	#endregion
	private bool _eraserMode = false;
	public bool Screencapture = false;
	private void Start(){

		_eraserMode = false;
		_eraser.onClick.AddListener( () => { changeMode(true); } );
		_paintMode.onClick.AddListener( () => { changeMode(false); } );

		//Fetch the Raycaster from the GameObject (the Canvas)
        m_Raycaster = FindObjectOfType<GraphicRaycaster>();
        //Fetch the Event System from the Scene
        m_EventSystem = FindObjectOfType<EventSystem>();

		_brushSizeSlider.onValueChanged.AddListener( delegate {changedValueOnSlider();});
		REDscript.redEvent += onRedChange;
		GreenScript.GreeEvent += onGreenChan;
		BlueScript.BlueEvent += onBlueChan;
	}
	private void changedValueOnSlider(){
		w = (int)_brushSizeSlider.value;
		h = w;
	}
	private void onRedChange(int collor){
		Image temp = _colorPickerBTN.transform.GetChild(0).GetComponent<Image>();
		temp.color = new Color( ((float)collor/255),temp.color.g, temp.color.b);
		_colorPicker = temp.color;
		
	}
	private void onGreenChan(int collor){
		Image temp = _colorPickerBTN.transform.GetChild(0).GetComponent<Image>();
		temp.color = new Color( temp.color.r,((float)collor/255), temp.color.b);
		_colorPicker = temp.color;
	}
	private void onBlueChan(int collor){
		Image temp = _colorPickerBTN.transform.GetChild(0).GetComponent<Image>();
		temp.color = new Color( temp.color.r, temp.color.g, ((float)collor/255));
		_colorPicker = temp.color;
	}

	private void changeMode(bool b){
		_eraserMode = b;
	}
	private void OnGUI(){
		if(!Screencapture){
			GUI.DrawTexture(new Rect(mouse.x - (w / 2), mouse.y - (h / 2), w, h), cursor);
		}
        
    }
	private void Update(){
		Vector2 currentMouseOrTouchPos;
		mouse = new Vector2(Input.mousePosition.x, Screen.height - Input.mousePosition.y);
		#if UNITY_EDITOR
			currentMouseOrTouchPos = Camera.main.ScreenToWorldPoint(Input.mousePosition);
		#else
			// will work on mobile, but canvas size is different!
			if( Input.touchCount > 0 && Input.GetTouch(0).phase == TouchPhase.Moved){
				currentMouseOrTouchPos = Input.GetTouch(0).position;
			}	
		#endif


		if( Input.GetKey(KeyCode.Mouse0) || (Input.touchCount > 0 && Input.GetTouch(0).phase == TouchPhase.Moved) ){
			
			if(_eraserMode){
				//Set up the new Pointer Event
				m_PointerEventData = new PointerEventData(m_EventSystem);
				//Set the Pointer Event Position to that of the mouse position
				m_PointerEventData.position = Input.mousePosition;
				//Create a list of Raycast Results
           		List<RaycastResult> results = new List<RaycastResult>();
				//Raycast using the Graphics Raycaster and mouse click position
            	m_Raycaster.Raycast(m_PointerEventData, results);   
				//For every result returned, output the name of the GameObject on the Canvas hit by the Ray
				foreach (RaycastResult result in results)
				{
					
					var y = result.gameObject.GetComponent<CustomDespose>();
					if (y !=null){
						Destroy( result.gameObject);
					}
				}
				
			}else{
				 
				RectTransform gameObj = Instantiate(_prefab, _prefab.position, _prefab.rotation) as RectTransform;
				gameObj.transform.SetParent (_parent.transform, true);
				gameObj.position = currentMouseOrTouchPos;
				int temp = w/10;
				gameObj.localScale = Vector3.one*temp;
				gameObj.GetComponent<RawImage>().color = _colorPicker;
			}
				
		}
	}

}

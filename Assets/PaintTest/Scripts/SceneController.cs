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
	private void Start(){

		_eraserMode = false;
		_eraser.onClick.AddListener( () => { ChangeMode(true); } );
		_paintMode.onClick.AddListener( () => { ChangeMode(false); } );

		//Fetch the Raycaster from the GameObject (the Canvas)
        m_Raycaster = FindObjectOfType<GraphicRaycaster>();
        //Fetch the Event System from the Scene
        m_EventSystem = FindObjectOfType<EventSystem>();

		_brushSizeSlider.onValueChanged.AddListener( delegate {changedValueOnSlider();});
		FindObjectOfType<REDscript>().redEvent += OnRedChange;
		FindObjectOfType<GreenScript>().GreeEvent += OnGreenChan;
		FindObjectOfType<BlueScript>().BlueEvent += OnBlueChan;
	}
	private void changedValueOnSlider(){
		w = (int)_brushSizeSlider.value;
		h = w;
	}
	private void OnRedChange(int collor){
		Image temp = _colorPickerBTN.transform.GetChild(0).GetComponent<Image>();
		temp.color = new Color( 1-((float)collor/255),temp.color.g, temp.color.b);
		_colorPicker = temp.color;
		
	}
	private void OnGreenChan(int collor){
		Image temp = _colorPickerBTN.transform.GetChild(0).GetComponent<Image>();
		temp.color = new Color( temp.color.r,1-((float)collor/255), temp.color.b);
		_colorPicker = temp.color;
	}
	private void OnBlueChan(int collor){
		Image temp = _colorPickerBTN.transform.GetChild(0).GetComponent<Image>();
		temp.color = new Color( temp.color.r, temp.color.g, 1-((float)collor/255));
		_colorPicker = temp.color;
	}

	private void ChangeMode(bool b){
		_eraserMode = b;
	}
	private void OnGUI(){
         GUI.DrawTexture(new Rect(mouse.x - (w / 2), mouse.y - (h / 2), w, h), cursor);
    }
	private void Update(){
		// Cursor
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
					// Debug.Log("Hit " + result.gameObject.name);
					var y = result.gameObject.GetComponent<CustomDespose>();
					if (y !=null){
						Destroy( result.gameObject);
					}
				}
				
			}else{
				 
				RectTransform gameObj = Instantiate(_prefab, _prefab.position, _prefab.rotation) as RectTransform;
				gameObj.transform.SetParent (_parent.transform, true);
				// gameObj.transform.position = new Vector3(currentMouseOrTouchPos.x, currentMouseOrTouchPos.y, 0);
				gameObj.position = currentMouseOrTouchPos;
				// Initial 10*10 -1
				// w 16 -x
				int temp = w/10;
				gameObj.localScale = Vector3.one*temp;
				gameObj.GetComponent<RawImage>().color = _colorPicker;
			}
				
		}
	}

}

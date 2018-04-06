using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;
using System.Collections.Generic;
public class PaintingScript : MonoBehaviour {
	#region UI raycast
	private GraphicRaycaster m_Raycaster;
    private PointerEventData m_PointerEventData;
    private EventSystem m_EventSystem;
	#endregion

	#region Cursor
	private int widthOfCursor = 16;
    private int heightOfCursor = 16;
    [SerializeField] private Texture2D _cursor;
	#endregion

	private Vector2 mouse;
	private bool _eraserMode = false;
	private Texture2D _changeTexture;
	private Sprite _thisSprite;

	#region  SerializeFields
	[SerializeField] private SpriteRenderer _thisRenderer;
	[SerializeField] private Button _colorPickerBTN;
	[SerializeField] private Color _colorPicker;
	[SerializeField] private Button _eraserBTN;
	[SerializeField] private Button _paintModeBTN;
	[SerializeField] private Slider _brushSizeSlider;
	#endregion 
	// public bool Screencapture;
	// Use this for initialization
	void Start () {
		_eraserBTN.onClick.AddListener( delegate { changeMode(true); } );
		_paintModeBTN.onClick.AddListener( delegate  { changeMode(false); } );
		//Fetch the Raycaster from the GameObject (the Canvas)
		m_Raycaster = GameObject.FindGameObjectWithTag("CANVAS").GetComponent<GraphicRaycaster>();
        //Fetch the Event System from the Scene
        m_EventSystem = GameObject.FindGameObjectWithTag("EVENT").GetComponent<EventSystem>();
		REDscript.redEvent += onRedChange;
		GreenScript.GreeEvent += onGreenChan;
		BlueScript.BlueEvent += onBlueChan;
		_brushSizeSlider.onValueChanged.AddListener( delegate {changedValueOnSlider();});
		CreateTexture();
		CreateSprite();
		Screenshot.screenShotEvent += OngetTexture;
	}
	private Texture2D OngetTexture(){
		return _changeTexture;
	}
	private void CreateTexture(){
		_changeTexture = new Texture2D(Screen.width,Screen.height);
        _thisRenderer.material.mainTexture = _changeTexture;
		

        for (int y = 0; y < _changeTexture.height; y++)
        {
            for (int x = 0; x < _changeTexture.width; x++)
            {
                Color tempcolor = Color.gray;
                _changeTexture.SetPixel(x, y, tempcolor);
            }
        }
        _changeTexture.Apply();
	}
	private void CreateSprite(){
		_thisSprite = Sprite.Create(_changeTexture, new Rect(0,0,Screen.width,Screen.height), Vector2.one*0.5f);
		_thisRenderer.sprite = _thisSprite;
		_thisRenderer.gameObject.GetComponent<Image>().sprite = _thisSprite;
		_thisRenderer.gameObject.GetComponent<Image>().material.mainTexture = _changeTexture;
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

	private void changedValueOnSlider(){
		widthOfCursor = (int)_brushSizeSlider.value;
		heightOfCursor = widthOfCursor;
	}
	private void changeMode(bool b){
		_eraserMode = b;
	}
	
	private void OnGUI(){
		// if(!Screencapture){
			GUI.DrawTexture(new Rect(mouse.x - (widthOfCursor / 2), mouse.y - (heightOfCursor / 2), widthOfCursor, heightOfCursor), _cursor);
		// }
        
    }
	private void Update(){
		// Vector2 currentMouseOrTouchPos;
		mouse = new Vector2(Input.mousePosition.x, Screen.height - Input.mousePosition.y);
		#if UNITY_EDITOR
			// currentMouseOrTouchPos = Camera.main.ScreenToWorldPoint(Input.mousePosition);
		#else
			// will work on mobile, but canvas size is different!
			// if( Input.touchCount > 0 && Input.GetTouch(0).phase == TouchPhase.Moved){
			// 	currentMouseOrTouchPos = Input.GetTouch(0).position;
			// }	
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
					var y = result;
					// Debug.Log(y.gameObject.layer);
					if (y.gameObject.layer == 8){
						Paint(Color.gray);
					}
				}
				
			}else{
				Paint(_colorPicker);
				//  gameObj.localScale = Vector3.one*temp;
				// RectTransform gameObj = Instantiate(_prefab, _prefab.position, _prefab.rotation) as RectTransform;
				// gameObj.transform.SetParent (_parent.transform, true);
				// gameObj.position = currentMouseOrTouchPos;
				// int temp = w/10;
				// gameObj.localScale = Vector3.one*temp;
				// gameObj.GetComponent<RawImage>().color = _colorPicker;
			}
				
		}
	}
	private void Paint(Color nextColor){
		int scale = widthOfCursor/2;
		int rounded = 0;
		int step = rounded;
		int diffMinus =0;
		int diffPlus =0;
		bool flip =false;
		// inversed y!
		// _changeTexture.SetPixel((int)(mouse.x), Screen.height - (int)(mouse.y), _colorPicker);
		for(int y = Screen.height - (int)(mouse.y) - (scale);  y < Screen.height - (int)(mouse.y) + (scale) + 1; y++){
			step = 0;
			diffMinus = scale-rounded;
			diffPlus = scale+rounded;
			for(int i = (int)(mouse.x) - (scale); i< (int)(mouse.x) + (scale) +1; i++){
				if(step >= diffMinus){
					if(step <= diffPlus){
						_changeTexture.SetPixel(i, y, nextColor);
					}
				}
				step+=1;
			}
			if(flip == false){
				if(rounded >= scale){
					flip = true;
					rounded-=1;
				}else{
					rounded+=1;
				}
			}else{
				rounded-=1;
			}
		}
		_changeTexture.Apply();
	}
}

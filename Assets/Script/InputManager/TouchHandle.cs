using UnityEngine;
using System.Collections;

public delegate void del_no_param();

public class TouchHandle : MonoBehaviour {

	public static TouchHandle _instance;

	private event del_no_param _turnLeftEvt;
	private event del_no_param _turnRightEvt;

	private float _oldPosX;
	private bool isTurning = false;
	private Touch[] touches;

	internal void AddEvent (del_no_param turnLeft, del_no_param turnRight) 
	{
		_turnLeftEvt = turnLeft;
		_turnRightEvt = turnRight;
	}

	void Awake() {
		_instance = this;
	}

	// Use this for initialization
	void Start () {
	
	}
	
	// Update is called once per frame
	void Update () {
		#if UNITY_ANDROID || UNITY_IPHONE || UNITY_WP8
		#region unity_touches
		//ko can xet den th nhieu tay :v
		if (Input.touchCount >0 && Input.touchCount <= 1)
		{
			touches = Input.touches;
			for (int i = 0; i < 1; i++)
			{
				switch (touches[i].phase)
				{
				case TouchPhase.Began:
					Ray ray = Camera.main.ScreenPointToRay(touches[i].position);
					if (Physics.Raycast(ray, 100)) 
					{
						isTurning = true;
						_oldPosX = touches[i].position.x;
					}
					break;
				case TouchPhase.Moved:
					if (_oldPosX == touches[i].position.x)
						return;
					if (isTurning) 
					{
						if (_turnLeftEvt != null && _turnRightEvt != null) 
						{
							float sub = touches[i].position.x - _oldPosX;
							if (sub > 0)
								_turnRightEvt();
							else
								_turnLeftEvt();
							_oldPosX = touches[i].position.x;
						}
					}
					break;
				case TouchPhase.Ended:
				case TouchPhase.Canceled:
					isTurning = false;
					break;
				}
				
			}
		}
		#endregion
		#endif

		if(SystemInfo.deviceType == DeviceType.Desktop)
		{
			//we are on a desktop device, so don't use touch
			if (Input.GetMouseButtonDown(0))
			{
				Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);
				if (Physics.Raycast(ray, 10, 1 << LayerMask.NameToLayer("Player"))) 
				{
					isTurning = true;
					_oldPosX = Input.mousePosition.x;
				}
			}
			else if (Input.GetMouseButtonUp(0))
			{
				isTurning = false;
				
			}
			else if (Input.GetMouseButton(0))
			{
				if (_oldPosX == Input.mousePosition.x)
					return;
				if (isTurning) 
				{
					if (_turnLeftEvt != null && _turnRightEvt != null) 
					{
						float sub = Input.mousePosition.x - _oldPosX;
						if (sub > 0)
							_turnRightEvt();
						else
							_turnLeftEvt();
						_oldPosX = Input.mousePosition.x;
					}
				}
			}
		}

	}
}

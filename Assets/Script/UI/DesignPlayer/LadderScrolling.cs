using UnityEngine;
using System.Collections;

public class LadderScrolling : MonoBehaviour {

	public UnityEngine.UI.Extensions.HorizontalScrollSnap _scrollSnap;
	public float speed = 200;

	private bool _isScrolling = false;
	private int _curScreen;
	private int _nextScreen;

	private Vector3 _outPos = new Vector3(-290, -68, 0);
	private Vector3 _inPos = new Vector3(60, -68, 0);

	private Transform _curTab;
	private Transform _nextTab;

	// Use this for initialization
	void Start () {
		_curScreen = 3;
		_curTab = _nextTab = transform.GetChild (_curScreen);
	}
	
	// Update is called once per frame
	void Update () {
		if (!_isScrolling && _curScreen != _scrollSnap.CurrentPage)
		{
			_isScrolling = true;
			_nextScreen = _scrollSnap.CurrentPage;
			_nextTab = transform.GetChild(_nextScreen);
		}
		if (_isScrolling)
			Scroll ();
	}

	private void Scroll()
	{
		if (_curTab.localPosition.x != -290 || _nextTab.localPosition.x != 60) 
		{
			_curTab.localPosition = Vector3.MoveTowards (_curTab.localPosition, _outPos, speed * Time.deltaTime);
			_nextTab.localPosition = Vector3.MoveTowards (_nextTab.localPosition, _inPos, speed * Time.deltaTime);
		} 
		else 
		{
			_isScrolling = false;
			_curTab = _nextTab;
			_curScreen = _nextScreen;
		}
	}

}

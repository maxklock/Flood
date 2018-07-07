using System.Collections;
using System.Collections.Generic;

using Flood.MixedReality;

using UnityEngine;
using UnityEngine.Events;

public class Button3D : MonoBehaviour
{

    private Vector3 _initialPosition;
    public float ButtonDeep = 0.01f;

    public bool On { get; private set; }

    public UnityEvent OnClick;


    private bool _pressedDown = false;

	// Use this for initialization
	void Start () {
		_initialPosition = new Vector3(transform.position.x, transform.position.y, transform.position.z);
	}
	
	// Update is called once per frame
	void Update () {
	    if (Vector3.Distance(ControllerHandler.Instance.RightController.transform.position, transform.position) < 0.2)
	    {
	        if (!_pressedDown)
	        {
	            _pressedDown = true;
	            this.transform.position -= transform.up * ButtonDeep;
	            On = !On;
	            OnClick.Invoke();

            }

	    }
	    else
	    {
	        this.transform.position = _initialPosition;
	        _pressedDown = false;
            
	    }

	}
}

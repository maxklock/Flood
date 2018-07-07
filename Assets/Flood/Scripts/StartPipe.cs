using System.Collections;
using System.Collections.Generic;

using Flood;

using UnityEngine;

[RequireComponent(typeof(PlaceableObject))]
public class StartPipe : Pipe {

	// Use this for initialization
	void Start ()
    {
		GridManager.Instance.SetCell(GetComponent<PlaceableObject>(), transform.position, GridPositionState.REAL_WORLD);
	}
	
	// Update is called once per frame
	void Update () {
		
	}
}

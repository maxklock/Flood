using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(GridManager))]
public class GridDrawing : MonoBehaviour
{

    private GridManager _gridManager;
    public GameObject LineRendererPrefab;

	// Use this for initialization
	void Start ()
	{
	    _gridManager = GetComponent<GridManager>();
        CreateLineRenderer();

	}
	
	// Update is called once per frame
	void Update () {
		
	}

    private void CreateLineRenderer()
    {
        //Build XY Layers
        for (int z = 0; z <= _gridManager.GridDimensions.z; z++)
        {
            //Build X Layer
            for (int x = 0; x <= _gridManager.GridDimensions.x; x++)
            {

                var lR = GameObject.Instantiate(LineRendererPrefab);
                lR.transform.parent = this.transform;
                var lRS = lR.GetComponent<LineRenderer>();
                lRS.positionCount = 2;
                lRS.SetPosition(0, new Vector3(x * _gridManager.GridScale, 0, z * _gridManager.GridScale));
                lRS.SetPosition(1, new Vector3(x * _gridManager.GridScale, _gridManager.GridDimensions.y * _gridManager.GridScale, z * _gridManager.GridScale));

            }
            for (int y = 0; y <= _gridManager.GridDimensions.y; y++)
            {
                var lR = GameObject.Instantiate(LineRendererPrefab);
                lR.transform.parent = this.transform;
                var lRS = lR.GetComponent<LineRenderer>();
                lRS.positionCount = 2;
                lRS.SetPosition(0, new Vector3(0, y * _gridManager.GridScale, z * _gridManager.GridScale));
                lRS.SetPosition(1, new Vector3(_gridManager.GridDimensions.x * _gridManager.GridScale, y * _gridManager.GridScale, z * _gridManager.GridScale));

            }
        }


        for (int x = 0; x <= _gridManager.GridDimensions.x; x++)
        {
            for (int y = 0; y <= _gridManager.GridDimensions.y; y++)
            {
                var lR = GameObject.Instantiate(LineRendererPrefab);
                lR.transform.parent = this.transform;
                var lRS = lR.GetComponent<LineRenderer>();
                lRS.positionCount = 2;
                lRS.SetPosition(0, new Vector3(x * _gridManager.GridScale, y * _gridManager.GridScale, 0));
                lRS.SetPosition(1, new Vector3(x * _gridManager.GridScale, y * _gridManager.GridScale, _gridManager.GridDimensions.z * _gridManager.GridScale));
            }

        }
    }
}

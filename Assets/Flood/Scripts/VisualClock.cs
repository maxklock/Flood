using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class VisualClock : MonoBehaviour {


    public float RemainingTime { get; private set; }
    public float MaxTime;

    public GameObject ScaleGemObject;
    private float _maxScaleGemObject;
    private Vector3 _referencePosition;



    // Use this for initialization
    void Start()
    {
        _maxScaleGemObject = ScaleGemObject.transform.localScale.y;
        _referencePosition = new Vector3(ScaleGemObject.transform.localPosition.x, ScaleGemObject.transform.localPosition.y, ScaleGemObject.transform.localPosition.z);

        RemainingTime = MaxTime;
    }


    // Update is called once per frame
    void Update()
    {
        RemainingTime -= Time.deltaTime;

        ScaleGemObject.transform.localScale = new Vector3(transform.localScale.x, (RemainingTime / MaxTime) * _maxScaleGemObject, transform.localScale.z);
        ScaleGemObject.transform.localPosition = new Vector3(ScaleGemObject.transform.localPosition.x, _referencePosition.y - (_maxScaleGemObject - (((RemainingTime - 1) / MaxTime))), ScaleGemObject.transform.localPosition.z);


        if (RemainingTime == 0)
            Debug.Log("Time is up!");
    }
}

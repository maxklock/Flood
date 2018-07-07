using System.Collections;
using System.Collections.Generic;

using Flood;

using UnityEngine;

public class AssemblyLineSpawner : MonoBehaviour
{

    public Vector3 LocalDir;

    public float MaxSpawnDuration;
    private float _currentSpawnDuration;

    public float Speed = 0.05f;

    public PlaceableObject[] SpawnableObjectPrefab;

	// Use this for initialization
	void Start ()
	{
	    _currentSpawnDuration = MaxSpawnDuration;
	}
	
	// Update is called once per frame
	void Update ()
	{
	    _currentSpawnDuration += Time.deltaTime;
	    if (_currentSpawnDuration > MaxSpawnDuration)
	    {
	        _currentSpawnDuration = 0;
	        SpawnObject();


	    }

	}

    private void SpawnObject()
    {
        var obj = GameObject.Instantiate(SpawnableObjectPrefab[Random.Range(0, SpawnableObjectPrefab.Length)]);
        obj.GetComponent<BoxCollider>().enabled = false;
        obj.transform.position = this.transform.position;

    }

    public Vector3 GetMovement()
    {
        return transform.forward* Speed * Time.deltaTime;
    }
}

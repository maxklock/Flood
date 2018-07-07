using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PipeOnAssemblyLine : MonoBehaviour {

    void OnCollisionStay(Collision collision)
    {
        if (collision.transform.tag == "AssemblyLine")
        {
            //Debug.Log(collision.other.transform.GetComponentInChildren<AssemblyLineSpawner>().GetMovement());
            transform.position += collision.transform.GetComponentInChildren<AssemblyLineSpawner>().GetMovement();
        }
    }
}

using System.Collections;
using System.Collections.Generic;

using Flood;

using UnityEngine;

public class AssemblyKillPlane : MonoBehaviour {

    void OnCollisionEnter(Collision collision)
    {
        if (collision.gameObject.GetComponent<PlaceableObject>()!=null)
        {
            GameObject.Destroy(collision.gameObject);
        }
    }
}

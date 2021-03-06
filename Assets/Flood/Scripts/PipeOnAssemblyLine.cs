﻿using UnityEngine;

namespace Flood
{
    using Flood.Assembly;

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
}

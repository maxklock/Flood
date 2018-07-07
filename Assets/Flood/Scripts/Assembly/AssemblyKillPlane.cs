using UnityEngine;

namespace Flood.Assembly
{
    public class AssemblyKillPlane : MonoBehaviour {

        void OnCollisionEnter(Collision collision)
        {
            if (collision.gameObject.GetComponent<PlaceableObject>()!=null)
            {
                GameObject.Destroy(collision.gameObject);
            }
        }
    }
}

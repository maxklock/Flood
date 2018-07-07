using UnityEngine;

namespace Flood
{
    public class Obstical : MonoBehaviour
    {

        // Use this for initialization
        private void Start()
        {
            var colliders = GetComponentsInChildren<Collider>();

            GridManager.Instance.SetCellByCollider(gameObject, colliders);
        }
    }
}
using UnityEngine;

namespace Flood
{
    [RequireComponent(typeof(PlaceableObject))]
    public class EndPipe : Pipe
    {

        // Use this for initialization
        void Start()
        {
            GridManager.Instance.SetCell(gameObject, transform.position, GridPositionState.REAL_WORLD);
        }

        // Update is called once per frame
        void Update()
        {

        }
    }
}
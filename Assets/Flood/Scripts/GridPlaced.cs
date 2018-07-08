using UnityEngine;

namespace Flood
{
    public class GridPlaced : MonoBehaviour
    {

        // Use this for initialization
        private void Start()
        {
            GridManager.Instance.SetCell(gameObject, transform.position, GridPositionState.REAL_WORLD);
        }
    }
}
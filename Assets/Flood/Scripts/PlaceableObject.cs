namespace Flood
{
    using UnityEngine;
    public class PlaceableObject : MonoBehaviour
    {
        public bool OneTimeDrop = true;

        public bool CanBeTaken = true;

        private Color _startColor;

        private void Start()
        {
            _startColor = GetComponentInChildren<Renderer>().material.color;
        }

        public bool Take()
        {
            if (!CanBeTaken)
            {
                return false;
            }

            GetComponentInChildren<Rigidbody>().isKinematic = true;
            GetComponent<PipeOnAssemblyLine>().enabled = false;
            GetComponentInChildren<Renderer>().material.color = Color.green;


            return true;
        }

        public void Drop(bool placeable)
        {
            if (!OneTimeDrop)
            {
                GetComponentInChildren<Renderer>().material.color = _startColor;
                return;
            }

            GetComponentInChildren<Renderer>().material.color = Color.red;
            if (placeable)
            {
                CanBeTaken = false;
            }
            else
            {
                GetComponentInChildren<Renderer>().material.color = Color.blue;
                Destroy(gameObject);
            }
        }

    }
}

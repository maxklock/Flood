namespace Flood
{
    using UnityEngine;
    public class PlaceableObject : MonoBehaviour
    {
        public bool OneTimeDrop = true;

        private Color _startColor;

        private void Start()
        {
            _startColor = GetComponentInChildren<Renderer>().material.color;
        }

        public void Take()
        {
            GetComponentInChildren<Renderer>().material.color = Color.green;
        }

        public void Drop()
        {
            if (!OneTimeDrop)
            {
                GetComponentInChildren<Renderer>().material.color = _startColor;
                return;
            }

            GetComponentInChildren<Renderer>().material.color = Color.red;
            Destroy(this);
        }

    }
}

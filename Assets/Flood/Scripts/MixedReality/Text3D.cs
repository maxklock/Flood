using UnityEngine;

namespace Flood.MixedReality
{
    public class Text3D : MonoBehaviour
    {
        public float Distance = 1f;

        public TextMesh TextMesh;

        // Use this for initialization
        private void Start()
        {

        }

        public void SetText(string text)
        {
            TextMesh.text = text;
        }

        // Update is called once per frame
        private void Update()
        {
            var pos = Camera.main.transform.position;
            pos.y = 0;
            var direction = Camera.main.transform.forward;
            direction.y = 0;
            direction.Normalize();

            transform.position = pos + direction * Distance;
            transform.forward = direction;
        }
    }
}
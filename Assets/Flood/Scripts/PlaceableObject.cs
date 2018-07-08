namespace Flood
{
    using UnityEngine;
    public class PlaceableObject : MonoBehaviour
    {
        public bool OneTimeDrop = true;

        public bool CanBeTaken = true;

        public bool Take()
        {
            if (!CanBeTaken)
            {
                return false;
            }

            GetComponentInChildren<Rigidbody>().isKinematic = true;
            GetComponent<PipeOnAssemblyLine>().enabled = false;


            return true;
        }

        private void Update()
        {
            if (CanBeTaken)
            {
                return;
            }

            if (transform.position.y < 0.15f)
            {
                Destroy(gameObject);
            }
        }

        public void Drop(bool placeable, Material material)
        {
            if (!OneTimeDrop)
            {
                return;
            }

            CanBeTaken = false;

            if (placeable)
            {
                LevelEvaluation.Instance.Connected++;
                LevelEvaluation.Instance.Evaluate();
                GetComponentInChildren<Renderer>().material = material;
            }
            else
            {
                LevelEvaluation.Instance.Dropped++;
                GetComponentInChildren<Rigidbody>().isKinematic = false;
            }
        }

    }
}

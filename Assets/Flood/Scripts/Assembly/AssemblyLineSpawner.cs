using UnityEngine;

namespace Flood.Assembly
{
    public class AssemblyLineSpawner : MonoBehaviour
    {

        public Vector3 LocalDir;

        public float MaxSpawnDuration;
        private float _currentSpawnDuration;

        public float Speed = 0.05f;

        public PlaceableObject[] SpawnableObjectPrefab;

        public bool Running = true;

        // Use this for initialization
        void Start ()
        {
            _currentSpawnDuration = MaxSpawnDuration;
        }

        public void StopSpawing()
        {
            Running = false;
        }

        public void StartSpawning()
        {
            Running = true;
        }
	
        // Update is called once per frame
        void Update ()
        {
            if(!Running)
                return;

            _currentSpawnDuration += Time.deltaTime;
            if (_currentSpawnDuration > MaxSpawnDuration)
            {
                _currentSpawnDuration = 0;
                SpawnObject();


            }

        }

        private void SpawnObject()
        {
            var obj = GameObject.Instantiate(SpawnableObjectPrefab[Random.Range(0, SpawnableObjectPrefab.Length)]);
            obj.GetComponent<BoxCollider>().enabled = false;
            obj.transform.position = this.transform.position;

        }

        public Vector3 GetMovement()
        {
            return transform.forward* Speed * Time.deltaTime;
        }
    }
}

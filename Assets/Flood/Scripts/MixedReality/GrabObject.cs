namespace Flood.MixedReality
{
    using System.Linq;

    using Flood.Utils;

    using UnityEngine;

    public class GrabObject : MonoBehaviour
    {

        public float GrabDistance = 0.3f;
        public float GrabOffset = 0.2f;
        public float CarryDistance = 0.4f;
        public Material GhostMaterial;
        public Material PlacedMaterial;

        public bool ShowGhost;

        private PlaceableObject _grabbedObject;
        private GameObject _ghost;
        private float _grabbedDistance;
        private Vector3 _localRotation;

        // Use this for initialization
        private void Start()
        {

        }

        // Update is called once per frame
        private void Update()
        {
            var right = ControllerHandler.Instance.RightController;

            if (AxisToButtonUtil.Instance.IsPressed("CONTROLLER_RIGHT_TRIGGER"))
            {
                var ray = new Ray(right.transform.position - right.transform.forward * GrabOffset, right.transform.forward);
                if (_grabbedDistance < 0)
                {
                    return;
                }
                if (_grabbedObject == null)
                {
                    RaycastHit hit;
                    if (Physics.Raycast(ray, out hit, GrabDistance + GrabOffset))
                    {
                        _grabbedObject = hit.transform.gameObject.GetComponent<PlaceableObject>();
                        if (_grabbedObject == null || !_grabbedObject.CanBeTaken)
                        {
                            _grabbedObject = null;
                            return;
                        }
                        _grabbedObject.Take();
                        _grabbedDistance = _grabbedObject.transform.localScale.x / 2 + GrabOffset + CarryDistance; // hit.distance;
                        _localRotation = Vector3.zero;

                        if (ShowGhost)
                        {
                            _ghost = new GameObject("ghost");
                            _ghost.transform.localScale = _grabbedObject.transform.localScale;
                            Instantiate(_grabbedObject.GetComponentInChildren<MeshCollider>().gameObject, _ghost.transform);
                            _ghost.GetComponentInChildren<Renderer>().material = GhostMaterial;
                        }
                    }
                    else
                    {
                        _grabbedDistance = -1;
                    }
                    return;
                }

                _localRotation += 10 * new Vector3(0 /*Input.GetAxis("CONTROLLER_RIGHT_STICK_VERTICAL")*/, Input.GetAxis("CONTROLLER_RIGHT_STICK_HORIZONTAL"), 0);

                _grabbedObject.transform.position = ray.origin + ray.direction * (_grabbedDistance);
                _grabbedObject.transform.rotation = right.transform.rotation * Quaternion.Euler(_localRotation);

                if (_ghost == null)
                    return;
                _ghost.transform.position = _grabbedObject.transform.position;
                _ghost.transform.rotation = _grabbedObject.transform.rotation;
                GridManager.Instance.ApplyGridTransform(_ghost, GridManager.Instance.WorldToGrid(_ghost.transform.position));
                return;
            }

            if (_grabbedObject != null)
            {
                var placeable = GridManager.Instance.SetCellTry(_grabbedObject.gameObject, _grabbedObject.transform.position, GridPositionState.REAL_WORLD);
                _grabbedObject.GetComponent<Pipe>()?.UpdateEnds(_grabbedObject.transform.eulerAngles);
                var neighbors = CanBePlaced(_grabbedObject);
                _grabbedObject.Drop(placeable && neighbors, PlacedMaterial);
            }
            if (_ghost != null)
                Destroy(_ghost);
            _grabbedObject = null;
            _grabbedDistance = 0;
        }

        private static bool CanBePlaced(PlaceableObject obj)
        {
            var pipe = obj.GetComponent<Pipe>();

            var neigbors = new[] { Vector3.up, Vector3.down, Vector3.forward, Vector3.back, Vector3.left, Vector3.right, };
            var pipes = neigbors.Select(
                n => new
                {
                    Direction = n,
                    Cell = GridManager.Instance.GetCell(GridManager.Instance.WorldToGrid(obj.transform.position) + n, GridPositionState.GRID_CELL)?.GetComponent<Pipe>()
                }).Where(p => p.Cell != null).ToList();

            if (!pipes.Any())
            {
                Debug.Log($"{obj.name}: No Neighbor Pipes");
                return false;
            }

            Debug.Log($"{obj.name}: ({pipe.Ends.Aggregate(string.Empty, (res, e) => res + e + " ", res => res.TrimEnd())})");
            foreach (var p in pipes)
            {
                Debug.Log($" --> {p.Cell.Ends.Aggregate(string.Empty, (res, e) => res + e + " ")}");
            }
            var cell = pipes.First();
            foreach (var end in pipe.Ends)
            {
                switch (end)
                {
                    case PipeEnd.Top:
                        cell = pipes.FirstOrDefault(c => c.Direction == Vector3.up);
                        if (cell != null && cell.Cell.Ends.Contains(PipeEnd.Bottom))
                        {
                            return true;
                        }
                        break;
                    case PipeEnd.Bottom:
                        cell = pipes.FirstOrDefault(c => c.Direction == Vector3.down);
                        if (cell != null && cell.Cell.Ends.Contains(PipeEnd.Top))
                        {
                            return true;
                        }
                        break;
                    case PipeEnd.Left:
                        cell = pipes.FirstOrDefault(c => c.Direction == Vector3.left);
                        if (cell != null && cell.Cell.Ends.Contains(PipeEnd.Right))
                        {
                            return true;
                        }
                        break;
                    case PipeEnd.Right:
                        cell = pipes.FirstOrDefault(c => c.Direction == Vector3.right);
                        if (cell != null && cell.Cell.Ends.Contains(PipeEnd.Left))
                        {
                            return true;
                        }
                        break;
                    case PipeEnd.Front:
                        cell = pipes.FirstOrDefault(c => c.Direction == Vector3.forward);
                        if (cell != null && cell.Cell.Ends.Contains(PipeEnd.Back))
                        {
                            return true;
                        }
                        break;
                    case PipeEnd.Back:
                        cell = pipes.FirstOrDefault(c => c.Direction == Vector3.back);
                        if (cell != null && cell.Cell.Ends.Contains(PipeEnd.Front))
                        {
                            return true;
                        }
                        break;
                }
            }

            Debug.Log($" --> No Match");
            return false;
        }
    }
}
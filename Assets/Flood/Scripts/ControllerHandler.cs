using UnityEngine;

namespace Flood
{
    using System;
    using System.Linq;
    using System.Runtime.InteropServices;

    using Flood.Utils;

    using UnityEngine.XR;

    using UnityGLTF;

    public class ControllerHandler : MonoBehaviour
    {
        public GameObject MRCameraParent;

        public GameObject TeleportMarker;

        public float GrabDistance = 0.3f;
        public float GrabOffset = 0.2f;
        public float CarryDistance = 0.4f;

        private GameObject _left;
        private GameObject _right;

        private PlaceableObject _grabbedObject;
        private float _grabbedDistance;


        private Vector3 _localRotation;

        private static readonly Quaternion HandRotationOffset = Quaternion.Euler(34.054f, -15.144f, -8.618f);

        // Use this for initialization
        private void Start ()
        {
            _left = new GameObject("left");
            _left.transform.parent = transform;

            _right = new GameObject("right");
            _right.transform.parent = transform;

            var pointer = GameObject.CreatePrimitive(PrimitiveType.Cube);
            pointer.transform.localScale = Vector3.one * 0.01f + Vector3.forward * 9.9f;
            pointer.transform.position = Vector3.forward * 5f;
            pointer.transform.parent = _right.transform;
            pointer.GetComponent<Collider>().enabled = false;
        }
	
        // Update is called once per frame
        private void Update ()
        {
            SetControllerTransform(_left.transform, XRNode.LeftHand);
            SetControllerTransform(_right.transform, XRNode.RightHand);

            if (TeleportMarker.activeSelf && AxisToButtonUtil.Instance.IsUp("CONTROLLER_RIGHT_STICK_VERTICAL"))
            {
                var camOffset = Vector3.forward * Camera.main.transform.localPosition.z + Vector3.right * Camera.main.transform.localPosition.x;
                MRCameraParent.transform.position = TeleportMarker.transform.position - camOffset;
                return;
            }
            if (_grabbedObject == null && AxisToButtonUtil.Instance.IsPressed("CONTROLLER_RIGHT_STICK_VERTICAL"))
            {
                TeleportMarker.SetActive(true);

                RaycastHit hit;
                var ray = new Ray(_right.transform.position, _right.transform.forward);
                if (Physics.Raycast(ray, out hit, 10, LayerMask.GetMask("Floor")))
                {
                    TeleportMarker.transform.position = hit.point;
                }
                else
                {
                    TeleportMarker.SetActive(false);
                }
                return;
            }
            TeleportMarker.SetActive(false);

            if (AxisToButtonUtil.Instance.IsPressed("CONTROLLER_RIGHT_TRIGGER"))
            {
                var ray = new Ray(_right.transform.position - _right.transform.forward * GrabOffset, _right.transform.forward);
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
                            return;
                        }
                        _grabbedObject.Take();
                        _grabbedDistance = _grabbedObject.transform.localScale.x / 2 + GrabOffset + CarryDistance; // hit.distance;
                        _localRotation = Vector3.zero;
                    }
                    else
                    {
                        _grabbedDistance = -1;
                    }
                    return;
                }

                _localRotation += 10 * new Vector3(Input.GetAxis("CONTROLLER_RIGHT_STICK_VERTICAL"), Input.GetAxis("CONTROLLER_RIGHT_STICK_HORIZONTAL"), 0);

                _grabbedObject.transform.position = ray.origin + ray.direction * (_grabbedDistance);
                _grabbedObject.transform.rotation = _right.transform.rotation * Quaternion.Euler(_localRotation);
                return;
            }
            if (_grabbedObject != null)
            {
                var placeable = GridManager.Instance.SetCellTry(_grabbedObject, _grabbedObject.transform.position, GridPositionState.REAL_WORLD);
                _grabbedObject.GetComponent<Pipe>()?.UpdateEnds(_grabbedObject.transform.eulerAngles);
                var neighbors = CanBePlaced(_grabbedObject);
                _grabbedObject.Drop(placeable && neighbors);
            }
            _grabbedObject = null;
            _grabbedDistance = 0;
        }

        private bool CanBePlaced(PlaceableObject obj)
        {
            var pipe = obj.GetComponent<Pipe>();

            var neigbors = new [] { Vector3.up, Vector3.down, Vector3.forward, Vector3.back, Vector3.left, Vector3.right, };
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

        private void SetControllerTransform(Transform transform, XRNode node)
        {
            transform.position = InputTracking.GetLocalPosition(node) + MRCameraParent.transform.position;
            transform.rotation = InputTracking.GetLocalRotation(node) * HandRotationOffset;
        }
    }
}


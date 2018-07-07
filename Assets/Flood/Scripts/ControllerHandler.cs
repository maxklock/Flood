using UnityEngine;

namespace Flood
{
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
                Debug.Log("stick up: " + TeleportMarker.activeSelf + ", " + TeleportMarker.activeInHierarchy);
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
                        Debug.Log("grab: " + hit.transform.gameObject.name);
                        _grabbedObject = hit.transform.gameObject.GetComponent<PlaceableObject>();
                        if (_grabbedObject == null)
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
                if (GridManager.Instance.SetCellTry(_grabbedObject, _grabbedObject.transform.position, GridPositionState.REAL_WORLD))
                {
                    _grabbedObject.Drop();
                }
            }
            _grabbedObject = null;
            _grabbedDistance = 0;
        }

        private void SetControllerTransform(Transform transform, XRNode node)
        {
            transform.position = InputTracking.GetLocalPosition(node) + MRCameraParent.transform.position;
            transform.rotation = InputTracking.GetLocalRotation(node) * HandRotationOffset;
        }
    }
}


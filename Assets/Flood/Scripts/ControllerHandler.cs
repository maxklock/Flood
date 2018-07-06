using UnityEngine;

namespace Flood
{
    using System.Runtime.InteropServices;

    using Flood.Utils;

    using UnityEngine.XR;

    using UnityGLTF;

    public class ControllerHandler : MonoBehaviour
    {
        private GameObject _left;
        private GameObject _right;

        private GameObject _grabbedObject;
        private float _grabbedDistance;

        public GameObject MRCameraParent;

        public GameObject TeleportMarker;

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

            var ray = new Ray(_right.transform.position, _right.transform.forward);

            if (_grabbedObject == null && AxisToButtonUtil.Instance.IsPressed("CONTROLLER_RIGHT_STICK_VERTICAL"))
            {
                TeleportMarker.SetActive(true);

                RaycastHit hit;
                if (Physics.Raycast(ray, out hit, 10, LayerMask.GetMask("Floor")))
                {
                    TeleportMarker.transform.position = hit.point;
                    if (AxisToButtonUtil.Instance.IsUp("CONTROLLER_RIGHT_TRIGGER"))
                    {
                        var camOffset = Vector3.forward * Camera.main.transform.localPosition.z + Vector3.right * Camera.main.transform.localPosition.x;
                        MRCameraParent.transform.position = hit.point - camOffset;
                    }
                }
                return;
            }
            TeleportMarker.SetActive(false);

            if (AxisToButtonUtil.Instance.IsPressed("CONTROLLER_RIGHT_TRIGGER"))
            {
                if (_grabbedDistance < 0)
                {
                    return;
                }
                if (_grabbedObject == null)
                {
                    RaycastHit hit;
                    if (Physics.Raycast(ray, out hit, 0.3f))
                    {
                        Debug.Log("grab: " + hit.transform.gameObject.name);
                        _grabbedObject = hit.transform.gameObject;
                        _grabbedDistance = Vector3.Distance(ray.origin, _grabbedObject.transform.position);
                    }
                    else
                    {
                        _grabbedDistance = -1;
                    }
                    return;
                }
                Debug.Log("move:");
                _grabbedObject.transform.position = ray.origin + ray.direction * _grabbedDistance;
                _grabbedObject.transform.rotation = _right.transform.rotation;
                //_grabbedObject.transform.Rotate(Vector3.up, Input.GetAxis("CONTROLLER_RIGHT_STICK_HORIZONTAL") * 10, Space.Self);
                //_grabbedObject.transform.Rotate(Vector3.left, Input.GetAxis("CONTROLLER_RIGHT_STICK_VERTICAL") * 10, Space.Self);
                return;
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


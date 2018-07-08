namespace Flood.MixedReality
{
    using System;
    using System.Linq;

    using HoloToolkit.Unity;
    using HoloToolkit.Unity.InputModule;

    using UnityEngine;
    using UnityEngine.XR;

    public class ControllerHandler : SingleInstance<ControllerHandler>
    {
        private static readonly Quaternion HandRotationOffset = Quaternion.Euler(34.054f, -15.144f, -8.618f);

        public GameObject MRCameraParent;

        [HideInInspector]
        public GameObject LeftController;

        [HideInInspector]
        public GameObject RightController;

        // Use this for initialization
        private void Start ()
        {
            if (MRCameraParent == null)
            {
                MRCameraParent = FindObjectOfType<MixedRealityTeleport>().gameObject;
            }
            LeftController = new GameObject("left");
            LeftController.transform.parent = transform;

            RightController = new GameObject("right");
            RightController.transform.parent = transform;

            var pointer = GameObject.CreatePrimitive(PrimitiveType.Cube);
            pointer.transform.localScale = Vector3.one * 0.01f + Vector3.forward * 9.9f;
            pointer.transform.position = Vector3.forward * 5f;
            pointer.transform.parent = RightController.transform;
            pointer.GetComponent<Collider>().enabled = false;
        }
	
        // Update is called once per frame
        private void Update ()
        {
            SetControllerTransform(LeftController.transform, XRNode.LeftHand);
            SetControllerTransform(RightController.transform, XRNode.RightHand);
        }

        private void SetControllerTransform(Transform transform, XRNode node)
        {
            transform.position = InputTracking.GetLocalPosition(node) + MRCameraParent.transform.position;
            transform.rotation = InputTracking.GetLocalRotation(node) * HandRotationOffset;
        }
    }
}


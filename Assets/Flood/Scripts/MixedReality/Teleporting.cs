﻿namespace Flood.MixedReality
{
    using Flood.Utils;

    using HoloToolkit.Unity;

    using UnityEngine;

    public class Teleporting : MonoBehaviour
    {

        public GameObject TeleportMarker;

        // Use this for initialization
        private void Start()
        {

        }

        // Update is called once per frame
        private void Update()
        {
            var right = ControllerHandler.Instance.RightController;
            if (TeleportMarker.activeSelf && AxisToButtonUtil.Instance.IsUp("CONTROLLER_RIGHT_STICK_VERTICAL"))
            {
                var camOffset = Camera.main.transform.position - ControllerHandler.Instance.MRCameraParent.transform.position;
                camOffset.y = TeleportMarker.transform.position.y;
                ControllerHandler.Instance.MRCameraParent.transform.position = TeleportMarker.transform.position - camOffset;
                return;
            }
            if (AxisToButtonUtil.Instance.IsPressed("CONTROLLER_RIGHT_STICK_VERTICAL"))
            {
                TeleportMarker.SetActive(true);

                RaycastHit hit;
                var ray = new Ray(right.transform.position + right.transform.forward * 0.5f, right.transform.forward);
                if (Physics.Raycast(ray, out hit, 10) && hit.transform.gameObject.IsInLayerMask(LayerMask.GetMask("Floor")))
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
        }
    }
}
using UnityEngine;

namespace Flood
{
    using Flood.Utils;

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
                var camOffset = Vector3.forward * Camera.main.transform.localPosition.z + Vector3.right * Camera.main.transform.localPosition.x;
                ControllerHandler.Instance.MRCameraParent.transform.position = TeleportMarker.transform.position - camOffset;
                return;
            }
            if (AxisToButtonUtil.Instance.IsPressed("CONTROLLER_RIGHT_STICK_VERTICAL"))
            {
                TeleportMarker.SetActive(true);

                RaycastHit hit;
                var ray = new Ray(right.transform.position, right.transform.forward);
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
        }
    }
}
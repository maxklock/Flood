using UnityEngine;
using UnityEngine.SceneManagement;

namespace Flood
{
    using HoloToolkit.Unity;
    using HoloToolkit.Unity.InputModule;

    public class LevelManager : MonoBehaviour
    {
        public GameObject MRPrefab;
        private GameObject _mrCameraParent;
        public string MenuScene = "Menu";

        public void LoadScene(string scene)
        {
            _mrCameraParent.transform.position = Vector3.zero;
            SceneManager.LoadScene(scene);
        }

        public void BackToMenu()
        {
            _mrCameraParent.transform.position = Vector3.zero;
            SceneManager.LoadScene(MenuScene);
        }

        // Use this for initialization
        private void Awake()
        {
            _mrCameraParent = FindObjectOfType<MixedRealityTeleport>()?.gameObject ?? Instantiate(MRPrefab);
        }

        // Update is called once per frame
        private void Update()
        {

        }
    }
}
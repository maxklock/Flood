using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class LevelManager : MonoBehaviour
{
    public GameObject MRCameraParent;

    public void LoadScene(string scene)
    {
        MRCameraParent.transform.position = Vector3.zero;
        SceneManager.LoadScene(scene);
    }

    public void BackToMenu()
    {
        
    }

    // Use this for initialization
    void Start()
    {

    }

    // Update is called once per frame
    void Update()
    {

    }
}
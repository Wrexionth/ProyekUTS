using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class ContinueBtn : MonoBehaviour {

    public string scene;
    // Use this for initialization
    void Start()
    {
        GetComponent<Button>().onClick.AddListener(Continue);

    }

    // Update is called once per frame
    void Update()
    {

    }
    private void Continue()
    {
        Character.reset();
        SceneManager.LoadScene(scene);
    }
}

using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class CreditsBtn : MonoBehaviour {

    // Use this for initialization
    void Start()
    {
        GetComponent<Button>().onClick.AddListener(onclicked);
    }

    // Update is called once per frame
    void Update()
    {

    }

    private void onclicked()
    {
        Character.gameover = false;
        SceneManager.LoadScene("Credits");
    }
}

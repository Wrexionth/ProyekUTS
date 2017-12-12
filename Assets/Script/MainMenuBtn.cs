using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class MainMenuBtn : MonoBehaviour {

	// Use this for initialization
	void Start ()
    {
        GetComponent<Button>().onClick.AddListener(Menu);

    }
	
	// Update is called once per frame
	void Update () {
		
	}
    private void Menu()
    {
        SceneManager.LoadScene("Menu");
    }
}

﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class ExitBtn : MonoBehaviour {

	// Use this for initialization
	void Start () {
        GetComponent<Button>().onClick.AddListener(exit);
	}
	
	// Update is called once per frame
	void Update () {
		
	}

    private void exit()
    {
        Application.Quit();//ignored in editor
    }
}

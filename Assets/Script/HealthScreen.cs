using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class HealthScreen : MonoBehaviour {

    // Use this for initialization
    private Text health;
	void Start () {
        health = GetComponent<Text>();
	}
	
	// Update is called once per frame
	void Update () {
        health.transform.position = new Vector3(0, 1, 0);
	}
}

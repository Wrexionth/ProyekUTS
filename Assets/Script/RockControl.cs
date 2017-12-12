using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RockControl : MonoBehaviour {
    
    // Use this for initialization
    void Start () {
        
    }
	
	// Update is called once per frame
	void Update () {

    }
    private void OnCollisionEnter2D(Collision2D collision)
    {
        if (collision.gameObject.tag == "Platform" || collision.gameObject.tag == "Mud" || collision.gameObject.tag == "Player" || collision.gameObject.tag == "Weapon")
        {
            transform.position = new Vector3(transform.position.x, Random.Range(10, 30), 0);
        }
    }
}

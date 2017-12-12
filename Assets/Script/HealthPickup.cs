using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HealthPickup : MonoBehaviour {

    public int HealthToAdd;

    void OnTriggerEnter2D(Collider2D other)
    {
        if (other.GetComponent<Character>() == null)
        {
            return;
        }
        Destroy(gameObject);
    }
}

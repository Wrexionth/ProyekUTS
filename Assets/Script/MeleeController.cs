using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MeleeController : MonoBehaviour {
    
    public Character player;

    public int pointsForKill;
    
    // Use this for initialization
    void Start()
    {
    }

    void FixedUpdate()
    {
    }
    // Update is called once per frame
    void Update()
    {
        if (player.dir == "right") transform.position = new Vector3(player.transform.position.x + 0.7f, player.transform.position.y + 0.55f, 1);
        else transform.position = new Vector3(player.transform.position.x - 0.7f, player.transform.position.y + 0.55f, 1);
    }
    
}

using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraControl : MonoBehaviour {

    // Use this for initialization
    public GameObject player;
    public static bool bossFight;
    void Start () {
        bossFight = false;
    }

    // Update is called once per frame
    void LateUpdate()
    {
        if (!bossFight) transform.position = new Vector3(player.transform.position.x, transform.position.y, -10);
    }
    public static void CameraLock()
    {
        bossFight = true;
    }
}

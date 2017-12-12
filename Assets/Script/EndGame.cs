using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EndGame : MonoBehaviour {

    [Tooltip("text untuk story3(: -> enter, ; -> clear)")]
    public string story3 = "";
    // Use this for initialization
    void Start () {
        GameObject.Find("ScoreText").SetActive(true);
        GameObject.Find("ExtBtn").SetActive(true);
    }
	
	// Update is called once per frame
	void Update () {

    }
    IEnumerator ShowStory(string story, string textmesh_target)
    {
        foreach (char letter in story.ToCharArray())
        {
            if (letter == ':')
            {
                GameObject.Find(textmesh_target).GetComponent<TextMesh>().text += "\n";
            }
            else if (letter == ';')
            {
                GameObject.Find(textmesh_target).GetComponent<TextMesh>().text = "";
            }
            else GameObject.Find(textmesh_target).GetComponent<TextMesh>().text += letter;
            Debug.Log(letter + "\n");
            yield return new WaitForSeconds(0.1f);
        }
        GameObject.Find(textmesh_target).GetComponent<TextMesh>().text = "";
        
    }
}

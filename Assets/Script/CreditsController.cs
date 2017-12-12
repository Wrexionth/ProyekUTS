using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class CreditsController : MonoBehaviour {

    public Text credits;
    public Text titles;
    public Image images;
    public Image endImage;
    private string fade;
    [Tooltip("Title")]
    public string[] list_title;
    [Tooltip("Story")]
    public string[] list_story;
    [Tooltip("Story saat ini")]
    public int currentStory;
    [Tooltip("per image per kalimat")]
    public Sprite[] img;    
    // Use this for initialization

    void Start () {
        credits.text = list_story[currentStory].Replace('$', '\n');
        titles.text = list_title[currentStory].Replace('$', '\n');
        images.sprite = img[currentStory];
        fade = "in";
	}

    // Update is called once per frame
    void Update() {
        if (currentStory < list_story.Length - 1)
        {
            if (fade == "out")
            {
                credits.color = new Color(255f, 255f, 255f, credits.color.a - 0.01f);
                titles.color = new Color(255f, 255f, 255f, credits.color.a);
                images.color = new Color(255f, 255f, 255f, credits.color.a);
                if (credits.color.a <= 0)
                {
                    currentStory++;
                    credits.text = list_story[currentStory].Replace('$', '\n');
                    titles.text = list_title[currentStory].Replace('$', '\n');
                    images.sprite = img[currentStory];
                    Debug.Log(currentStory);
                    fade = "in";
                }
            }
            else
            {
                credits.color = new Color(255f, 255f, 255f, credits.color.a + 0.01f);
                titles.color = new Color(255f, 255f, 255f, credits.color.a);
                images.color = new Color(255f, 255f, 255f, credits.color.a);
                if (credits.color.a >= 1.5) fade = "out";
            }
        }
        else
        {
            if (fade == "out")
            {
                credits.color = new Color(255f, 255f, 255f, credits.color.a - 0.01f);
                titles.color = new Color(255f, 255f, 255f, credits.color.a);
                if (credits.text == "Thank you for playing") endImage.color = new Color(255f, 255f, 255f, credits.color.a);
                if (credits.color.a <= 0)
                {
                    credits.text = "Thank you for playing";
                    titles.text = "";
                    fade = "in";
                    Invoke("Mainmenu", 5f);
                }
            }
            else
            {
                credits.color = new Color(255f, 255f, 255f, credits.color.a + 0.01f);
                titles.color = new Color(255f, 255f, 255f, credits.color.a);
                images.color = new Color(255f, 255f, 255f, credits.color.a);
                if (credits.text == "Thank you for playing") endImage.color = new Color(255f, 255f, 255f, credits.color.a);
                if (credits.color.a >= 1.5) fade = "out";
            }
        }
    }
    private void Mainmenu()
    {
        SceneManager.LoadScene("Menu");
    }
}

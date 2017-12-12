using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class StoryController : MonoBehaviour {

    //bool check = true;
    private Text content;
    private Image bg;
    

    [Header("Component")]
    [Tooltip("Fast forward button")]
    public Button ff;
    [Tooltip("Auto button")]
    public Button auto;
    [Tooltip("Next scenario button")]
    public Button next;
    [Tooltip("Volume up button")]
    public Button vup;
    [Tooltip("Volume down button")]
    public Button vdown;
    [Tooltip("Mute button")]
    public Button vmute;

    [Header("Content")]
    [Tooltip("Story")]
    public string[] list_story;
    [Tooltip("list index story saat ini, checkbox satu aja")]
    public bool[] storyStatus;
    [Tooltip("per image per kalimat")]
    public Sprite[] img;
    
    [Header("")]
    //[Range(0.0010f, 0.0100f)]
    [Tooltip("In seconds (default: 0.0050f)")]
    public float textSpeed = 0.0050f;

    [Header("Auto text setting")]
    //[Range(0.00100f, 10.00000f)]
    [Tooltip("In seconds (default: 1.0000f)")]
    public float auto_delayToNextSentence = 1.00000f;

    [Header("Fast forward text setting")]
    //[Range(0.00050f, 0.01000f)]
    [Tooltip("In seconds (default: 0.00750f)")]
    public float ff_delayToNextSentence = 0.00750f;

    //[Header("")]
    //[Tooltip("In microseconds (default: 100)")]
    //public int delayToNextScenario = 10;

    [Header("")]
    [Tooltip("Nama scenario berikutnya(match)")]
    public string nextScenario = "";
    
    Sprite nextSprite = null;
    
    bool isStart = false, isAuto = false, isFF = false, isInterrupt = false, isPartial = false, isProcess = true, afterInterrupt = false, isKey = false;
    int idx = -1, idx_part = 0, tempCountChar = -1;
    string logStory = "";
    // Use this for initialization
    void Awake () {
        content = GameObject.Find("txt01").GetComponent<Text>();
        bg = GameObject.Find("bg01").GetComponent<Image>();
        content.text = "";
        for (int i = 0; i < storyStatus.Length; i++)
        {
            if (storyStatus[i])
            {
                idx = i;
                bg.sprite = img[i];
                break;
            }
        }
    }

    void Start()
    {
        auto.onClick.AddListener(onClickAuto);
        ff.onClick.AddListener(onClickFastForward);
        next.onClick.AddListener(onClickNext);
        vup.onClick.AddListener(onClickVolUp);
        vdown.onClick.AddListener(onClickVolDown);
        vmute.onClick.AddListener(onClickVolMute);
        Debug.Log("start engine");
        GameObject.Find("SoundManager").GetComponent<AudioSource>().Play();
        tempCountChar = countChar(list_story[idx], ';');
    }

    // Update is called once per frame
    void Update()
    {
        #region MyRegion
        //if (!isAuto && !isFF && (Input.GetKeyDown(KeyCode.Z) || Input.GetMouseButtonDown(1)))
        //{
        //    if (!Input.GetButtonDown("BtnAuto") && !Input.GetButtonDown("BtnFF") && !Input.GetButtonDown("BtnNext") && !Input.GetButtonDown("BtnVU") && !Input.GetButtonDown("BtnVD") && !Input.GetButtonDown("BtnVM"))
        //    {
        //        Debug.Log('2');
        //    }
        //    Debug.Log("1");
        //    return;
        //}
        //if (Input.GetKeyDown(KeyCode.Z) && !isProcess)
        //{
        //    //if (!afterInterrupt)
        //    //{
        //    //    if (idx_part + 1 < tempCountChar) idx_part++;
        //    //    else if (idx_part + 1 == tempCountChar)
        //    //    {
        //    //        if (idx + 1 < list_story.Length)
        //    //        {
        //    //            idx++;
        //    //            tempCountChar = countChar(list_story[idx], ';');
        //    //            idx_part = 0;
        //    //        }
        //    //        else onClickNext();
        //    //    }
        //    //    isPartial = true;
        //    //}
        //    //else
        //    //{
        //    //    isInterrupt = false;
        //    //    isAuto = false;
        //    //    isFF = false;
        //    //}

        //    if (idx_part + 1 < tempCountChar) idx_part++;
        //    else if (idx_part + 1 == tempCountChar)
        //    {
        //        if (idx + 1 < list_story.Length)
        //        {
        //            idx++;
        //            tempCountChar = countChar(list_story[idx], ';');
        //            idx_part = 0;
        //        }
        //        else onClickNext();
        //    }
        //    nextSprite = img[idx];
        //    bg.sprite = nextSprite;
        //    isPartial = true;
        //    isKey = true;
        //}
        #endregion
        if (isAuto || isFF) playall();
        else if (isKey) playall();
    }

    void playall()
    {
        if (isStart)
        {
            #region MyRegion
            //for (int i = 0; i < list_story.Length; i++)
            //{
            //    if (storyStatus[i])
            //    {
            //        current_story = list_story[i];
            //        if (i + 1 < storyStatus.Length) nextSprite = img[i + 1];
            //        StartCoroutine(readStory());
            //        break;
            //    }
            //}
            #endregion
            System.GC.Collect();
            if (storyStatus[idx])
            {
                Debug.Log(idx);
                if (idx + 1 < storyStatus.Length) nextSprite = img[idx + 1];
                StartCoroutine(readStory(list_story[idx]));
            }
            isStart = false;
        }
        else if (isPartial)
        {
            System.GC.Collect();
            StartCoroutine(readStory(list_story[idx].Split(';')[idx_part] + ";"));
            isPartial = false;
            isKey = false;
        }
        isProcess = true;
    }

    void onClickAuto()
    {
        if (!isInterrupt)
        {
            if (!isFF && !isAuto)
            {
                //Debug.Log("auto on");
                isAuto = true;
                if (!isStart) isStart = true;
                //if (!isStart && !Input.GetKeyDown(KeyCode.Z)) isStart = true;
            }
            else if (!isFF && isAuto)
            {
                isInterrupt = true;
                //afterInterrupt = true;
                isAuto = false;
            }
        }
        else
        {
            isInterrupt = false;
            isFF = false;
            isAuto = true;
        }
    }

    void onClickFastForward()
    {
        if (!isInterrupt)
        {
            if (!isFF && !isAuto)
            {
                //Debug.Log("fast forward on");
                isFF = true;
                if (!isStart) isStart = true;
            }
            else if (isFF && !isAuto)
            {
                isInterrupt = true;
                isFF = false;
            }
        }
        else
        {
            isInterrupt = false;
            isFF = true;
            isAuto = false;
        }
        
    }

    void onClickNext()
    {
        Debug.Log("next");
        bg.sprite = null;
        bg.color = Color.black;
        content.text = "";
        //System.Threading.Thread.Sleep(delayToNextScenario);
        if (nextScenario != "")
        {
            GameObject.Find("SoundManager").GetComponent<AudioSource>().Stop();
            SceneManager.LoadScene(nextScenario);
        }
    }

    void onClickVolUp()
    {
        float current = GameObject.Find("SoundManager").GetComponent<AudioSource>().volume;
        if (current < 1f)
        {
            current += 0.1f;
        }
        GameObject.Find("SoundManager").GetComponent<AudioSource>().volume = current;
    }

    void onClickVolDown()
    {
        float current = GameObject.Find("SoundManager").GetComponent<AudioSource>().volume;
        if (current > 0f)
        {
            current -= 0.1f;
        }
        GameObject.Find("SoundManager").GetComponent<AudioSource>().volume = current;
    }

    void onClickVolMute()
    {
        //vmute.GetComponent<Text>().text = (vmute.GetComponent<Text>().text == "Music On") ? "Music Off" : "Music On";
        
        GameObject.Find("SoundManager").GetComponent<AudioSource>().mute = !(GameObject.Find("SoundManager").GetComponent<AudioSource>().mute);
    }

    IEnumerator readStory(string sentence)
    {
        //Debug.Log("|||start story|||");
        int sumOfWrittenStr = 0, startIdx = 0;
        //string s = current_story;
        content.text = "";
        content.lineSpacing = 1f;
        //Debug.Log(sentence != logStory);
        //if (sentence != logStory )
        //{
        //    Debug.Log(list_story[idx].Contains(sentence));
        //if (list_story[idx] != sentence && list_story[idx].Contains(sentence))
        //{
        //    Debug.Log("kepanggil" + "\n");
        //    Debug.Log("sentence : " + sentence);
        //    Debug.Log("liststory (" + idx + ")  : " + list_story[idx]);
        //    Debug.Log("logstory (" + (idx - 1) + ")  : " + logStory);
        //    sumOfWrittenStr = list_story[idx].IndexOf(';') + 1;
        //    startIdx = sumOfWrittenStr;
        //}

        for (int i = startIdx; i < sentence.Length; i++)
        {
            if (isInterrupt == false)
            {
                sumOfWrittenStr++;
                if (sentence[i] == ':') content.text += "\n";
                else if (sentence[i] != '.' && sentence[i] != ';') content.text += sentence[i];
                if (sentence[i] == ';')
                {
                    yield return (isFF) ? new WaitForSeconds(ff_delayToNextSentence) : new WaitForSeconds(auto_delayToNextSentence);
                    if (sumOfWrittenStr < sentence.Length - 1) content.text = "";
                }
                else yield return new WaitForSeconds(textSpeed);
            }
            yield return new WaitUntil(() => isInterrupt == false);
        }
        #region MyRegion
        //int temp = countChar(sentence, ';');
        //if (temp == 1)
        //{

        //}
        //else
        //{
        //    if (list_story[idx].Contains(logStory))
        //    {

        //    }
        //    else if (idx - 1 > 0)
        //    {
        //        if (list_story[idx - 1] == logStory)
        //        {

        //        }
        //    }
        //    //string[] tempStory = sentence.Split(';');
        //    //string now = "";
        //    //if (sentence.Contains(logStory) && logStory != "")
        //    //{
        //    //    startIdx = sentence.IndexOf(logStory) + 1;
        //    //}

        //    //for (int i = 0; i < tempStory.Length; i++)
        //    //{
        //    //    now = tempStory[i];
        //    //    for (int j = 0; j < now.Length; j++)
        //    //    {

        //    //    }
        //    //}
        //}

        //Debug.Log("|||ended story|||");
        #endregion
        fadein(sentence);
    }

    void fadein(string sentence)
    {
        //Debug.Log("---start fadein---");
        if (bg.sprite != nextSprite && nextSprite != null) bg.sprite = nextSprite;
        #region MyRegion
        //for (int i = 0; i < list_story.Length; i++)
        //{
        //    if (current_story == list_story[i])
        //    {
        //        if (i + 1 < storyStatus.Length)
        //        {
        //            isStart = true;
        //            storyStatus[i] = false;
        //            storyStatus[i + 1] = true;
        //            break;
        //        }
        //        else
        //        {
        //            storyStatus[i] = false;
        //            bg.sprite = null;
        //            bg.color = Color.black;
        //            content.text = "";
        //            //yield return new WaitForSeconds(delayToNextScenario);
        //            if (nextScenario != "")
        //            {
        //                GameObject.Find("SoundManager").GetComponent<AudioSource>().Stop();
        //                SceneManager.LoadScene(nextScenario);
        //            }

        //        }
        //    }
        //}
        #endregion
        if (sentence == list_story[idx])
        {
            //afterInterrupt = false;
            if (idx + 1 < storyStatus.Length)
            {
                isStart = true;
                storyStatus[idx] = false;
                storyStatus[idx + 1] = true;
                idx++;
            }
            else
            {
                Debug.Log("next");
                storyStatus[idx] = false;
                bg.sprite = null;
                bg.color = Color.black;
                content.text = "";
                //yield return new WaitForSeconds(delayToNextScenario);
                if (nextScenario != "")
                {
                    GameObject.Find("SoundManager").GetComponent<AudioSource>().Stop();
                    SceneManager.LoadScene(nextScenario);
                }

            }
        }
        isProcess = false;
        logStory = sentence;
        //Debug.Log(isProcess);
        //Debug.Log("---ended fadein---");
    }
    int countChar(string txt, char c)
    {
        int ctr = 0;
        for (int i = 0; i < txt.Length; i++)
        {
            ctr += (txt[i] == c) ? 1 : 0;
        }
        return ctr;
    }
}

#region story
//Story 1
//private string[] list_story = new string[] {
//"In the past.......;The human still in their glory days:Their technology can overcame any problem.....;The war have not any place in the earth:Natural resources can recover as time passes.....;",
//"Shop have more organic foods:People still can eat healthy junk food.....;Childrens can feel the nature in the middle of city:Bad weather can be regulated into good weather...;",
//"Happiness..., smile..., warm family..., healthy body..;",
//"No sadness..., no pain..., no poverty..., no crime...;And that is the big mistake for human...;",
//"Sudden war between abnormal races.....;Angel...., Devil...., Beastman.... and Monster.....;Then which side is human?.....:Neither of them.....;Why?.....:Can't use magic....., Weak body....., Short life force",
//"That's way human can't just give up their own life;Family....., friends....., lover..... and nation.....;",
//"Human will use whatever they have:to kill their own nemesis.....;Money....., bio weapon....., mass weapon.....: and even subject experiment.....;
//But that was pointless......;Hard skin devil..., high rank angel...,monster with huge body and:polution from devil's dead body;But the most big damage is.....;Goblin.....;",
//"The most weak sub species from devil..:Most of them have child size..;But the terrified things from them was not that..;",
//"Skirmish on their escape route.., steal woman.. and kill children..;Because of that...;population of human decreasing drastically.....;"
//};
/*
 * 
 * Story 2
 * 
 * img1 = destroyedcity05 = 3
 * My name is Kevin..., war orphan...;I'm live in this city,... or so i though...;
 * This city didn't have what it should needed as a city...:Adult throwing us into this city just to save their face...;
 * We..., must search our own food and drink...:Though that is not easy for weak children like us...;
 * 
 * img2 = forestcity01 = 8
 * Some part of city still live...:Just it through forestry from the leftover of monster's dead body...;
 * The outer side of building become mossy...:Sometime a bud grow from it...;Sometime the building will collapse from it...:Sometime an edible plant grow from it...;
 * But the most hard to live in this city is ...:We didn't found any edible meat...;So how we can live up until now?...;
 * Some of us move to the next settlement to get:the work and send the result back to this city...;
 * Some of us choose to live with the devil or angel...;Though not most of them come back alive...;
 * Some of us choose to give up:and just sit to wait their own death...;
 * Some of us choose to be a hunter:to find loot from the monster...;
 * We call them as a adventurer...;
 * 
 * img3 = forestcity02 = 3
 * But though have been all of them...:Our live in this city is not much change...;
 * Hunted..., killed..., slaverery...;That is the most peaceful scenery in this city...;
 * What we can do to avoid both of them is:only hide our weak body...;
 * 
 * img4 = destroyedcity01 = 4
 * This place is the real form of the city...;Weak building..., dirty road..., muddy water...,:polluted air..., gray sun...;
 * This place is even not worth to said as a place again...;But we still named it as a place...;Prisoner.....;
 * A place to hunted..., a place to take a slave...,:a place to have fun..., a place to relieve stress...;
 * I hate this place...;But i can't leave this place too easy or it will only get me killed...;
 * 
 * img5 = destroyedcity03 = 6
 * This place still enough to hide us from goblin...;Yes, we live together with goblin...;
 * At first this is only abandoned place...:Most of us believe with that...;
 * But a small army of goblin come:and make a settlement in this place...;
 * To make them not perceive us:we hide the smallest child in the building...;
 * But if some goblin come in the building...;We kill them in that place...;
 * That's way we still alive until now:though we live together with them...;
 * 
 * img6 = destroyedcity04 = 6
 * This place is the meeting place of oldest children...;
 * We though a strategy to live in this suck place...;
 * How to repel goblin...:How to minimize the victim...;
 * How to manage our food and water...:How to make the small children keep educated...;
 * How to fight as a weak human.....;
 * How to discard our dead body.....;
 * 
 * img7 = victim01 = 7
 * Ah, a new dead body again...:This is the 3rd time recently...;
 * Though we become more tolerate with our dead friends...;
 * It didn't make any changes:that a dead body can still in that place...;
 * A dead body can invite any nearby monster...;
 * Sometime goblin..., but sometime a huge one...;If we encounter the huge one:no doubt that we will die...;
 * But the most troublesome is the smell...;Dead body's smell will pollute this area:and make us can't live in this area again...;
 * That's way we discard it.....;
 * 
 * img8 = destroyedcity02 = 6
 * This is the place we discard that dead body...;
 * This place have a bottomless and wide hole...;I think this is the effect of:huge magic attack from angel...;
 * Know that this place still have some humans:they still attack us with that...;
 * But that is not make the devil more better...;Devil have their own way to make use of human...;
 * Slave magic..., brainwash magic..., torture...,:replenish their mana..., livestock...;
 * What is that ruckus?...;Goblin visiting us again?...:Then let's welcome it...;Don't think we just hide our body:without train it you damn goblin...;
 * 
 */

/*
 * Story 3
 * 
 * img 1 = Hero02 = 6
 * That is the last one...:Recently their movement become more active...;
 * Did the remnant of the last war still in there?...:If so, that is not good...;
 * I will not let them do:as they like...;I will protect this place...;
 * Wait, why the earth still crumbling...;Don't tell me ?!!!
 * Oh no, that is orc...;Their rank and strengh didn't have much:difference with goblin except their big body;
 * Then let's exterminate it before:they can enter this place;
 * 
 */

/*
 * Epilog
 * img 2 = DestroyedCity02 = 3
 * That is not funny...;If that orc enter the settlement, panic will:attack the people and the amount of victim will increase;
 * Kill it in this place is the correct decision...;But that is hard even with this kind of armament...;
 * But the most important is let's going back:to the settlement...;Aaahh, i hope my food still left:some taste before decaying...;
 * To be continue..........;
 */
#endregion
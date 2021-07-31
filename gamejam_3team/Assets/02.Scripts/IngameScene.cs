using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class IngameScene : MonoBehaviour
{
    AudioSource backgroundSource;
    AudioSource beatSource;
    public AudioClip[] clips = new AudioClip[2];
    public Canvas UICanvas;
    public Canvas NPCCanvas;

    Image[] npcImages;
    Image[] uiImages;

    bool turn = false;
    int noteIndex = 0;
    KeyCode[] notes;
    
    // Start is called before the first frame update
    void Start()
    {
        var audioSources = GetComponents<AudioSource>();
        beatSource = audioSources[0];
        backgroundSource = audioSources[1];
        var npcImageObjs = GameObject.FindGameObjectsWithTag("key");
        Dictionary<String, int> keyMap_ = new Dictionary<string, int>();
        keyMap_.Add("AsoundImage", 0);
        keyMap_.Add("SsoundImage", 1);
        keyMap_.Add("DsoundImage", 2);
        keyMap_.Add("FsoundImage", 3);
        keyMap_.Add("AkeyImage", 0);
        keyMap_.Add("SkeyImage", 1);
        keyMap_.Add("DkeyImage", 2);
        keyMap_.Add("FkeyImage", 3);
        npcImages = new Image[npcImageObjs.Length];
        for (int i = 0; i < npcImageObjs.Length; i++)
        {
            var image_ = npcImageObjs[i].GetComponent<Image>();
            var index = keyMap_[npcImageObjs[i].name];
            npcImages[index] = image_;
        }
        uiImages = UICanvas.GetComponentsInChildren<Image>();
        var uiImageObjs = GameObject.FindGameObjectsWithTag("uikey");
        uiImages = new Image[uiImageObjs.Length];
        for (int i = 0; i < uiImageObjs.Length; i++)
        {
            var image_ = uiImageObjs[i].GetComponent<Image>();
            var index = keyMap_[uiImageObjs[i].name];
            uiImages[index] = image_;
        }
        foreach (var i in uiImages) i.enabled = false;
        foreach (var i in npcImages) i.enabled = false;

        backgroundSource.clip = clips[1];
        backgroundSource.volume = 0.7f;
        beatSource.clip = clips[0];
        StartCoroutine(TestSequence());
    }
    
    IEnumerator TestSequence()
    {
        var waitForSeconds = new WaitForSeconds(2.0f);
        var waitForSeconds2 = new WaitForSeconds(0.5f);
        
        backgroundSource.Play();
        yield return waitForSeconds;
        for (int i = 0; i < 4; i++)
        {
            beatSource.clip = clips[0];
            beatSource.Play();
            npcImages[i].enabled = true;
            yield return waitForSeconds2;
        }
        notes = new KeyCode[] {KeyCode.A, KeyCode.S, KeyCode.D, KeyCode.F};
        turn = true;
        while (turn)
        {
            yield return waitForSeconds2;
        }
        foreach (var i in npcImages) i.enabled = false;
        foreach (var i in uiImages) i.enabled = false;
        
        for (int i = 0; i < 4; i++)
        {
            beatSource.clip = clips[0];
            beatSource.Play();
            npcImages[i].enabled = true;
            yield return waitForSeconds2;
        }
        
        notes = new KeyCode[] {KeyCode.A, KeyCode.S, KeyCode.D, KeyCode.F};
        turn = true;
        while (turn)
        {
            yield return waitForSeconds2;
        }
        foreach (var i in npcImages) i.enabled = false;
        
        GameManager.instance.endingType = EndingType.Happy;
        GameManager.instance.LoadScene("05.EndingScene");
    }

    void playerKeyCombo(KeyCode[] combo)
    {
        if (turn && Input.anyKeyDown)
        {
            timer += Time.deltaTime;
            if (timer > waitTime) failure();
            if (Input.GetKeyDown(notes[noteIndex]))
            {
                beatSource.Play();
                uiImages[noteIndex].enabled = true;
                noteIndex++;
                timer = 0.0f;
            }
            else
            {
                failure();
            }
            
            if (noteIndex >= notes.Length)
            {
                turn = false;
                noteIndex = 0;
                timer = 0.0f;
                
            }
        }
    }
    void failure()
    {
        turn = false;
        GameManager.instance.endingType = EndingType.Bad;
        GameManager.instance.LoadScene("05.EndingScene");
    }

    float timer = 0.0f;
    float waitTime = 0.7f;
    // Update is called once per frame
    void Update()
    {
        if (turn && Input.anyKeyDown)
        {
            playerKeyCombo(notes);
        }
    }
}

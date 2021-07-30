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
    // Start is called before the first frame update
    void Start()
    {
        var audioSources = GetComponents<AudioSource>();
        beatSource = audioSources[0];
        backgroundSource = audioSources[1];
        var npcImageObjs = GameObject.FindGameObjectsWithTag("key");
        npcImages = new Image[npcImageObjs.Length];
        for (int i = 0; i < npcImageObjs.Length; i++)
        {
            npcImages[i] = npcImageObjs[i].GetComponent<Image>();
        }
        uiImages = UICanvas.GetComponentsInChildren<Image>();
        var uiImageObjs = GameObject.FindGameObjectsWithTag("uikey");
        uiImages = new Image[uiImageObjs.Length];
        for (int i = 0; i < uiImageObjs.Length; i++)
        {
            uiImages[i] = uiImageObjs[i].GetComponent<Image>();
        }
        
        foreach (var i in uiImages) i.enabled = false;
        foreach (var i in npcImages) i.enabled = false;

        backgroundSource.clip = clips[1];
        beatSource.clip = clips[0];
        StartCoroutine(TestSequence());
    }

    IEnumerator TestSequence()
    {
        var waitForSeconds = new WaitForSeconds(2.0f);
        var waitForSeconds2 = new WaitForSeconds(0.5f);
        backgroundSource.clip = clips[1];
        backgroundSource.volume = 0.7f;
        backgroundSource.Play();
        yield return waitForSeconds;
        for (int i = 0; i < 4; i++)
        {
            beatSource.clip = clips[0];
            beatSource.Play();
            npcImages[i].enabled = true;
            yield return waitForSeconds2;
        }
        turn = true;
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
        if (turn)
        {
            timer += Time.deltaTime;
            if (timer > waitTime) failure(); 
            // A S D F
            // 틀린 입력에는?
            KeyCode[] notes = {KeyCode.A, KeyCode.S, KeyCode.D, KeyCode.F};
            KeyCode current;
            if (Input.GetKeyDown(notes[noteIndex]))
            {
                Debug.Log(timer);
                beatSource.Play();
                uiImages[noteIndex].enabled = true;
                noteIndex++;
                timer = 0.0f;
            }

            if (noteIndex >= notes.Length)
            {
                turn = false;
                noteIndex = 0;
                timer = 0.0f;
                GameManager.instance.endingType = EndingType.Happy;
                GameManager.instance.LoadScene("05.EndingScene");
            }
        }
    }
}

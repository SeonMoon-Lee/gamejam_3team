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

    Image[] npcKeyImages;
    Image[] uiImages;
    Image[] npcImages;

    bool turn = false;
    int noteIndex = 0;
    KeyCode[] notes;
    Dictionary<KeyCode, int> noteMap;
    IEnumerator sequence;
    
    // Start is called before the first frame update
    void Start()
    {
        //SetupStage();

        var audioSources = GetComponents<AudioSource>();
        beatSource = audioSources[0];
        backgroundSource = audioSources[1];
        var npcKeyImageObj = GameObject.FindGameObjectsWithTag("key");
        Dictionary<String, int> keyMap_ = new Dictionary<string, int>();
        keyMap_.Add("WsoundImageActive", 0);
        keyMap_.Add("AsoundImageActive", 1);
        keyMap_.Add("SsoundImageActive", 2);
        keyMap_.Add("DsoundImageActive", 3);
        keyMap_.Add("WkeyImageActive", 0);
        keyMap_.Add("AkeyImageActive", 1);
        keyMap_.Add("SkeyImageActive", 2);
        keyMap_.Add("DkeyImageActive", 3);
        npcKeyImages = new Image[npcKeyImageObj.Length];
        for (int i = 0; i < npcKeyImageObj.Length; i++)
        {
            var image_ = npcKeyImageObj[i].GetComponent<Image>();
            var index = keyMap_[npcKeyImageObj[i].name];
            npcKeyImages[index] = image_;
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
        foreach (var i in npcKeyImages) i.enabled = false;
        var npcImagesObjs = GameObject.FindGameObjectsWithTag("NPC");
        npcImages = new Image[npcImagesObjs.Length];
        for (int i = 0; i < npcImagesObjs.Length; i++)
        {
            string msg = $"{npcImagesObjs[i].name}: {i}";
            Debug.Log(msg);
            npcImages[i] = npcImagesObjs[i].GetComponent<Image>();
            npcImages[i].enabled = false;
        }

        noteMap = new Dictionary<KeyCode, int>();
        noteMap.Add(KeyCode.W, 0);
        noteMap.Add(KeyCode.A, 1);
        noteMap.Add(KeyCode.S, 2);
        noteMap.Add(KeyCode.D, 3);

        backgroundSource.clip = clips[1];
        backgroundSource.volume = 0.7f;
        beatSource.clip = clips[0];
        sequence = TestSequence();
        StartCoroutine(sequence);
    }
    
    IEnumerator TestSequence()
    {
        var waitForSeconds = new WaitForSeconds(2.0f);
        var waitForSeconds2 = new WaitForSeconds(0.5f);
        
        backgroundSource.Play();
        yield return waitForSeconds;
        notes = new KeyCode[] {KeyCode.W, KeyCode.W, KeyCode.S, KeyCode.D, KeyCode.W, KeyCode.A, KeyCode.S, KeyCode.D};
        for (int i = 0; i < notes.Length; i++)
        {
            beatSource.clip = clips[0];
            beatSource.Play();
            npcKeyImages[noteMap[notes[i]]].enabled = true;
            yield return waitForSeconds2;
            npcKeyImages[noteMap[notes[i]]].enabled = false;
        }
        
        turn = true;
        while (turn)
        {
            yield return waitForSeconds2;
        }
        foreach (var i in uiImages) i.enabled = false;

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
                uiImages[noteMap[notes[noteIndex]]].enabled = true;
                noteIndex++;
                timer = 0.0f;
            }
            else
            {
                StartCoroutine(failure());
            }
            
            if (noteIndex >= notes.Length)
            {
                turn = false;
                noteIndex = 0;
                timer = 0.0f;
                
            }
        }
    }
    IEnumerator failure()
    {
        turn = false;
        StopCoroutine(sequence);
        backgroundSource.Stop();
        foreach (var npc in npcImages)
        {
            npc.enabled = true;
        }
        yield return new WaitForSeconds(5.0f);
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



    //참고
    private int stageIndex = 0;
    void SetupStage()
    {
        //0,1,2 스테이지 노트리스트를 가져온다.

        //해산물들을 스테이지에 따라 Active 해준다.

    }
    void NextStage()
    {
        GameManager.instance.SetStage(++stageIndex);
        SetupStage();
    }
    IEnumerator ReadyCount()
    {
        yield return null;
        //3
        yield return null;
        //2
        yield return null;
        //1

        //start
        StartCoroutine(Process());
    }
    IEnumerator Process()
    {
        yield return null;
        //NPC TURN
        yield return null;
        //1스테이지 일 경우 your 턴 표시
        //플레이어 턴

        //성공,실패 처리
        yield return null;
        //다음 턴이 있으면 다음턴 실행
        //없으면 스테이지 클리어 연출

        //마지막 스테이지일 경우 엔딩 씬 로드
    }
}

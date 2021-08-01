using System;
using System.Collections;
using System.Collections.Generic;
using UnityEditorInternal;
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
    Image[] failImages;
    Image[] npcImages;
    Image backgroundFailImage;
    Text yourTurnText;
    Image ruReadyImage;
    Image goImage;

    List<List<List<string>>> notes;
    Dictionary<KeyCode, int> noteMap;
    Dictionary<String, int> keyMap;
    Dictionary<string, KeyCode> keycodeMap;
    IEnumerator sequence;
    public GameObject Octopus, wasd, Lobster, space, Starfish, arrows;

    public GameObject StageClearPopup;
    public GameObject PausePopup;

    public TweenScale OctopusTween, LobsterTween, StarfishTween;
    // Start is called before the first frame update
    void Start()
    {
        SetInit();
        SetupStage();
        StartCoroutine(ReadyCount());
    }

    void SetInit()
    {
        keyMap = new Dictionary<string, int>
        {
            {"WsoundImageActive", 0},
            {"AsoundImageActive", 1},
            {"SsoundImageActive", 2},
            {"DsoundImageActive", 3},
            {"SpsoundImageActive", 4},
            {"UpsoundImageActive", 5},
            {"LeftsoundImageActive", 6},
            {"DownsoundImageActive", 7},
            {"RightsoundImageActive", 8},
            {"WkeyImageActive", 0},
            {"AkeyImageActive", 1},
            {"SkeyImageActive", 2},
            {"DkeyImageActive", 3},
            {"SpkeyImageActive", 4},
            {"UpkeyImageActive", 5},
            {"LeftkeyImageActive", 6},
            {"DownkeyImageActive", 7},
            {"RightkeyImageActive", 8},
            {"WkeyImageFail", 0},
            {"AkeyImageFail", 1},
            {"SkeyImageFail", 2},
            {"DkeyImageFail", 3},
            {"SpkeyImageFail", 4},
            {"UpkeyImageFail", 5},
            {"LeftkeyImageFail", 6},
            {"DownkeyImageFail", 7},
            {"RightkeyImageFail", 8}
        };
        noteMap = new Dictionary<KeyCode, int>
        {
            {KeyCode.W, 0}, {KeyCode.A, 1}, {KeyCode.S, 2}, {KeyCode.D, 3}, {KeyCode.Space, 4},
            {KeyCode.UpArrow, 5}, {KeyCode.LeftArrow, 6}, {KeyCode.DownArrow, 7}, {KeyCode.RightArrow, 8}
        };

        keycodeMap = new Dictionary<string, KeyCode>
        {
            {"W", KeyCode.W}, {"A", KeyCode.A}, {"S", KeyCode.S}, {"D", KeyCode.D}, {"space", KeyCode.Space},
            {"up", KeyCode.UpArrow}, {"left", KeyCode.LeftArrow}, {"down", KeyCode.DownArrow}, {"right", KeyCode.RightArrow}
        };

        Octopus = GameObject.Find("OctopusObject");
        wasd = GameObject.Find("wasd");
        Lobster = GameObject.Find("LobsterObject");
        space = GameObject.Find("space");
        Starfish = GameObject.Find("StarfishObject");
        arrows = GameObject.Find("arrows");

        var npcKeyImageObj = GameObject.FindGameObjectsWithTag("key");
        npcKeyImages = new Image[npcKeyImageObj.Length];
        foreach (var t in npcKeyImageObj)
        {
            var image = t.GetComponent<Image>();
            var index = keyMap[t.name];
            npcKeyImages[index] = image;
        }
        var uiImageObjs = GameObject.FindGameObjectsWithTag("uikey");
        uiImages = new Image[uiImageObjs.Length];
        foreach (var t in uiImageObjs)
        {
            var image = t.GetComponent<Image>();
            var index = keyMap[t.name];
            uiImages[index] = image;
        }
        var failImageObjs = GameObject.FindGameObjectsWithTag("failkey");
        failImages = new Image[failImageObjs.Length];
        foreach (var t in failImageObjs)
        {
            var image = t.GetComponent<Image>();
            var index = keyMap[t.name];
            failImages[index] = image;
        }
        var npcImagesObjs = GameObject.FindGameObjectsWithTag("NPC");
        npcImages = new Image[npcImagesObjs.Length];
        for (var i = 0; i < npcImagesObjs.Length; i++)
        {
            npcImages[i] = npcImagesObjs[i].GetComponent<Image>();
            npcImages[i].enabled = false;
        }
        foreach (var i in uiImages) i.enabled = false;
        foreach (var i in npcKeyImages) i.enabled = false;
        foreach (var i in failImages) i.enabled = false;

        var textObject = GameObject.Find("MessageObject");
        yourTurnText = textObject.GetComponentInChildren<Text>();
        yourTurnText.enabled = false;
        
        var plantImagesObj = GameObject.Find("background-fail");
        backgroundFailImage = plantImagesObj.GetComponent<Image>();
        backgroundFailImage.enabled = false;
        stageId = GameManager.instance.StageId;

        var readyGoObj = GameObject.Find("Ready");
        ruReadyImage = readyGoObj.GetComponent<Image>();
        ruReadyImage.enabled = false;
        goImage = readyGoObj.GetComponentsInChildren<Image>()[1];
        goImage.enabled = false;
    }

    IEnumerator Failure()
    {
        // turn = false;
        StopCoroutine(sequence);
        backgroundSource.Stop();
        backgroundFailImage.enabled = true;
        foreach (var npc in npcImages)
        {
            npc.enabled = true;
        }
        
        yield return new WaitForSeconds(5.0f);
        GameManager.instance.endingType = EndingType.Bad;
        GameManager.instance.LoadScene("05.EndingScene");
    }

    List<List<List<string>>> getNotes()
    {
        var notes = GameManager.instance.GetCurrentStage();
        int turn = 0, part = 0;
        List<List<List<string>>> data = new List<List<List<string>>>();
        List<List<string>> p_ = new List<List<string>>();
        List<string> k_ = new List<string>();
        p_.Add(k_);
        data.Add(p_);
        foreach (var note in notes)
        {
            if (note.turn - 1 > turn)
            {
                p_ = new List<List<string>>();
                k_ = new List<string> {note.key};
                p_.Add(k_);
                data.Add(p_);
                turn++;
            }
            else if (note.part - 1 > part)
            {
                k_ = new List<string> {note.key};
                p_.Add(k_);
                part++;
            }
            else
            {
                k_.Add(note.key);
            }
        }
        return data;
    }

    //참고
    private int stageId;
    void SetupStage()
    {
        //0,1,2 스테이지 노트리스트를 가져온다.
        notes = getNotes();

        //해산물들을 스테이지에 따라 Active 해준다.
        AudioSource[] audioSources;
        switch (stageId)
        {
            case 1:
                Octopus.SetActive(true);
                Octopus.transform.position += new Vector3(300, 0.0f, 0.0f);
                wasd.SetActive(true);
                wasd.transform.position += new Vector3(300, 0.0f, 0.0f);
                Lobster.SetActive(false);
                space.SetActive(false);
                Starfish.SetActive(false);
                arrows.SetActive(false);

                // 음악 세팅
                audioSources = GetComponents<AudioSource>();
                beatSource = audioSources[0];
                backgroundSource = audioSources[1];
                backgroundSource.clip = clips[1];
                backgroundSource.volume = 0.7f;
                beatSource.clip = clips[0];
                break;
            case 2:
                Octopus.SetActive(true);
                Octopus.transform.position += new Vector3(100, 0.0f, 0.0f);
                wasd.SetActive(true);
                wasd.transform.position += new Vector3(100, 0.0f, 0.0f);
                Lobster.SetActive(true);
                Lobster.transform.position += new Vector3(200, 0.0f, 0.0f);
                space.SetActive(true);
                space.transform.position += new Vector3(200, 0.0f, 0.0f);
                Starfish.SetActive(false);
                arrows.SetActive(false);

                // 음악 세팅
                audioSources = GetComponents<AudioSource>();
                beatSource = audioSources[0];
                backgroundSource = audioSources[1];
                backgroundSource.clip = clips[1];
                backgroundSource.volume = 0.7f;
                beatSource.clip = clips[0];
                break;
            case 3:
                Octopus.SetActive(true);
                wasd.SetActive(true);
                Lobster.SetActive(true);
                space.SetActive(true);
                Starfish.SetActive(true);
                arrows.SetActive(true);

                // 음악 세팅
                audioSources = GetComponents<AudioSource>();
                beatSource = audioSources[0];
                backgroundSource = audioSources[1];
                backgroundSource.clip = clips[1];
                backgroundSource.volume = 0.7f;
                beatSource.clip = clips[0];
                break;
        }

    }
    public void NextStage()
    {
        GameManager.instance.StopBgm();
        GameManager.instance.SetStage(++stageId);
        GameManager.instance.LoadScene("02.IngameScene");
    }
    IEnumerator ReadyCount()
    {
        var waitReadyGo = new WaitForSecondsRealtime(1.0f);
        ruReadyImage.enabled = true;
        yield return waitReadyGo;
        ruReadyImage.enabled = false;
        goImage.enabled = true;
        yield return waitReadyGo;
        goImage.enabled = false;

        //start
        sequence = Process();
        StartCoroutine(sequence);
    }

    KeyCode prevCode = KeyCode.Z;
    bool CheckCode()
    {
        if (Input.GetKeyDown(KeyCode.A))
        {
            prevCode = KeyCode.A;
            return true;
        } else if (Input.GetKeyDown(KeyCode.W))
        {
            prevCode = KeyCode.W;
            return true;
        } else if (Input.GetKeyDown(KeyCode.S))
        {
            prevCode = KeyCode.S;
            return true;
        } else if (Input.GetKeyDown(KeyCode.D))
        {
            prevCode = KeyCode.D;
            return true;
        } else if (Input.GetKeyDown(KeyCode.Space))
        {
            prevCode = KeyCode.Space;
            return true;
        } else if (Input.GetKeyDown(KeyCode.UpArrow))
        {
            prevCode = KeyCode.UpArrow;
            return true;
        } else if (Input.GetKeyDown(KeyCode.LeftArrow))
        {
            prevCode = KeyCode.LeftArrow;
            return true;
        } else if (Input.GetKeyDown(KeyCode.DownArrow))
        {
            prevCode = KeyCode.DownArrow;
            return true;
        } else if (Input.GetKeyDown(KeyCode.RightArrow))
        {
            prevCode = KeyCode.RightArrow;
            return true;
        }
        return false;
    }
    
    IEnumerator Process()
    {
        var waitForSeconds = new WaitForSeconds(2.0f);
        var waitForPart = new WaitForSeconds(0.75f);
        var waitForSeconds2 = new WaitForSeconds(0.7f);
        var waitForSeconds3 = new WaitForSeconds(0.05f);

        backgroundSource.Play();
        yield return waitForSeconds;

        foreach (var turn in notes)
        {
            var combo = new List<KeyCode>();
            foreach (var part in turn)
            {
                foreach (var note in part)
                {
                    var note_ = keycodeMap[note];
                    combo.Add(note_);
                    // 노트에 따라 음이 달라지면 클립도 변경해야한다.
                    beatSource.clip = clips[0];
                    beatSource.Play();
                    npcKeyImages[noteMap[note_]].enabled = true;
                    // 스테이지 1 튜토리얼 격으로 표시해줌
                    if(stageId == 1)
                        uiImages[noteMap[note_]].enabled = true;
                    yield return waitForSeconds2;
                    npcKeyImages[noteMap[note_]].enabled = false;
                    if(stageId == 1)
                        uiImages[noteMap[note_]].enabled = false;
                    yield return waitForSeconds3;
                }
                yield return waitForPart;
            }
            // your turn!
            yourTurnText.enabled = true;
            yield return waitForSeconds3;

            foreach (var note in combo)
            {
                yield return new WaitUntil(CheckCode);
                if (Input.GetKeyDown(note))
                {
                    beatSource.Play();
                    uiImages[noteMap[note]].enabled = true;
                    yield return waitForSeconds3;
                    uiImages[noteMap[note]].enabled = false;
                }
                else
                {
                    failImages[noteMap[prevCode]].enabled = true;
                    StartCoroutine(Failure());
                }
            }
            yourTurnText.enabled = false;
            yield return waitForSeconds2;
        }
        yield return null;
        //NPC TURN
        yield return null;
        //1스테이지 일 경우 your 턴 표시
        //플레이어 턴

        //성공,실패 처리
        yield return null;
        if (stageId <= 3)
        {
            //NextStage(); 
            StageClearPopup.SetActive(true);
            GameManager.instance.SetBgm("track4-loop");
        }
        else
        {
            GameManager.instance.endingType = EndingType.Happy;
            GameManager.instance.LoadScene("05.EndingScene");
        }
        //다음 턴이 있으면 다음턴 실행
        //없으면 스테이지 클리어 연출

        //마지막 스테이지일 경우 엔딩 씬 로드
    }

    public void OnClickContinue()
    {
        PausePopup.SetActive(false);
        StartCoroutine(Continue());
    }
    IEnumerator Continue()
    {
        yield return new WaitForSecondsRealtime(1f);

        Debug.Log("Ready");
        yield return new WaitForSecondsRealtime(1f);

        Debug.Log("go");
        Time.timeScale = 1;
    }
    public void OnClickTitle()
    {
        PausePopup.transform.Find("MainPopup").gameObject.SetActive(true);
    }
    public void GoTitle()
    {
        GameManager.instance.LoadScene("01.StartScene");
    }
    public void OnClickQuit()
    {
        PausePopup.transform.Find("ExitPopup").gameObject.SetActive(true);
    }
    public void Quit()
    {
        Application.Quit();
    }
    public void ClosePopup(GameObject gobj)
    { 
        gobj.SetActive(false); 
    }
}

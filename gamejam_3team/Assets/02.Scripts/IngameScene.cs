using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Security.Cryptography;
using TreeEditor;
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
    Text yourTurn;

    bool turn = false;
    int noteIndex = 0;
    // KeyCode[] notes;
    List<List<List<string>>> notes;
    Dictionary<KeyCode, int> noteMap;
    Dictionary<String, int> keyMap;
    Dictionary<string, KeyCode> keycodeMap;
    IEnumerator sequence;
    
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
            {"RightkeyImageActive", 8}
        };
        noteMap = new Dictionary<KeyCode, int>
        {
            {KeyCode.W, 0}, {KeyCode.A, 1}, {KeyCode.S, 2}, {KeyCode.D, 3}, {KeyCode.Space, 4},
            {KeyCode.UpArrow, 5}, {KeyCode.LeftArrow, 6}, {KeyCode.DownArrow, 7}, {KeyCode.RightArrow, 8}
        };

        keycodeMap = new Dictionary<string, KeyCode>
        {
            {"W", KeyCode.W}, {"A", KeyCode.A}, {"S", KeyCode.S}, {"D", KeyCode.D}, {"space", KeyCode.Space},
            {"��", KeyCode.UpArrow}, {"��", KeyCode.LeftArrow}, {"��", KeyCode.DownArrow}, {"��", KeyCode.RightArrow}
        };
    }
    
    /*
    IEnumerator TestSequence()
    {
        var waitForSeconds = new WaitForSeconds(2.0f);
        var waitForSeconds2 = new WaitForSeconds(0.95f);
        var waitForSeconds3 = new WaitForSeconds(0.05f);
        
        backgroundSource.Play();
        yield return waitForSeconds;
        
        foreach (var t in notes)
        {
            // ��Ʈ�� ���� ���� �޶����� Ŭ���� �����ؾ��Ѵ�.
            beatSource.clip = clips[0];
            beatSource.Play();
            npcKeyImages[noteMap[t]].enabled = true;
            yield return waitForSeconds2;
            npcKeyImages[noteMap[t]].enabled = false;
            yield return waitForSeconds3;
        }
        
        turn = true;
        while (turn)
        {
            yield return waitForSeconds2;
        }
        foreach (var i in uiImages) i.enabled = false;

        GameManager.instance.endingType = EndingType.Happy;
        GameManager.instance.LoadScene("05.EndingScene");
    }*/

    /*
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
    }*/
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
            // playerKeyCombo(notes);
        }
    }

    List<List<List<string>>> getNotes(int stage)
    {
        var notes_ = GameManager.instance.GetCurrentStage();
        int turn = 0, part = 0;
        List<List<List<string>>> data = new List<List<List<string>>>();
        List<List<string>> p_ = new List<List<string>>();
        List<string> k_ = new List<string>();
        p_.Add(k_);
        data.Add(p_);
        foreach (var note in notes_)
        {
            if (note.turn - 1 > turn)
            {
                p_ = new List<List<string>>();
                k_ = new List<string>();
                k_.Add(note.key);
                p_.Add(k_);
                data.Add(p_);
                turn++;
            } else if (note.part - 1 > part)
            {
                k_ = new List<string>();
                k_.Add(note.key);
                p_.Add(k_);
                part++;
            }
            else
            {
                k_.Add(note.key);
            }
            
        }
        
        List<KeyCode[]> notes = new List<KeyCode[]>
        {
            new KeyCode[] {KeyCode.W, KeyCode.W, KeyCode.S, KeyCode.D, KeyCode.W, KeyCode.A, KeyCode.S, KeyCode.D},
            new KeyCode[] {KeyCode.W, KeyCode.Space, KeyCode.S, KeyCode.D, KeyCode.Space, KeyCode.A, KeyCode.S, KeyCode.D}
        };
        
        // return notes[stage];
        return data;
    }

    //����
    private int stageIndex;
    void SetupStage()
    {
        // stageIndex = GameManager.instance.StageIndex;
        stageIndex = 0;
        
        //0,1,2 �������� ��Ʈ����Ʈ�� �����´�.
        notes = getNotes(stageIndex);

        //�ػ깰���� ���������� ���� Active ���ش�.
        var Octopus = GameObject.Find("OctopusObject");
        var wasd = GameObject.Find("wasd");
        var Lobster = GameObject.Find("LobsterObject");
        var space = GameObject.Find("space");
        var Starfish = GameObject.Find("StarfishObject");
        var arrows = GameObject.Find("arrows");
        AudioSource[] audioSources;
        switch (stageIndex)
        {
            case 0:
                Octopus.SetActive(true);
                Octopus.transform.position += new Vector3(300, 0.0f, 0.0f);
                wasd.SetActive(true);
                wasd.transform.position += new Vector3(300, 0.0f, 0.0f);
                Lobster.SetActive(false);
                space.SetActive(false);
                Starfish.SetActive(false);
                arrows.SetActive(false);
                
                // ���� ����
                audioSources = GetComponents<AudioSource>();
                beatSource = audioSources[0];
                backgroundSource = audioSources[1];
                backgroundSource.clip = clips[1];
                backgroundSource.volume = 0.7f;
                beatSource.clip = clips[0];
                break;
            case 1:
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
                
                // ���� ����
                audioSources = GetComponents<AudioSource>();
                beatSource = audioSources[0];
                backgroundSource = audioSources[1];
                backgroundSource.clip = clips[1];
                backgroundSource.volume = 0.7f;
                beatSource.clip = clips[0];
                break;
            case 2:
                Octopus.SetActive(true);
                wasd.SetActive(true);
                Lobster.SetActive(true);
                space.SetActive(true);
                Starfish.SetActive(true);
                arrows.SetActive(true);
                
                // ���� ����
                audioSources = GetComponents<AudioSource>();
                beatSource = audioSources[0];
                backgroundSource = audioSources[1];
                backgroundSource.clip = clips[1];
                backgroundSource.volume = 0.7f;
                beatSource.clip = clips[0];
                break;
        }
        var npcKeyImageObj = GameObject.FindGameObjectsWithTag("key");
        npcKeyImages = new Image[npcKeyImageObj.Length];
        for (int i = 0; i < npcKeyImageObj.Length; i++)
        {
            var image_ = npcKeyImageObj[i].GetComponent<Image>();
            var index = keyMap[npcKeyImageObj[i].name];
            npcKeyImages[index] = image_;
        }
        uiImages = UICanvas.GetComponentsInChildren<Image>();
        var uiImageObjs = GameObject.FindGameObjectsWithTag("uikey");
        uiImages = new Image[uiImageObjs.Length];
        for (int i = 0; i < uiImageObjs.Length; i++)
        {
            var image_ = uiImageObjs[i].GetComponent<Image>();
            var index = keyMap[uiImageObjs[i].name];
            uiImages[index] = image_;
        }
        var npcImagesObjs = GameObject.FindGameObjectsWithTag("NPC");
        npcImages = new Image[npcImagesObjs.Length];
        for (int i = 0; i < npcImagesObjs.Length; i++)
        {
            string msg = $"{npcImagesObjs[i].name}: {i}";
            Debug.Log(msg);
            npcImages[i] = npcImagesObjs[i].GetComponent<Image>();
            npcImages[i].enabled = false;
        }
        foreach (var i in uiImages) i.enabled = false;
        foreach (var i in npcKeyImages) i.enabled = false;

        var textObject = GameObject.Find("MessageObject");
        yourTurn = textObject.GetComponentInChildren<Text>();
        yourTurn.enabled = false;
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
        // sequence = TestSequence();
        // StartCoroutine(sequence);
        sequence = Process();
        StartCoroutine(sequence);
    }
    IEnumerator Process()
    {
        var waitForSeconds = new WaitForSeconds(2.0f);
        var waitForPart = new WaitForSeconds(1.5f);
        var waitForSeconds2 = new WaitForSeconds(0.95f);
        var waitForSeconds3 = new WaitForSeconds(0.05f);
        
        backgroundSource.Play();
        yield return waitForSeconds;
        
        foreach (var turn in notes)
        {
            var combo = new List<KeyCode>();
            int noteIndex = 0;
            foreach (var part in turn)
            {
                KeyCode prevCode;
                foreach (var note in part)
                {
                    var note_ = keycodeMap[note];
                    combo.Add(note_);
                    // ��Ʈ�� ���� ���� �޶����� Ŭ���� �����ؾ��Ѵ�.
                    beatSource.clip = clips[0];
                    beatSource.Play();
                    prevCode = note_;
                    npcKeyImages[noteMap[note_]].enabled = true;
                    yield return waitForSeconds2;
                    npcKeyImages[noteMap[note_]].enabled = false;
                    yield return waitForSeconds3;
                }
                yield return waitForPart;   
            }
            // your turn!
            yourTurn.enabled = true;
            yield return waitForSeconds3;
            
            foreach (var note in combo)
            {
                yield return new WaitUntil(() => Input.anyKeyDown);
                if (Input.GetKeyDown(note))
                {
                    beatSource.Play();
                    uiImages[noteMap[note]].enabled = true;
                    //yield return waitForSeconds2;
                    yield return waitForSeconds3;
                    uiImages[noteMap[note]].enabled = false;
                }
                else
                {
                    StartCoroutine(failure());
                }
            }
            yourTurn.enabled = false;
            yield return waitForSeconds2;
        }
        yield return null;
        //NPC TURN
        yield return null;
        //1�������� �� ��� your �� ǥ��
        //�÷��̾� ��
        
        //����,���� ó��
        yield return null;
        if (stageIndex < 1)
        {
            NextStage();            
        }
        else
        {
            GameManager.instance.endingType = EndingType.Happy;
            GameManager.instance.LoadScene("05.EndingScene");
        }
        //���� ���� ������ ������ ����
        //������ �������� Ŭ���� ����

        //������ ���������� ��� ���� �� �ε�
    }
}

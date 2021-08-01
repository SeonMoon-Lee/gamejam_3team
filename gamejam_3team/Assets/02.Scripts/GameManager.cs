using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.Networking;
public enum EndingType
{
    Bad,
    Happy,
}
public class GameManager : MonoBehaviour
{
    public static GameManager instance = null;

    public EndingType endingType = EndingType.Bad;
    public int StageId = 1;
    public FadeInOut FadeImage;
    public AudioSource bgm;
    public AudioSource buttonSound;
    public List<AudioClip> clips;
    private Dictionary<int, List<NoteData>> stageMap = new Dictionary<int, List<NoteData>>();
    private void Awake()
    {
        if (instance == null)
            instance = this;
        else
        { 
            Destroy(this.gameObject);
            return;
        }
        DontDestroyOnLoad(this);

    }
    // Start is called before the first frame update
    void Start()
    {
        StartCoroutine(ConnectGoogleSheet("https://docs.google.com/spreadsheets/d/1qCImmI0rqJFvqHx260oQxOcp8iZcIwGoivc-Pe3HK4s/export?format=csv"));
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void InitValues()
    {
        StageId = 1;
        endingType = EndingType.Bad;
    }
    public void InitDatas()
    {
        //���������� ��Ʈ �ʱ�ȭ�� ���⼭ �ϰ� Dic�� ��Ƶδ°� ���?
        //List<NoteData>�� ��Ƶθ� ������.
        //NoteData �� List<StepData>�� ��´�.
        //StepData �� startTime, expireTime, List<KeyCode>�� ����� ������ ���?


    }
    public void SetBgm(string name)
    {
        bgm.clip = clips.Find(_ => _.name == name);
        bgm.Play();
    }
    public void StopBgm()
    {
        bgm.Stop();
    }
    public void PlayButtonSound()
    {
        buttonSound.Play();
    }
    public void LoadScene(string name)
    {
        //FadeImage.curColor = Color.clear;
        //FadeImage.targetColor = Color.black;
        //FadeImage.Play();
        StartCoroutine(LoadSceneAsync(name));
    }
    IEnumerator LoadSceneAsync(string name)
    {
        var request = UnityEngine.SceneManagement.SceneManager.LoadSceneAsync(name);
        yield return request;

        FadeImage.targetColor = Color.clear;
        FadeImage.curColor = Color.black;
        FadeImage.Play();
    }
    public void SetStage(int idx) { StageId = idx; }
    //NoteData�� ��ȯ�ϴ°� ����Ʈ?
    public List<NoteData> GetCurrentStage() { return stageMap[StageId]; }

    IEnumerator ConnectGoogleSheet(string url)
    {
        var request = UnityWebRequest.Get(url);
        yield return request.SendWebRequest();
        if (string.IsNullOrEmpty(request.error))
        {
            string data = request.downloadHandler.text;

            Debug.Log(data);
            NoteData[] noteDatas = CSVSerializer.Deserialize<NoteData>(data);

            List<NoteData> notelist = new List<NoteData>(noteDatas);

            for(int i =1; i <=3; ++i)
            {
                stageMap.Add(i,notelist.Where(_ => _.idx == i).ToList());
            }
        }
        LoadScene("01.StartScene");
    }

}
public class NoteData
{
    //idx	turn	part	npc_number	key
    public int idx;
    public int turn;
    public int part;
    public int npc_number;
    public string key;
}
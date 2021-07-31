using System.Collections;
using System.Collections.Generic;
using UnityEngine;
public enum EndingType
{
    Bad,
    Happy,
}
public class GameManager : MonoBehaviour
{
    public static GameManager instance = null;

    public EndingType endingType = EndingType.Bad;
    public int StageIndex = 0;
    public FadeInOut FadeImage;
    public AudioSource bgm;
    public List<AudioClip> clips;
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
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void InitValues()
    {
        StageIndex = 0;
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
    public void LoadScene(string name)
    {
        FadeImage.curColor = Color.clear;
        FadeImage.targetColor = Color.black;
        FadeImage.Play();
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
    public void SetStage(int idx) { StageIndex = idx; }
    //NoteData�� ��ȯ�ϴ°� ����Ʈ?
    public int GetCurrentStage() { return StageIndex; }

}

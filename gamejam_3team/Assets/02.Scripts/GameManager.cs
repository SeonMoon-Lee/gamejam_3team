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
        //스테이지별 노트 초기화를 여기서 하고 Dic에 담아두는건 어떨까?
        //List<NoteData>로 담아두면 좋을듯.
        //NoteData 는 List<StepData>를 담는다.
        //StepData 는 startTime, expireTime, List<KeyCode>가 담겨져 있으면 어떨까?


    }
    public void LoadScene(string name)
    {
        UnityEngine.SceneManagement.SceneManager.LoadScene(name);
    }

    public void SetStage(int idx) { StageIndex = idx; }
    //NoteData를 반환하는게 베스트?
    public int GetCurrentStage() { return StageIndex; }

}

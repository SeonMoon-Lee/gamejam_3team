using System.Collections;
using System.Collections.Generic;
using UnityEngine;
public enum EndingType
{
    Bad,
    Happy,
}
public class EndingScene : MonoBehaviour
{
    public GameObject BadEnding;
    public GameObject HappyEnding;

    public GameObject ButtonGroup;
    // Start is called before the first frame update
    IEnumerator Start()
    {
        if (GameManager.instance.endingType == EndingType.Bad)
            BadEnding.SetActive(true);
        else
            HappyEnding.SetActive(true);

        yield return new WaitForSeconds(3f);

        ButtonGroup.SetActive(true);
    }


    public void OnClickRetry()
    {
        GameManager.instance.LoadScene("02.IngameScene");
    }
    public void OnClickTitle()
    {
        GameManager.instance.LoadScene("01.StartScene");
    }
    
}

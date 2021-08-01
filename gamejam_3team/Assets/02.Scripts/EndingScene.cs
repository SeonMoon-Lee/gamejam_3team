using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class EndingScene : MonoBehaviour
{

    public GameObject BadEnding;
    public Sprite[] badBackgrounds;
    public Image badImage;
    public GameObject HappyEnding;

    public GameObject Credit;

    public GameObject ButtonGroup;
    // Start is called before the first frame update
    IEnumerator Start()
    {
        if (GameManager.instance.endingType == EndingType.Bad)
        {
            badImage.sprite = badBackgrounds[GameManager.instance.StageId-1];
            BadEnding.SetActive(true);
            yield return new WaitForSeconds(3f);

        }
        else
        {
            HappyEnding.SetActive(true);
            yield return new WaitForSeconds(1.0f);
            GameManager.instance.SetBgm("track4-loop");
            yield return new WaitForSeconds(5f);
            GameManager.instance.StopBgm();
            Credit.SetActive(true);
        }

        ButtonGroup.SetActive(true);
    }


    public void OnClickRetry()
    {
        GameManager.instance.PlayButtonSound();
        GameManager.instance.LoadScene("02.IngameScene");
    }
    public void OnClickTitle()
    {
        GameManager.instance.PlayButtonSound();
        GameManager.instance.LoadScene("01.StartScene");
    }
    
}

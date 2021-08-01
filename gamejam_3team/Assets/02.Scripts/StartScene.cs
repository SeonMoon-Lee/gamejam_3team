using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class StartScene : MonoBehaviour
{
    private void Start()
    {
        GameManager.instance.SetBgm("track01");
    }
    public void OnClickStart()
    {
        GameManager.instance.PlayButtonSound();
        GameManager.instance.LoadScene("03.StoryScene");
    }
}

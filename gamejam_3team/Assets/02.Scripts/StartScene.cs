using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class StartScene : MonoBehaviour
{
    public void OnClickStart()
    {
        GameManager.instance.LoadScene("03.StoryScene");
    }
}

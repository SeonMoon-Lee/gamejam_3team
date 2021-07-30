using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HelpScene : MonoBehaviour
{
    // Start is called before the first frame update
    IEnumerator Start()
    {
        yield return new WaitForSeconds(5f);

        //GameManager.instance.LoadScene("02.IngameScene");
        GameManager.instance.endingType = EndingType.Happy;
        GameManager.instance.LoadScene("05.EndingScene");
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}

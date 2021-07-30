using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class StoryScene : MonoBehaviour
{
    // Start is called before the first frame update
    public GameObject[] stories;
    void Start()
    {
        //stories = StoryListObject.transform.get<GameObject>();
        for (int i = 0; i < stories.Length; ++i)
            stories[i].SetActive(false);

        StartCoroutine(StoryUpdate());
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    IEnumerator StoryUpdate()
    {
        for (int i = 0; i < stories.Length; ++i)
        {
            yield return new WaitForSeconds(1f);
            stories[i].SetActive(true);
        }
        yield return new WaitForSeconds(3f);

        GameManager.instance.LoadScene("04.HelpScene");
    }
}

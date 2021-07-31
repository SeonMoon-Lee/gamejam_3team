using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
public class FadeInOut : MonoBehaviour
{
    public MaskableGraphic target;
    // Start is called before the first frame update
    public Color curColor;
    public Color targetColor;
    public float duration = 1;
    public bool isPlayOnAwake = true;
    void Start()
    {
        if(isPlayOnAwake)
            StartCoroutine(Fade());
    }

    private IEnumerator Fade()
    {
        float time = 0f;
        while(time<1)
        {
            target.color = Color.Lerp(curColor, targetColor, time / 1f);
            time += Time.deltaTime/duration;
            yield return null;

        }
        yield return null;

    }
    public void Play()
    {
        StartCoroutine(Fade());
    }
}

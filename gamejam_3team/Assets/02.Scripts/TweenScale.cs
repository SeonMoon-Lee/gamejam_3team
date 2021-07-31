using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TweenScale : MonoBehaviour
{
    public Transform target;

    public Vector3 from;
    public Vector3 to;
    public float duration = 1;
    public float delay = 0;
    public bool isPlayOnAwake = true;

    // Start is called before the first frame update
    void Start()
    {
        if (isPlayOnAwake)
            StartCoroutine(Tween());
    }
    public void Play()
    {
        StartCoroutine(Tween());
    }
    IEnumerator Tween()
    {
        float time = 0;
        target.localScale = from;
        yield return new WaitForSeconds(delay);
        while (time < 1)
        {
            target.localScale = Vector3.Lerp(from, to, time / 1f);
            time += Time.deltaTime / duration;
            yield return null;
        }
    }

}

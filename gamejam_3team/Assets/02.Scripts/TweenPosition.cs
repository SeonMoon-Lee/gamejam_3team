using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TweenPosition : MonoBehaviour
{
    public Transform target;

    public Vector3 from;
    public Vector3 to;
    public float duration = 1;
    public float delay = 0;
    // Start is called before the first frame update
    IEnumerator Start()
    {
        float time = 0;
        target.localPosition = from;
        yield return new WaitForSeconds(delay);
        while (time < 1)
        {
            target.localPosition = Vector3.Lerp(from, to, time / 1f);
            time += Time.deltaTime / duration;
            yield return null;
        }
    }
}

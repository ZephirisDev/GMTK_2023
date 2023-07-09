using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Untilgone : MonoBehaviour
{
    private void Awake()
    {
        StartCoroutine(disappear());
    }

    IEnumerator disappear()
    {
        var c = GetComponent<CanvasGroup>();
        float x = 0;
        while(c.alpha > 0)
        {
            c.alpha -= Time.deltaTime;
            x += Time.deltaTime;
            if (x > 3)
                c.alpha = -1;
            yield return null;
        }
    }
}

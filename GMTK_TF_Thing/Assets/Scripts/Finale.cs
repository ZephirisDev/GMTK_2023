using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Finale : MonoBehaviour
{
    [SerializeField] Transform badger;

    public void GameOver(bool success)
    {
        if (!success)
        {
            StartCoroutine(WalkTo(badger.transform.position + new Vector3(0, 0, -10)));
        }
    }

    IEnumerator WalkTo(Vector3 pos)
    {
        var dif = pos - badger.transform.position;
        int steps = 16;
        float totalTime = 3f;

        var smallDif = new Vector2(dif.x / (float)steps, dif.y / (float)steps);

        float progress = 0f;

        float p = 1 / totalTime;

        while (progress < 1f)
        {

            badger.transform.position += new Vector3(dif.x, 0, dif.z) * Time.deltaTime * p;
            progress += Time.deltaTime * p;

            yield return null;

        }

        badger.transform.position = new Vector3(pos.x, 0.5f, pos.z);
    }
}

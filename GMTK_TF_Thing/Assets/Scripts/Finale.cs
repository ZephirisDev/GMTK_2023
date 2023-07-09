using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class Finale : MonoBehaviour
{
    [SerializeField] Transform badger;
    [SerializeField] GameObject goodEnding;
    [SerializeField] GameObject badEnding;

    public void GameOver(bool success)
    {
        if (!success)
        {
            StartCoroutine(WalkTo(badger.transform.position + new Vector3(0, 0, -10)));
            badEnding.SetActive(true);
        }
        else
        {
            goodEnding.SetActive(true);
        }
        StartCoroutine(Enddddd());
        FindObjectOfType<GeneralController>().enabled = false;
    }

    IEnumerator Enddddd()
    {
        var c = FindObjectOfType<music>().GetComponent<AudioSource>();
        while(c.volume > 0)
        {
            c.volume -= Time.deltaTime * 0.02f;
            yield return null;
        }
        yield return new WaitForSeconds(3);
        SceneManager.LoadScene(0);
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

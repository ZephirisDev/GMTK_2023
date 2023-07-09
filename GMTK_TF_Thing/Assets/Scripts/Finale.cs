using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class Finale : MonoBehaviour
{
    [SerializeField] Transform badger;
    [SerializeField] GameObject goodEnding;
    [SerializeField] GameObject badEnding;

    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.Z))
            GameOver(true);
    }

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
        StartCoroutine(Enddddd(success));
        foreach(var n in FindObjectsOfType<GeneralController>())
            n.enabled = false;
    }

    IEnumerator Enddddd(bool scc)
    {
        yield return new WaitForSeconds(1.5f);
        AudioHandler.TryPlaySound(scc ? SoundIdentifier.Nom : SoundIdentifier.Sigh);
        yield return new WaitForSeconds(2f);
        if (scc)
            AudioHandler.TryPlaySound(SoundIdentifier.Wink);
        var c = FindObjectOfType<music>().GetComponent<AudioSource>();
        while(c.volume > 0)
        {
            c.volume -= Time.deltaTime * 0.06f;
            yield return null;
        }
        yield return new WaitForSeconds(1f);
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

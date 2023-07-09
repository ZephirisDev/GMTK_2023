using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class IntroHandler : MonoBehaviour
{
    [SerializeField] FrontController front;
    [SerializeField] CanvasGroup blackScreen;
    [SerializeField] Animator anima;
    [SerializeField] Animator cameraAnima;
    [SerializeField] Animator shadowAnima;
    [SerializeField] Transform badger;
    [SerializeField] List<GameObject> killa;
    private bool hasStarted;

    private void Awake()
    {
        front.enabled = false;
        shadowAnima.enabled = false;
        StartCoroutine(BlackScreen());
    }

    private void Update()
    {
        if (hasStarted) return;

        if(Input.GetKeyDown(KeyCode.A) || Input.GetKeyDown(KeyCode.D) || Input.GetKeyDown(KeyCode.W) || Input.GetKeyDown(KeyCode.E) ||
            Input.GetKeyDown(KeyCode.Space) || Input.GetKeyDown(KeyCode.Q) || Input.GetKeyDown(KeyCode.Return) || Input.GetKeyDown(KeyCode.S))
        {
            hasStarted = true;
            StartCoroutine(WalkTo(new Vector3(0, 0, -2)));
            StartCoroutine(Intro());
        }
    }

    IEnumerator BlackScreen()
    {
        while (blackScreen.alpha > 0)
        {
            blackScreen.alpha -= Time.deltaTime;
            yield return null;
        }
    }

    IEnumerator WalkTo(Vector3 pos)
    {
        var dif = pos - badger.transform.localPosition;
        int steps = 16;
        float totalTime = 2f;

        var smallDif = new Vector2(dif.x / (float)steps, dif.y / (float)steps);

        float progress = 0f;

        float p = 1 / totalTime;

        while (progress < 1f)
        {

            badger.transform.localPosition += new Vector3(dif.x, 0, dif.z) * Time.deltaTime * p;
            progress += Time.deltaTime * p;

            yield return null;

        }

        badger.transform.localPosition = new Vector3(pos.x, 0.5f, pos.z);
    }

    IEnumerator Intro()
    {
        yield return new WaitForSeconds(1.5f);
        anima.SetTrigger("surprise");
        AudioHandler.TryPlaySound(SoundIdentifier.Hurt);
        float s = 0;
        var pos = anima.transform.position;
        while (s < 1)
        {
            anima.transform.position += new Vector3(0, Time.deltaTime * 0.8f, 0);
            s += Time.deltaTime * 4;
            yield return null;
        }
        s = 0;
        while (s < 1)
        {
            anima.transform.position -= new Vector3(0, Time.deltaTime * 1.2f, 0);
            s += Time.deltaTime * 6;
            yield return null;
        }
        anima.transform.position = pos;
        anima.SetTrigger("normal");
        cameraAnima.SetTrigger("Start");
        shadowAnima.enabled = true;
        front.enabled = true;
        yield return new WaitForSeconds(5f);
        foreach(var n in killa)
        {
            Destroy(n.gameObject);
        }
    }
}

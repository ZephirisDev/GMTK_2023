using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TFHandler : MonoBehaviour
{
    [SerializeField] Animator camAnimator;
    [SerializeField] Animator badger;
    [SerializeField] Animator player;
    [SerializeField] CanvasGroup tfThingy;
    private FrontController preyController;
    private BackController predatorController;

    private void Awake()
    {
        preyController = GetComponent<FrontController>();
        predatorController = GetComponent<BackController>();
        predatorController.enabled = false;

        preyController.OnDamage += Damage;
        preyController.OnDeath += TurnToPredator;
    }

    private void Damage()
    {
        camAnimator.SetTrigger("Hurt");
        StartCoroutine(GetBadgerClose());
    }

    private IEnumerator GetBadgerClose()
    {
        float totalDistance = 0.5f;
        while(totalDistance > 0)
        {
            badger.transform.localPosition += new Vector3(0, 0, 1f * Time.deltaTime);
            totalDistance -= 1f * Time.deltaTime;
            yield return null;
        }
    }

    private IEnumerator GetBadgerAway()
    {
        float totalDistance = -3f;
        while (totalDistance < 0)
        {
            badger.transform.localPosition += new Vector3(0, 0, -1f * Time.deltaTime);
            totalDistance += 1f * Time.deltaTime;
            yield return null;
        }
    }

    private void TurnToPredator()
    {
        preyController.enabled = false;
        StartCoroutine(TFSequence());
    }

    IEnumerator TFSequence()
    {
        // Time til animation be done
        FindObjectOfType<music>().PlaySong(1);
        tfThingy.gameObject.SetActive(true);
        yield return new WaitForSeconds(2f);
        camAnimator.SetTrigger("TFTrigger");
        badger.SetTrigger("TF");
        player.SetTrigger("TF");
        AudioHandler.TryPlaySound(SoundIdentifier.Grrr);
        badger.GetComponent<BadgerHandler>().EndIt();
        yield return new WaitForSeconds(1f);
        predatorController.enabled = true;
        while(tfThingy.alpha > 0)
        {
            tfThingy.alpha -= Time.deltaTime * 3;
            yield return null;
        }
        tfThingy.gameObject.SetActive(false);
        FindObjectOfType<music>().PlaySong(2);
        yield return new WaitForSeconds(2f);
        badger.GetComponent<BadgerHandler>().SetVuln();
    }
}

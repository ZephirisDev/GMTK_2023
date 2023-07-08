using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TFHandler : MonoBehaviour
{
    [SerializeField] Animator camAnimator;
    [SerializeField] Animator badger;
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
        camAnimator.SetTrigger("TFTrigger");
        badger.SetTrigger("TF");
        yield return new WaitForSeconds(3);
        StartCoroutine(GetBadgerAway());
        predatorController.enabled = true;
    }
}

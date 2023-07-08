using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TFHandler : MonoBehaviour
{
    [SerializeField] Animator camAnimator;
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
        yield return new WaitForSeconds(3);
        predatorController.enabled = true;
    }
}

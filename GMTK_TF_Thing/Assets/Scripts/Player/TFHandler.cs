using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TFHandler : MonoBehaviour
{
    [SerializeField] SpriteRenderer sprite;
    [SerializeField] Animator camAnimator;
    [SerializeField] Sprite testerSprite;
    private FrontController preyController;
    private BackController predatorController;

    private void Awake()
    {
        preyController = GetComponent<FrontController>();
        predatorController = GetComponent<BackController>();
        predatorController.enabled = false;

        preyController.OnDeath += TurnToPredator;
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
        sprite.sprite = testerSprite;
        predatorController.enabled = true;
    }
}

using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BackController : GeneralController
{
    [SerializeField] BadgerHandler badger;
    protected override bool GoesForward => false;
    private int count;

    private void FixedUpdate()
    {
        count++;
        if(count % 20 == 0)
        {
            FindObjectOfType<ScreenShaker>().ShakeScreen(0.2f, 0.1f);
        }
    }

    protected override void SpecialPowers()
    {
        var rotations = Physics.OverlapSphere(transform.position, size, LayerLibrary.Finale);
        if(rotations.Length > 0)
        {
            FindObjectOfType<Finale>().GameOver(false);
            animator.SetTrigger("return");
            this.enabled = false;
            shadowAnimator.enabled = false;
        }
    }


    protected override void Rotate(float amount)
    {
        base.Rotate(amount);
        badger.transform.Rotate(0, amount, 0);
    }
}

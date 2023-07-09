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

    protected override void Rotate(float amount)
    {
        base.Rotate(amount);
        badger.transform.Rotate(0, amount, 0);
    }
}

using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FrontController : GeneralController
{
    [SerializeField] float jumpinTime;
    [SerializeField] float jumpinCooldown;
    private bool jumping;
    private float jumpinCooldownLeft;
    protected override bool IsJumping => jumping;

    protected override void Cooldowns(float time)
    {
        base.Cooldowns(time);
        jumpinCooldownLeft -= time;
    }

    protected override void SpecialPowers()
    {
        if (Input.GetKey(KeyCode.W))
        {
            if (jumpinCooldownLeft > 0 || jumping) return;
            Jump();
        }
    }

    private void Jump()
    {
        jumping = true;
        StartCoroutine(JumpingTime());
    }

    private IEnumerator JumpingTime()
    {
        transform.position += new Vector3(0, 0.2f);
        yield return new WaitForSeconds(jumpinTime);
        jumpinCooldownLeft = jumpinCooldown;
        jumping = false;
        transform.position += new Vector3(0, -0.2f);
    }
}

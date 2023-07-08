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
        animator.SetBool("jumping", true);
        shadowAnimator.SetBool("jumping", true);
        StartCoroutine(JumpingTime());
    }

    private IEnumerator JumpingTime()
    {
        float timePassed = 0;
        Vector3 jumpVec = new Vector3(0, 0.7f, -0.1f);
        while (timePassed < jumpinTime / 3) { 
            camHolder.localPosition += jumpVec * Time.deltaTime;
            timePassed += Time.deltaTime;
            yield return null;
        }
        yield return new WaitForSeconds(jumpinTime / 3 * 2);
        animator.SetBool("jumping", false);
        shadowAnimator.SetBool("jumping", false);
        while (camHolder.localPosition.y > 0)
        {
            camHolder.localPosition -= jumpVec * 2 * Time.deltaTime;
            yield return null;
        }
        camHolder.localPosition = Vector3.zero;
        jumpinCooldownLeft = jumpinCooldown;
        jumping = false;
    }

    protected override void Die()
    {
        base.Die();
        animator.SetTrigger("TF");
    }
}

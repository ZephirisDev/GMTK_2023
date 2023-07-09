using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FrontController : GeneralController
{
    [SerializeField] float jumpinTime;
    [SerializeField] float jumpinCooldown;
    [SerializeField] BadgerHandler badger;
    private bool jumping;
    private float jumpinCooldownLeft;

    private float cooldownBadger;

    protected override bool IsJumping => jumping;

    protected override void Cooldowns(float time)
    {
        base.Cooldowns(time);
        jumpinCooldownLeft -= time;
        if (curSpeed != speed) return;
        cooldownBadger -= time;
        if(cooldownBadger < 0)
        {
            cooldownBadger = 0.25f;
            badger.AddPos(transform.position);
        }
    }

    private int count;

    private void FixedUpdate()
    {
        count++;
        if (count % 20 == 0)
        {
            AudioHandler.TryPlaySound(SoundIdentifier.Walk);
        }
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
        AudioHandler.TryPlaySound(SoundIdentifier.Jump);
        float timePassed = 0;
        Vector3 jumpVec = new Vector3(0, 1f, -0.1f);
        var playerContPos = animator.transform.localPosition;
        while (timePassed < jumpinTime / 3) { 
            camHolder.localPosition += jumpVec * Time.deltaTime;
            animator.transform.localPosition += new Vector3(0, 1.7f) * Time.deltaTime;
            timePassed += Time.deltaTime;
            yield return null;
        }
        yield return new WaitForSeconds(jumpinTime / 3 * 2);
        animator.SetBool("jumping", false);
        shadowAnimator.SetBool("jumping", false);
        while (camHolder.localPosition.y > 0)
        {
            camHolder.localPosition -= jumpVec * 2 * Time.deltaTime;
            animator.transform.localPosition -= new Vector3(0, 2f) * Time.deltaTime;
            yield return null;
        }
        animator.transform.localPosition = playerContPos;
        camHolder.localPosition = Vector3.zero;
        jumpinCooldownLeft = jumpinCooldown;
        if (damageCooldown <= 0)
            damageCooldown = 0.1f;
        jumping = false;
    }

    public override void Damage()
    {
        base.Damage();
        AudioHandler.TryPlaySound(SoundIdentifier.Hurt);
        cooldownBadger += 0.2f;
    }

    protected override void Die()
    {
        base.Die();
        FindObjectOfType<WorldGenerator>().Switch();
    }
}

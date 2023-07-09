using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GeneralController : MonoBehaviour
{
    [Tooltip("Speed in which the player goes forward")]
    [SerializeField] protected int speed;
    [Tooltip("Knockback Power, preferably around 2x of speed")]
    [SerializeField] int knockbackIntensity;
    [Tooltip("Speed that the player walks sideways")]
    [SerializeField] float sideSpeed;
    [Tooltip("How many times can the player run into something?")]
    [SerializeField] int totalHP;
    [Tooltip("How big is the player hitbox?")]
    [SerializeField] protected float size;
    [SerializeField] protected Animator animator;
    [SerializeField] protected Animator shadowAnimator;
    [SerializeField] protected Transform camHolder;

    // Could use this for GameOverHandlers, ScreenShakeHandler, UI, etc.
    public System.Action OnDamage, OnDeath;

    protected int curSpeed;
    private int hpLeft;
    protected float damageCooldown;
    protected bool disableSide;

    protected virtual bool GoesForward => true;
    private int Direction => GoesForward ? 1 : -1;
    protected virtual bool IsJumping => false;

    private void Awake()
    {
        hpLeft = totalHP;
        curSpeed = speed;
    }

    private void Update()
    {
        var isImmune = damageCooldown > 0;
        Cooldowns(Time.deltaTime);
        if(isImmune && damageCooldown < 0)
        {
            if (!AttemptMovement(Vector3.zero, false))
                damageCooldown = 0.1f;
        }
        SpecialPowers();

        float zMovement = curSpeed * Time.deltaTime * Direction;

        TryMoveForward(zMovement);

        if (disableSide) return;

        float xMovement = 0;

        if (Input.GetKey(KeyCode.A))
            xMovement = -1;
        else if (Input.GetKey(KeyCode.D))
            xMovement = 1;

        if (Input.GetKeyDown(KeyCode.R))
            Damage();

        CheckForRotations();

        if (xMovement == 0) return;

        var sideMovement = sideSpeed * Time.deltaTime * xMovement;

        if (AttemptMovement(transform.right * sideMovement, true))
            transform.position += transform.right * sideMovement;
    }

    protected void CheckForRotations()
    {
        var rotations = Physics.OverlapSphere(transform.position, size, LayerLibrary.Rotators);
        foreach(var r in rotations)
        {
            if(r.TryGetComponent(out Rotater rot))
            {
                if (rot.DoRotation(GoesForward))
                {
                    StartCoroutine(DoRotation(rot.preyGoLeft));
                }
            }
        }
    }

    IEnumerator DoRotation(bool left)
    {
        curSpeed = speed;
        if (!GoesForward) left = !left;
        disableSide = true;
        yield return new WaitForEndOfFrame();
        float angles = 90;
        curSpeed = speed;
        while (angles > 0)
        {
            Rotate(Mathf.Clamp(70 * Time.deltaTime * (left ? -1 : 1), -angles, angles));
            angles -= 70 * Time.deltaTime;

            yield return null;
        }
        disableSide = false;
    }

    protected virtual void Rotate(float amount)
    {
        transform.Rotate(0, amount, 0);
    }

    protected virtual void Cooldowns(float time)
    {
        damageCooldown -= time;
    }

    protected virtual void SpecialPowers() { }

    protected virtual void TryMoveForward(float movement)
    {
        if (!AttemptMovement(transform.forward * movement, false))
        {
            if (!ExtraCheck(transform.forward * movement))
            {
                Damage();
                StartCoroutine(SpeedBack());
                return;
            }
        }
        Uhhhh(transform.forward * movement);
            transform.position += transform.forward * movement;

            

    }

    // Makes it so the player has a lil knockback effect
    private IEnumerator SpeedBack()
    {
        curSpeed = -knockbackIntensity;
        while (curSpeed < speed)
        {
            curSpeed++;
            yield return new WaitForSeconds(0.03f);
        }
        curSpeed = speed;
    }

    private bool AttemptMovement(Vector3 movementVector, bool isSide)
    {
        var obstacles = Physics.OverlapSphere(transform.position + movementVector, size, LayerLibrary.Obstacles | LayerLibrary.Bounds);
        bool canPass = true;
        foreach(var obs in obstacles)
        {
            if (obs.TryGetComponent(out Obstacle obstacleComponent))
            {
                if (damageCooldown > 0) continue;
                if (!obstacleComponent.IsWalkable(GoesForward, IsJumping))
                    return false;
            }
            else
                canPass = false;
        }
        return canPass;
    }

    private void Uhhhh(Vector3 movementVector)
    {
        var obstacles = Physics.OverlapSphere(transform.position + movementVector, size * 0.75f, LayerLibrary.Obstacles);
        foreach (var obs in obstacles)
        {
            if (obs.TryGetComponent(out Obstacle obstacleComponent))
            {
                obstacleComponent.RunInto(GoesForward, IsJumping);
            }
        }
    }

    private bool ExtraCheck(Vector3 movementVector)
    {
        var obstacles = Physics.OverlapSphere(transform.position + movementVector, size * 0.75f, LayerLibrary.Obstacles);
        bool canPass = true;
        foreach (var obs in obstacles)
        {
            if (obs.TryGetComponent(out Obstacle obstacleComponent))
            {
                if (!obstacleComponent.IsWalkable(GoesForward, IsJumping))
                {
                    canPass = false;
                }
                obstacleComponent.RunInto(GoesForward, IsJumping);
            }
        }

        return canPass && Physics.OverlapSphere(transform.position + movementVector, size, LayerLibrary.Bounds).Length == 0;
    }

    protected virtual void Die()
    {
        AudioHandler.TryPlaySound(SoundIdentifier.Hurt);
        Debug.Log("I died :(");
        OnDeath?.Invoke();
    }

    public virtual void Damage()
    {
        if (damageCooldown > 0) return;

        damageCooldown = GoesForward ? 0.8f : 0.2f;
        if(--hpLeft <= 0)
        {
            Die();
        }
        FindObjectOfType<ScreenShaker>().ShakeScreen(0.15f, 0.05f);
        Debug.Log($"Yeouch! ({hpLeft}/{totalHP})");
        OnDamage?.Invoke();
    }
}

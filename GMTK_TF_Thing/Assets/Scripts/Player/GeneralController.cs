using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GeneralController : MonoBehaviour
{
    [Tooltip("Speed in which the player goes forward")]
    [SerializeField] int speed;
    [Tooltip("Knockback Power, preferably around 2x of speed")]
    [SerializeField] int knockbackIntensity;
    [Tooltip("Speed that the player walks sideways")]
    [SerializeField] float sideSpeed;
    [Tooltip("How many times can the player run into something?")]
    [SerializeField] int totalHP;
    [Tooltip("How big is the player hitbox?")]
    [SerializeField] float size;
    [SerializeField] protected Animator animator;
    [SerializeField] protected Animator shadowAnimator;
    [SerializeField] protected Transform camHolder;

    // Could use this for GameOverHandlers, ScreenShakeHandler, UI, etc.
    public System.Action OnDamage, OnDeath;

    private int curSpeed;
    private int hpLeft;
    private float damageCooldown;
    private bool disableSide;

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
        Cooldowns(Time.deltaTime);
        SpecialPowers();

        float zMovement = curSpeed * Time.deltaTime * Direction;

        TryMoveForward(zMovement);

        if (disableSide) return;

        float xMovement = 0;

        if (Input.GetKey(KeyCode.A))
            xMovement = -1;
        else if (Input.GetKey(KeyCode.D))
            xMovement = 1;

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
                Debug.Log("WOAHHUH??");
                if (rot.DoRotation(GoesForward))
                {
                    Debug.Log("WOAH!");
                    StartCoroutine(DoRotation(rot.preyGoLeft));
                }
            }
        }
    }

    IEnumerator DoRotation(bool left)
    {
        if (!GoesForward) left = !left;
        disableSide = true;
        yield return new WaitForEndOfFrame();
        float angles = 90;
        while(angles > 0)
        {
            transform.Rotate(0, Mathf.Clamp(90 * Time.deltaTime * (left ? -1 : 1), -angles, angles), 0);
            angles -= 90 * Time.deltaTime;

            Debug.Log(angles);
            yield return null;
        }
        disableSide = false;
    }

    protected virtual void Cooldowns(float time)
    {
        damageCooldown -= time;
    }

    protected virtual void SpecialPowers() { }

    protected virtual void TryMoveForward(float movement)
    {
        if (AttemptMovement(transform.forward * movement, false))
        {
            transform.position += transform.forward * movement;
        }
        else
        {
            Damage();
            StartCoroutine(SpeedBack());
        }
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
        var obstacles = Physics.OverlapSphere(transform.position + movementVector, size, LayerLibrary.Obstacles);
        bool canPass = true;
        foreach(var obs in obstacles)
        {
            if (obs.TryGetComponent(out Obstacle obstacleComponent))
            {
                if (!obstacleComponent.IsWalkable(GoesForward, IsJumping))
                    canPass = false;
                if (!isSide || obstacleComponent.DestroyOnSideContact)
                    obstacleComponent.RunInto(GoesForward, IsJumping);
            }
            else
                canPass = false;
        }
        return canPass;
    }

    protected virtual void Die()
    {
        Debug.Log("I died :(");
        OnDeath?.Invoke();
    }

    public virtual void Damage()
    {
        if (damageCooldown > 0) return;

        damageCooldown = 0.3f;
        if(--hpLeft <= 0)
        {
            Die();
        }
        FindObjectOfType<ScreenShaker>().ShakeScreen(0.15f, 0.05f);
        Debug.Log($"Yeouch! ({hpLeft}/{totalHP})");
        OnDamage?.Invoke();
    }
}

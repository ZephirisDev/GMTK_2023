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

    // Could use this for GameOverHandlers, ScreenShakeHandler, UI, etc.
    public System.Action OnDamage, OnDeath;

    private int curSpeed;
    private int hpLeft;
    private float damageCooldown;

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

        TryMoveForward(new Vector3(0, 0, zMovement));

        float xMovement = 0;

        if (Input.GetKey(KeyCode.A))
            xMovement = -1;
        else if (Input.GetKey(KeyCode.D))
            xMovement = 1;

        var sideMovement = sideSpeed * Time.deltaTime * xMovement;

        var sideVector = new Vector3(sideMovement, 0);

        if (AttemptMovement(sideVector))
            transform.position += sideVector;
    }

    protected virtual void Cooldowns(float time)
    {
        damageCooldown -= time;
    }

    protected virtual void SpecialPowers() { }

    protected virtual void TryMoveForward(Vector3 movementVector)
    {
        if (AttemptMovement(movementVector))
        {
            transform.position += movementVector;
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

    private bool AttemptMovement(Vector3 addition)
    {
        var obstacles = Physics.OverlapSphere(transform.position + addition, size, LayerLibrary.Obstacles);
        bool canPass = true;
        foreach(var obs in obstacles)
        {
            if (obs.TryGetComponent(out Obstacle obstacleComponent))
            {
                if (!obstacleComponent.IsWalkable(GoesForward, IsJumping))
                    canPass = false;
                if (addition.z != 0 || obstacleComponent.DestroyOnSideContact)
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
        Debug.Log($"Yeouch! ({hpLeft}/{totalHP})");
        OnDamage?.Invoke();
    }
}

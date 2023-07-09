using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Obstacle : MonoBehaviour
{
    [SerializeField] HitBehaviour preyBehaviour;
    [SerializeField] HitBehaviour predatorBehaviour;
    [SerializeField] bool destroyOnSideContact;
    [SerializeField] bool ignoreWhenJumping;
    [SerializeField] ParticleType type;
    [SerializeField] int amount;
    public bool DestroyOnSideContact => destroyOnSideContact;

    /// <summary>
    ///  When a player runs into an obstacle, it might break or not, depending on if the player is TFed :3
    /// </summary>
    /// <param name="isTFed">Is the Player being chased or chasing?</param>
    /// <returns>Does the player take damage?</returns>
    public void RunInto(bool isPrey, bool isJumping = false)
    {
        if (ignoreWhenJumping && isJumping) return;
        var check = isPrey ? preyBehaviour : predatorBehaviour;

        if (check == HitBehaviour.DestroyThis || check == HitBehaviour.DamagePlayerAndDestroyThis)
            DestroyObstacle();
    }

    public bool IsWalkable(bool isPrey, bool isJumping = false)
    {
        if (ignoreWhenJumping && isJumping) return true;
        var check = isPrey ? preyBehaviour : predatorBehaviour;
        return check == HitBehaviour.Passage || check == HitBehaviour.DestroyThis;
    }

    private void DestroyObstacle()
    {
        // Destroy this and maybe do some particle stuff
        if (ded) return;
        StartCoroutine(WaitTilEnd());
        ded = true;
        AudioHandler.TryPlaySound(SoundIdentifier.Destroy);
    }

    private bool ded;

    private IEnumerator WaitTilEnd()
    {
        yield return new WaitForEndOfFrame();
        ParticleHandler.PlayParticles(transform.position, transform.rotation, type, amount);
        Destroy(gameObject);

    }

    enum HitBehaviour
    {
        Passage,
        DamagePlayer,
        DestroyThis,
        DamagePlayerAndDestroyThis
    }
}

using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ParticleHandler : MonoBehaviour
{
    [SerializeField] List<ParticleClasses> particles;
    private static ParticleHandler Instance;

    private void Awake()
    {
        Instance = this;
    }

    public static void PlayParticles(Vector3 pos, Quaternion rot, ParticleType type, int amount)
    {
        var c = Instance.particles.Find(x => x.type == type);
        if (c == null) return;
        c.particles.transform.position = pos;
        c.particles.transform.rotation = rot;
        c.particles.Emit(amount);
    }

}

[System.Serializable]
public class ParticleClasses
{
    [SerializeField] public ParticleSystem particles;
    [SerializeField] public ParticleType type;
}

public enum ParticleType
{
    Log,
    LogNoRoot,
    Stone
}
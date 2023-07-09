using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AudioHandler : MonoBehaviour
{
    private static AudioHandler Singleton;
    [SerializeField] AudioSource prefab;
    public List<Soundssss> sounds;

    private void Awake()
    {
        Singleton = this;
    }

    public static void TryPlaySound(SoundIdentifier identifier)
    {
        Debug.Log(identifier);
        AudioSource source = Instantiate(Singleton.prefab);
        var s = Singleton.sounds.Find(x => x.s == identifier);
        source.clip = s.clip;
        source.pitch = 1 + Random.Range(-0.05f, 0.05f);

        source.volume = s.volume;

        source.Play();
        Destroy(source.gameObject, s.clip.length + 0.1f);
    }
}

[System.Serializable]
public class Soundssss
{
    public SoundIdentifier s;
    public AudioClip clip;
    public float volume;
}

public enum SoundIdentifier
{
    Walk,
    Jump,
    Jump_2,
    Destroy,
    Hurt,
    Stomp,
    Grrr,
    Nom,
    Sigh,
    Wink,
    Button_Move,
    Button_Start
}

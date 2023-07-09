using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class music : MonoBehaviour
{

    public AudioSource source;
    public AudioClip[] songs;

    public void PlaySong(int song) {
        source.clip = songs[song];
        source.Play();
        switch (song)
        {
            case 0:
                source.volume = 0.3f;
                break;
            case 1:
                source.volume = 0.1f;
                break;
            case 2:
                source.volume = 0.1f;
                break;
        }
    }

    void Start() {
        PlaySong(0);
    }
}

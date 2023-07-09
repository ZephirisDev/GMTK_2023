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
    }

    void Start() {
        PlaySong(0);
    }
}

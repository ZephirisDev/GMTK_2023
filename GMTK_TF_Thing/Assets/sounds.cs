using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class sounds : MonoBehaviour
{
    public AudioSource source;
    public AudioClip[] soundArray;

    public void PlaySound(int sound) {
        source.clip = soundArray[sound];
        source.Play();
    }
}

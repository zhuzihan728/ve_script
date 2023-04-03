using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RecordingHelper : MonoBehaviour
{
    public static RecordingHelper instance;
    public AudioSource[] audioSourceArray;

    private void Awake()
    {
        instance = this;
        audioSourceArray = FindObjectsOfType<AudioSource>();
    }

    public void Play(int id)
    {
        audioSourceArray[id-1].Play();
    }
}

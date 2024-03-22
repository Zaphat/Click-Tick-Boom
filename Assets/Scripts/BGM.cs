using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BGM : MonoBehaviour
{
    private static BGM instance;
    [SerializeField] private AudioSource bgm;
    [SerializeField] private AudioClip[] bgmClips;
    private static int currentClipIndex = 0;
    private void Awake()
    {
        if (instance == null)
        {
            instance = this;
        }
    }
    private void Start()
    {
        bgm.clip = bgmClips[Random.Range(0, bgmClips.Length)];
        bgm.Play();
    }

    public void ChangeMusic()
    {
        int newClipIndex = Random.Range(0, bgmClips.Length);
        while (newClipIndex == currentClipIndex)
        {
            newClipIndex = Random.Range(0, bgmClips.Length);
        }
        currentClipIndex = newClipIndex;
        bgm.clip = bgmClips[currentClipIndex];
        bgm.Play();
    }

}

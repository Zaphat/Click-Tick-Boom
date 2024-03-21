using System.Collections;
using System.Collections.Generic;
using UnityEngine;


public class SFX : MonoBehaviour
{
    public static SFX instance;
    [SerializeField] private AudioSource audioSource;
    [SerializeField] private AudioClip gameOverSound;
    [SerializeField] private AudioClip buttonSound;
    void Awake()
    {
        if (instance == null)
        {
            instance = this;
        }
        audioSource = GetComponent<AudioSource>();
    }
    public void PlayOnPickup(AudioClip[] audioClip, Transform transform, float volume = 0.75f)
    {
        //AudioSource audioSource = Instantiate(audioSource, transform.position, Quaternion.identity);
        var randomClip = audioClip[Random.Range(0, audioClip.Length)];
        audioSource.clip = randomClip;
        audioSource.volume = volume;
        audioSource.Play();
        //Destroy(audioSource.gameObject, randomClip.length);
    }
    public void PlayOnGameOver()
    {
        //AudioSource audioSource = Instantiate(audioSource, transform.position, Quaternion.identity);
        audioSource.clip = gameOverSound;
        audioSource.Play();
        //Destroy(audioSource.gameObject, gameOverSound.length);
    }
    public void ButtonClick()
    {

        //AudioSource audioSource = Instantiate(audioSource, transform.position, Quaternion.identity);
        audioSource.clip = buttonSound;
        audioSource.Play();
        //Destroy(audioSource.gameObject, buttonSound.length);
    }
}

using System.Collections;
using System.Collections.Generic;
using UnityEngine;


public class SFX : MonoBehaviour
{
    public static SFX instance;
    [SerializeField] private AudioSource audioSourcePrefab;
    [SerializeField] private AudioClip gameOverSound;
    [SerializeField] private AudioClip buttonSound;
    [SerializeField] private AudioClip touchSound;
    void Awake()
    {
        if (instance == null)
        {
            instance = this;
        }
        audioSourcePrefab = GetComponent<AudioSource>();
    }
    public void PlayOnPickup(AudioClip[] audioClip, Transform transform, float volume = 0.75f)
    {
        AudioSource audioSource = Instantiate(audioSourcePrefab, transform.position, Quaternion.identity);
        AudioClip randomClip = audioClip[Random.Range(0, audioClip.Length)];
        audioSource.clip = randomClip;
        audioSource.volume = volume;
        audioSource.Play();
        Destroy(audioSource.gameObject, randomClip.length);
    }
    public void PlayOnGameOver()
    {
        AudioSource audioSource = Instantiate(audioSourcePrefab, transform.position, Quaternion.identity);
        audioSource.clip = gameOverSound;
        audioSource.Play();
        Destroy(audioSource.gameObject, gameOverSound.length);
    }
    public void ButtonClick()
    {
        AudioSource audioSource = Instantiate(audioSourcePrefab, transform.position, Quaternion.identity);
        audioSource.clip = buttonSound;
        audioSource.Play();
        Destroy(audioSource.gameObject, buttonSound.length);
    }
    public void TouchSound()
    {
        AudioSource audioSource = Instantiate(audioSourcePrefab, transform.position, Quaternion.identity);
        audioSource.clip = touchSound;
        audioSource.pitch = Random.Range(0.8f, 1.2f);
        audioSource.Play();
        Destroy(audioSource.gameObject, touchSound.length);
    }
}

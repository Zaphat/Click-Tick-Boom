using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SwitchBackground : MonoBehaviour
{
    public static SwitchBackground instance;

    [SerializeField] private Sprite[] backgroundImages;
    private int currentImageIndex = 0;
    void Awake()
    {
        if (instance == null)
        {
            instance = this;
        }
    }
    public void ChangeImage()
    {
        int newImageIndex = Random.Range(0, backgroundImages.Length);
        while (newImageIndex == currentImageIndex)
        {
            newImageIndex = Random.Range(0, backgroundImages.Length);
        }
        currentImageIndex = newImageIndex;
        GetComponent<SpriteRenderer>().sprite = backgroundImages[currentImageIndex];
    }
}

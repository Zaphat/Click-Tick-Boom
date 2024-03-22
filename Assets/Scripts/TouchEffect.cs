using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TouchEffect : MonoBehaviour
{
    [SerializeField] private GameObject[] touchEffect;


    void Update()
    {
#if UNITY_EDITOR
        if (Input.GetMouseButtonDown(0) && Time.timeScale != 0)
        {
            SFX.instance.TouchSound();
            Vector3 mousePos = Camera.main.ScreenToWorldPoint(Input.mousePosition);
            mousePos.z = 0;
            Destroy(Instantiate(touchEffect[Random.Range(0, touchEffect.Length)], mousePos, Quaternion.identity), 0.5f);
        }
#else
        if (Input.touchCount > 0 && Time.timeScale != 0)
        {
            SFX.instance.TouchSound();
            foreach (Touch touch in Input.touches)
            {
                Vector3 touchPos = Camera.main.ScreenToWorldPoint(touch.position);
                touchPos.z = 0;
                Destroy(Instantiate(touchEffect[Random.Range(0, touchEffect.Length)], touchPos, Quaternion.identity), 0.5f);
            }
        }
#endif
    }
}

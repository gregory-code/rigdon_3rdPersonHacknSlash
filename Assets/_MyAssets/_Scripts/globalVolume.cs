using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Rendering;
using UnityEngine.Rendering.HighDefinition;

public class globalVolume : MonoBehaviour
{
    [SerializeField] private Volume volume;
    private HDRISky sky;

    private void Start()
    {
        if (volume.profile.TryGet<HDRISky>(out HDRISky tempE))
        {
            sky = tempE;
        }
    }

    private void Update()
    {
        if (Time.timeScale == 0)
            return;

        sky.exposure.value -= 0.01f;
    }
}

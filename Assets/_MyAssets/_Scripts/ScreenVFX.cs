using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ScreenVFX : MonoBehaviour
{
    [SerializeField] float shakeDuration = 0.2f;
    [SerializeField] float shakeMangintude = 0.1f;

    bool shaking;

    public void StartShake()
    {
        if (!shaking)
        {
            StartCoroutine(ShakeCoroutine());
        }
    }

    IEnumerator ShakeCoroutine()
    {
        shaking = true;
        yield return new WaitForSeconds(shakeDuration);
        shaking = false;
    }

    private void LateUpdate()
    {
        if (shaking)
        {
            Vector3 shakeAmount = new Vector3(Random.value, Random.value, Random.value) * shakeMangintude * (Random.value > 0.5f ? 1 : -1);
            transform.localPosition += shakeAmount;
        }
        else
        {
            transform.localPosition = Vector3.Lerp(transform.localPosition, Vector3.zero, 5 * Time.deltaTime);
        }
    }
}

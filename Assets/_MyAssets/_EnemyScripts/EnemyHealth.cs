using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class EnemyHealth : MonoBehaviour
{
    [SerializeField] Image healthImage;
    public void SetValue(float val, float maxVal)
    {
        healthImage.fillAmount = val / maxVal;
    }
}

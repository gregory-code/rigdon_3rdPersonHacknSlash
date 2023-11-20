using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class damagepop : MonoBehaviour
{
    private Transform attachParentTransform;

    public void Init(Transform[] spawnPos, float damage)
    {
        attachParentTransform = spawnPos[Random.Range(0, spawnPos.Length)];
        transform.GetChild(0).GetComponent<TextMeshProUGUI>().text = damage.ToString();
        Destroy(this.gameObject, 1);
    }


    private void Update()
    {
        transform.position = Camera.main.WorldToScreenPoint(attachParentTransform.position);
    }
}

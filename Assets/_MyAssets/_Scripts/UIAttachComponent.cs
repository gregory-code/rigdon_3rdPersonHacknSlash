using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class UIAttachComponent : MonoBehaviour
{
    private Transform attachParentTransform;
    private Transform player;

    private Vector3 cashedScale;

    public void SetupAttachment(Transform attachParentTransform, Transform player)
    {
        cashedScale = transform.localScale;
        this.attachParentTransform = attachParentTransform;
        this.player = player;
    }

    private void Update()
    {
        Vector3 newScale = Vector3.zero;

        if(Vector3.Distance(attachParentTransform.position, player.position) < 10)
        {
            newScale = cashedScale;
        }

        transform.localScale = Vector3.Lerp(transform.localScale, newScale, 10 * Time.deltaTime);
        transform.position = Camera.main.WorldToScreenPoint(attachParentTransform.position);
    }
}

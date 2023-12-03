using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[ExecuteAlways]
public class menuScript : MonoBehaviour
{
    [SerializeField] Transform settingsButton;
    [SerializeField] Transform settingsAttachment;

    [SerializeField] Transform returnButton;
    [SerializeField] Transform returnAttachment;

    [SerializeField] Animator cameraAnimator;

    void Start()
    {
        SetReturnFalse();
    }


    void Update()
    {
        settingsButton.position = Camera.main.WorldToScreenPoint(settingsAttachment.position);
        if(returnButton.gameObject.activeInHierarchy == true) returnButton.position = Camera.main.WorldToScreenPoint(returnAttachment.position);
    }

    public void OpenSettings()
    {
        cameraAnimator.ResetTrigger("return");
        cameraAnimator.SetTrigger("settings");
    }

    public void Return()
    {
        cameraAnimator.ResetTrigger("settings");
        cameraAnimator.SetTrigger("return");
    }

    public void SetReturnFalse()
    {
        returnButton.gameObject.SetActive(false);
    }

    public void SetReturnTrue()
    {
        returnButton.gameObject.SetActive(true);
    }
}

using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.SceneManagement;

[ExecuteAlways]
public class menuScript : MonoBehaviour
{
    [SerializeField] Transform settingsButton;
    [SerializeField] Transform settingsAttachment;

    [SerializeField] Transform returnButton;
    [SerializeField] Transform returnAttachment;

    [SerializeField] Animator cameraAnimator;

    [SerializeField] TextMeshProUGUI pacifistText;
    bool pacifistModeOn;

    void Start()
    {
        PlayerPrefs.SetInt("pacifist", 0);
        SetReturnFalse();
    }


    void Update()
    {
        settingsButton.position = Camera.main.WorldToScreenPoint(settingsAttachment.position);
        if(returnButton.gameObject.activeInHierarchy == true) returnButton.position = Camera.main.WorldToScreenPoint(returnAttachment.position);
    }

    public void PacifistMode()
    {
        pacifistModeOn = !pacifistModeOn;
        if(pacifistModeOn)
        {
            PlayerPrefs.SetInt("pacifist", 1);
            pacifistText.text = "Pacifist Mode: On";
        }
        else
        {
            PlayerPrefs.SetInt("pacifist", 0);
            pacifistText.text = "Pacifist Mode: Off";
        }
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

    public void UpdateSens(float value)
    {
        PlayerPrefs.SetFloat("Sens", value);
    }

    public void StartGame()
    {
        SceneManager.LoadScene(1);
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

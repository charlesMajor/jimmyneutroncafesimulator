using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class WindowsManager : MonoBehaviour
{
    private bool controlsWindowOpened = false;
    private bool commandsWindowOpened = false;

    [SerializeField] private GameObject panel;
    [SerializeField] private TMP_Text controlsText;
    [SerializeField] private TMP_Text commandTitleText;
    [SerializeField] private TMP_Text[] commandsText;

    // Start is called before the first frame update
    void Start()
    {
    }

    // Update is called once per frame
    void Update()
    {
        manageControlsWindow();
        manageCommandsWindow();
    }

    private void manageControlsWindow()
    {
        if (Input.GetButtonDown("Controls Window"))
        {
            if (!controlsWindowOpened && !commandsWindowOpened)
            {
                controlsWindowOpened = true;
                panel.SetActive(true);
                GameManager.Instance.freezeTime();
                controlsText.enabled = true;
            }
            else if (controlsWindowOpened)
            {
                controlsWindowOpened = false;
                panel.SetActive(false);
                GameManager.Instance.unfreezeTime();
                controlsText.enabled = false;
            }
        }
    }

    private void manageCommandsWindow()
    {
        if (Input.GetButtonDown("Commands Window"))
        {
            if (!commandsWindowOpened && !controlsWindowOpened)
            {
                commandsWindowOpened = true;
                panel.SetActive(true);
                GameManager.Instance.freezeTime();
                commandTitleText.enabled = true;
                foreach (TMP_Text text in commandsText)
                {
                    text.enabled = true;
                }
            }
            else if (commandsWindowOpened)
            {
                commandsWindowOpened = false;
                panel.SetActive(false);
                GameManager.Instance.unfreezeTime();
                commandTitleText.enabled = false;
                foreach (TMP_Text text in commandsText)
                {
                    text.enabled = false;
                }
            }
        }
    }

    public TMP_Text getAvailableCommandText()
    {
        foreach (TMP_Text text in commandsText)
        {
            if (text.text == "")
            {
                return text;
            }
        }
        return null;
    }

    public TMP_Text getTextWithProduct(Products product)
    {
        foreach (TMP_Text text in commandsText)
        {
            if (text.text.Contains(product.ToString()))
            {
                return text;
            }
        }
        return null;
    }
}

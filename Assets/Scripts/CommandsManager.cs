using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class CommandsManager : MonoBehaviour
{
    private Dictionary<Products, int> amountOfEachProduct = new Dictionary<Products, int>();
    WindowsManager windowsManager;

    // Start is called before the first frame update
    void Start()
    {
        foreach (Products product in (Products[])System.Enum.GetValues(typeof(Products)))
        {
            if (product.ToString() != "None" && product.ToString() != "Trash")
            {
                amountOfEachProduct[product] = 0;
            }
        }

        windowsManager = GetComponent<WindowsManager>();
        setBaseCommandsWindow();
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public Products getRandomCommand()
    {
        int product = Random.Range(0, 2);
        Products command = new List<Products>(amountOfEachProduct.Keys)[product];
        amountOfEachProduct[command]++;

        updateCommandsWindow();
        return command;
    }

    public void removeCommand(Products command)
    {
        if (amountOfEachProduct[command] > 0)
        {
            amountOfEachProduct[command]--;
        }
        updateCommandsWindow();
    }

    private void setBaseCommandsWindow()
    {
        foreach (Products product in amountOfEachProduct.Keys)
        {
            TMP_Text text = windowsManager.getAvailableCommandText();
            text.text = product.ToString() + " : " + amountOfEachProduct[product];
        }
    }

    private void updateCommandsWindow()
    {
        foreach (Products product in amountOfEachProduct.Keys)
        {
            TMP_Text text = windowsManager.getTextWithProduct(product);
            text.text = product.ToString() + " : " + amountOfEachProduct[product];
        }
    }
}

using System.Collections;
using System.Collections.Generic;
using System.Data.SqlTypes;
using TMPro;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public enum Products { None, Trash, Coffee, Donut }
public class GameManager : MonoBehaviour
{
    public static GameManager Instance;

    [SerializeField] private Image rollImage;
    private float rollImageBaseHeight;
    [SerializeField] private GameObject jimmy;
    private JimmyController jimmyController;

    private int money = 0;
    [SerializeField] int amountOfMoneyNeeded = 1000;
    [SerializeField] TMP_Text moneyText;

    void Awake()
    {
        if (Instance == null)
        {
            Instance = this;
        }
        else if (Instance != this)
        {
            Destroy(gameObject);
        }
    }


    // Start is called before the first frame update
    void Start()
    {
        jimmyController = jimmy.GetComponent<JimmyController>();
        rollImageBaseHeight = rollImage.preferredHeight;
        updateMoneyText();
    }

    // Update is called once per frame
    void Update()
    {
        updateRollIcon();
    }

    private void updateRollIcon()
    {
        float newHeight = rollImageBaseHeight - (rollImageBaseHeight * jimmyController.getTimeUntilNextRoll() / jimmyController.getTimeBetweenRolls());
        RectTransform rectTransform = rollImage.rectTransform;
        rectTransform.sizeDelta = new Vector2(rectTransform.sizeDelta.x, newHeight);

        
        Color iconColor;

        if (jimmyController.getTimeUntilNextRoll() != 0)
        {
            iconColor = Color.white;
            iconColor.a = 1 - (1 * jimmyController.getTimeUntilNextRoll() / jimmyController.getTimeBetweenRolls());
        }
        else
        {
            iconColor = Color.green;
        }

        rollImage.color = iconColor;
    }

    public void addMoney(int amount)
    {
        money += amount;
        updateMoneyText();
        checkIfGameEnd();
    }

    public void updateMoneyText()
    {
        moneyText.text = money.ToString() + "$";
    }

    public void checkIfGameEnd()
    {
        if (money >= amountOfMoneyNeeded)
        {
            SceneManager.LoadScene("EndScene");
        }
    }

    public void freezeTime()
    {
        Time.timeScale = 0f;
    }

    public void unfreezeTime()
    {
        Time.timeScale = 1f;
    }
}

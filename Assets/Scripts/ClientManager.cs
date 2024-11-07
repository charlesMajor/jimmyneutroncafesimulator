using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class ClientManager : MonoBehaviour
{
    private GameObject table = null;
    public enum CommandState { None, Ready, Waiting, Eating, Completed }
    public CommandState currentCommandState;
    [SerializeField] private int moneyForCommand = 50;
    private bool reducedMoney = false;
    [SerializeField] private float clientSpeed = 2f;
    [SerializeField] private GameObject targetDestination;
    [SerializeField] private float timeToEat = 3f;
    private float timeSinceStartedEating = 0f;
    private Rigidbody2D rb;

    private Color baseClientColor;
    private SpriteRenderer sr;

    public Products command = Products.None;
    [SerializeField] private GameObject commandSign;
    private CommandSignManager commandSignManager;

    private AudioSource audio;
    [SerializeField] AudioClip moneyClip;

    // Start is called before the first frame update
    void Start()
    {
        commandSign = Instantiate(commandSign);
        commandSignManager = commandSign.GetComponent<CommandSignManager>();
        commandSign.SetActive(false);
        rb = GetComponent<Rigidbody2D>();
        sr = GetComponent<SpriteRenderer>();
        audio = GetComponent<AudioSource>();
        baseClientColor = sr.color;
    }

    private void OnEnable()
    {
        currentCommandState = CommandState.None;
        if (commandSignManager != null)
        {
            commandSignManager.switchTo(CommandSignManager.CommandSigns.Ready, Products.None);
        }
    }

    // Update is called once per frame
    void Update()
    {
        if (targetDestination != null)
        {
            Vector2 moveVector = new Vector2(targetDestination.transform.position.x - transform.position.x, 
                targetDestination.transform.position.y - transform.position.y).normalized;
            rb.velocity = moveVector * clientSpeed;

            if (isInZone(transform.position, targetDestination.transform.position))
            {
                if (this.targetDestination.tag == "Client Zone")
                {
                    ClientZoneManager zoneManager = targetDestination.GetComponent<ClientZoneManager>();
                    this.table = zoneManager.getLinkedTable();
                    this.table.GetComponent<TableManager>().setHasClient(true);
                    this.targetDestination = null;
                }

                advanceCommandState();
            }
        }
        else
        {
            rb.velocity = Vector2.zero;
            rb.isKinematic = true;
        }

        if (currentCommandState == CommandState.Waiting)
        {
            checkProductOnTable();
        }
        else if (currentCommandState == CommandState.Eating)
        {
            timeSinceStartedEating += Time.deltaTime;
            if (timeSinceStartedEating >= timeToEat)
            {
                advanceCommandState();
                timeSinceStartedEating = 0;
            }
        }
    }

    public bool isInZone(Vector2 client, Vector2 target)
    {
        if (client.x - target.x <= 0.15f && client.x - target.x >= -0.15f && client.y - target.y <= 0.15f && client.y - target.y >= -0.15f)
        {
            return true;
        }
        return false;
    }

    public void setTargetClientZone(GameObject clientZone)
    {
        this.targetDestination = clientZone;
    }

    public void setTalkable(bool isTalkable)
    {
        if (isTalkable)
        {
            sr.color = Color.yellow;
        }
        else
        {
            sr.color = baseClientColor;
        }
    }

    public Products getCommand()
    {
        command = GameManager.Instance.GetComponent<CommandsManager>().getRandomCommand();
        return command;
    }

    public bool isReadyToCommand()
    {
        if (currentCommandState == CommandState.Ready)
        {
            return true;
        }
        return false;
    }

    public void checkProductOnTable()
    {
        if (table != null)
        {
            Products tableProduct = table.GetComponent<TableManager>().getProductOnTable();
            if (tableProduct != Products.None)
            {
                if (tableProduct.Equals(command))
                {
                    this.commandSignManager.switchTo(CommandSignManager.CommandSigns.Happy, Products.None);
                    advanceCommandState();
                }
                else
                {
                    this.commandSignManager.switchTo(CommandSignManager.CommandSigns.Mad, Products.None);
                    if (!reducedMoney)
                    {
                        moneyForCommand = moneyForCommand / 2;
                        reducedMoney = true;
                    }
                }
            }
            else
            {
                this.commandSignManager.switchTo(CommandSignManager.CommandSigns.Waiting, command);
            }
        }
    }

    public void advanceCommandState()
    {
        switch(currentCommandState)
        {
            case CommandState.None:
                this.currentCommandState = CommandState.Ready;
                this.commandSign.transform.position = new Vector3(transform.position.x, transform.position.y + 1, 0);
                this.commandSign.SetActive(true);
                break;
            case CommandState.Ready:
                this.currentCommandState = CommandState.Waiting;
                audio.Play();
                break;
            case CommandState.Waiting:
                this.currentCommandState = CommandState.Eating;
                break;
            case CommandState.Eating:
                this.currentCommandState = CommandState.Completed;
                this.commandSign.SetActive(false);
                GameManager.Instance.GetComponent<CommandsManager>().removeCommand(command);
                GameManager.Instance.addMoney(moneyForCommand);
                audio.clip = moneyClip;
                audio.Play();
                this.table.GetComponent<TableManager>().setHasClient(false);
                this.table.GetComponent<TableManager>().changeProductOnTable(Products.Trash);
                this.targetDestination = GameObject.FindWithTag("Exit Zone");
                break;
            case CommandState.Completed:
                this.gameObject.SetActive(false);
                break;
        }
    }
}

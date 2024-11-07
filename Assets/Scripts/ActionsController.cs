using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Rendering;

public class ActionsController : MonoBehaviour
{
    private JimmyController controller;
    private GameObject currentUsableMachine = null;
    private GameObject currentUsableTrashcan = null;
    private GameObject currentMovableAndUsableTable = null;
    private GameObject currentTalkableClient = null;

    // Start is called before the first frame update
    void Start()
    {
        controller = GetComponent<JimmyController>();
    }

    // Update is called once per frame
    void Update()
    {
        if (currentUsableMachine != null)
        {
            if (currentUsableMachine.tag == "Machine")
            {
                manageMachine();
            }
        }

        if (currentUsableTrashcan != null)
        {
            manageTrashcan();
        }

        if (currentMovableAndUsableTable != null)
        {
            manageTable();
        }

        if (currentTalkableClient != null)
        {
            manageClient();
        }
    }

    private void manageMachine()
    {
        MachineManager manager = currentUsableMachine.GetComponent<MachineManager>();

        if (Input.GetButtonDown("Use"))
        {
            if (manager.useMachine())
            {
                controller.setCurrentProduct(manager.getMachineType());
            }
        }
    }

    private void manageTrashcan()
    {
        TrashcanManager manager = currentUsableTrashcan.GetComponent<TrashcanManager>();

        if (Input.GetButtonDown("Use"))
        {
            if (controller.getCurrentProduct() != Products.None)
            {
                controller.setCurrentProduct(Products.None);
            }
        }
    }

    private void manageTable()
    {
        TableManager manager = currentMovableAndUsableTable.GetComponent<TableManager>();

        if (Input.GetButtonDown("Use"))
        {
            switch(controller.getCurrentProduct())
            {
                case Products.None:
                    if (manager.getProductOnTable() != Products.None)
                    {
                        controller.setCurrentProduct(manager.getProductOnTable());
                        manager.changeProductOnTable(Products.None);
                    }
                    else
                    {
                        manager.moveTable();
                    }
                    break;
                default:
                    if (manager.getProductOnTable() == Products.None)
                    {
                        manager.changeProductOnTable(controller.getCurrentProduct());
                        controller.setCurrentProduct(Products.None);
                    }
                    break;
            }
        }

        if (Input.GetButtonUp("Use"))
        {
            manager.stopMovingTable();
        }
    }

    private void manageClient()
    {
        ClientManager manager = currentTalkableClient.GetComponent<ClientManager>();

        if (Input.GetButtonDown("Use"))
        {
            manager.getCommand();
            manager.advanceCommandState();
            manager.setTalkable(false);
        }
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.gameObject.tag == "Machine")
        {
            if (controller.getCurrentProduct() == Products.None)
            {
                if (currentUsableMachine != null)
                {
                    currentUsableMachine.GetComponent<MachineManager>().setMachineUsable(false);
                }
                currentUsableMachine = collision.gameObject;
                currentUsableMachine.GetComponent<MachineManager>().setMachineUsable(true);
            }
        }
    }

    private void OnTriggerExit2D(Collider2D collision)
    {
        if (collision.gameObject.tag == "Machine")
        {
            if (currentUsableMachine != null)
            {
                currentUsableMachine.GetComponent<MachineManager>().setMachineUsable(false);
                currentUsableMachine = null;
            }
        }
    }

    private void OnCollisionEnter2D(Collision2D collision)
    {
        if (collision.gameObject.tag == "Trashcan")
        {
            currentUsableTrashcan = collision.gameObject;
            currentUsableTrashcan.GetComponent<TrashcanManager>().setTrashcanUsable(true);
        }

        if (collision.gameObject.tag == "Table")
        {
            currentMovableAndUsableTable = collision.gameObject;
            currentMovableAndUsableTable.GetComponent<TableManager>().setMovableAndUsable(true);
        }

        if (collision.gameObject.tag == "Client")
        {
            if (collision.gameObject.GetComponent<ClientManager>().isReadyToCommand())
            {
                currentTalkableClient = collision.gameObject;
                currentTalkableClient.GetComponent<ClientManager>().setTalkable(true);
            }
        }
    }

    private void OnCollisionExit2D(Collision2D collision)
    {
        if (collision.gameObject.tag == "Trashcan")
        {
            if (currentUsableTrashcan != null)
            {
                currentUsableTrashcan.GetComponent<TrashcanManager>().setTrashcanUsable(false);
                currentUsableTrashcan = null;
            }
        }

        if (collision.gameObject.tag == "Table")
        {
            if (currentMovableAndUsableTable != null)
            {
                currentMovableAndUsableTable.GetComponent<TableManager>().stopMovingTable();
                currentMovableAndUsableTable.GetComponent<TableManager>().setMovableAndUsable(false);
                currentMovableAndUsableTable = null;
            }
        }

        if (collision.gameObject.tag == "Client")
        {
            if (currentTalkableClient != null)
            {
                currentTalkableClient.GetComponent<ClientManager>().setTalkable(false);
                currentTalkableClient = null;
            }
        }
    }
}

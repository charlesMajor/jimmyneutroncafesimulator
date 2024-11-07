using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ClientZoneManager : MonoBehaviour
{
    [SerializeField] private GameObject linkedTable;
    TableManager tableManager;
    private bool canReceiveClient = true; 

    // Start is called before the first frame update
    void Start()
    {
        tableManager = linkedTable.GetComponent<TableManager>();
    }

    // Update is called once per frame
    void Update()
    {
        if (linkedTable != null)
        {
            transform.position = new Vector3(linkedTable.transform.position.x + 0.7f, linkedTable.transform.position.y, 0);


            if (!tableManager.getHasClient() && tableManager.getProductOnTable() == Products.None)
            {
                canReceiveClient = true;
            }
            else
            {
                canReceiveClient = false;
            }
        }
    }

    public bool getCanReceiveClient()
    {
        return canReceiveClient;
    }

    public GameObject getLinkedTable()
    {
        return linkedTable;
    }
}

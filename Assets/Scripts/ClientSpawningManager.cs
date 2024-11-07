using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ClientSpawningManager : MonoBehaviour
{
    [SerializeField] private GameObject client;
    [SerializeField] private Sprite[] clientSprites;
    [SerializeField] private int maxClients = 5;
    private GameObject[] clientPool;
    private int currentClients = 0;
    private float clientSpawnTime = 5f;
    private float timeSinceLastClientSpawn = 0f;
    private Vector2 spawnPoint = new Vector2(8.04f, 0.17f);

    // Start is called before the first frame update
    void Start()
    {
        clientPool = new GameObject[maxClients];
        for (int i = 0; i < maxClients; i++)
        {
            clientPool[i] = Instantiate(client);
            clientPool[i].SetActive(false);
        }
    }

    // Update is called once per frame
    void Update()
    {
        timeSinceLastClientSpawn += Time.deltaTime;
        if (timeSinceLastClientSpawn >= clientSpawnTime)
        {
            spawnClient();
            timeSinceLastClientSpawn = 0;
        }
    }

    private void spawnClient()
    {
        currentClients = checkClientsAmount();

        if (currentClients < maxClients)
        {
            List<GameObject> possibleTables = checkAvailableTables();
            if (possibleTables.Count > 0)
            {
                GameObject clientToSpawn = getAvailableClient();
                clientToSpawn.transform.position = spawnPoint;

                int randomSprite = Random.Range(0, clientSprites.Length);
                clientToSpawn.GetComponent<SpriteRenderer>().sprite = clientSprites[randomSprite];

                int randomTable = Random.Range(0, possibleTables.Count);
                clientToSpawn.GetComponent<ClientManager>().setTargetClientZone(possibleTables[randomTable]);

                clientToSpawn.SetActive(true);
            }
        }
    }

    private GameObject getAvailableClient()
    {
        for (int i = 0; i < clientPool.Length; i++)
        {
            if (!clientPool[i].activeSelf)
                return clientPool[i];
        }
        return null;
    }

    private int checkClientsAmount()
    {
        GameObject[] clients = GameObject.FindGameObjectsWithTag("Client");
        return clients.Length;
    }

    private List<GameObject> checkAvailableTables()
    {
        GameObject[] clientZones = GameObject.FindGameObjectsWithTag("Client Zone");

        List<GameObject> possibleTables = new List<GameObject>();
        foreach (GameObject clientZone in clientZones) 
        {
            if (clientZone.GetComponent<ClientZoneManager>().getCanReceiveClient())
            {
                possibleTables.Add(clientZone);
            }
        }
        return possibleTables;
    }
}

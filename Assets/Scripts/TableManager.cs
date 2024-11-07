using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.Rendering;

public class TableManager : MonoBehaviour
{
    private Products productOnTable = Products.None;
    private bool isMoving = false;
    private Rigidbody2D rb;

    private SpriteRenderer sr;
    [SerializeField] private Sprite baseSprite;
    [SerializeField] private Sprite withCoffeeSprite;
    [SerializeField] private Sprite withDonutSprite;
    [SerializeField] private Sprite withTrashSprite;
    private Color baseTableColor;

    private bool hasClient = false;

    // Start is called before the first frame update
    void Start()
    {
        rb = GetComponent<Rigidbody2D>();
        sr = GetComponent<SpriteRenderer>();
        sr.sprite = baseSprite;
        baseTableColor = sr.color;
    }

    // Update is called once per frame
    void Update()
    {
        rb.isKinematic = !isMoving;
    }

    public void moveTable()
    {
        if (!hasClient)
        {
            isMoving = true;
            sr.color = Color.green;
        }
        else
        {
            stopMovingTable();
        }
    }

    public void stopMovingTable()
    {
        isMoving = false;
        sr.color = Color.yellow;
        rb.velocity = Vector2.zero;
    }

    public void setMovableAndUsable(bool isMovable)
    {
        if (isMovable)
        {
            sr.color = Color.yellow;
        }
        else
        {
            sr.color = baseTableColor;
        }
    }

    public void setHasClient(bool hasClient)
    {
        this.hasClient = hasClient;
    }

    public bool getHasClient()
    {
        return this.hasClient;
    }

    public void changeProductOnTable(Products product)
    {
        switch (product)
        {
            case Products.None:
                sr.sprite = baseSprite;
                break;
            case Products.Trash:
                sr.sprite = withTrashSprite;
                break;
            case Products.Coffee:
                sr.sprite = withCoffeeSprite;
                break;
            case Products.Donut:
                sr.sprite = withDonutSprite;
                break;
        }
        sr.color = baseTableColor;
        productOnTable = product;
    }

    public Products getProductOnTable()
    {
        return productOnTable;
    }
}

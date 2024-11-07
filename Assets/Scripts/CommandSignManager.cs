using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CommandSignManager : MonoBehaviour
{
    public enum CommandSigns { Ready, Waiting, Happy, Mad };
    [SerializeField] private Sprite readySprite;
    [SerializeField] private Sprite waitingCoffeeSprite;
    [SerializeField] private Sprite waitingDonutSprite;
    [SerializeField] private Sprite happySprite;
    [SerializeField] private Sprite madSprite;
    private SpriteRenderer sr;

    // Start is called before the first frame update
    void Start()
    {
        sr = GetComponent<SpriteRenderer>();
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void switchTo(CommandSigns sign, Products product)
    {
        switch (sign)
        {
            case CommandSigns.Ready:
                sr.sprite = readySprite;
                break;
            case CommandSigns.Waiting:
                switch (product)
                {
                    case Products.Coffee:
                        sr.sprite = waitingCoffeeSprite;
                        break;
                    case Products.Donut:
                        sr.sprite = waitingDonutSprite;
                        break;
                }
                break;
            case CommandSigns.Happy:
                sr.sprite = happySprite;
                break;
            case CommandSigns.Mad:
                sr.sprite = madSprite;
                break;
        }
    }
}

using System.Collections;
using System.Collections.Generic;
using System.Runtime.CompilerServices;
using UnityEngine;

public class JimmyController : MonoBehaviour
{
    private Vector3 inputsVector;
    private Vector3 moveVector;
    private float jimmySpeed = 5f;
    private Rigidbody2D rb;

    private Products currentProduct = Products.None;
    private JimmyAnimator animator;

    private bool canRoll;
    private float timeBetweenRolls = 1.5f;
    private float timeSinceLastRoll = 0;
    private bool isInRollingAnimation = false;
    private Vector3 directionInRoll;

    private AudioSource audio;

    // Start is called before the first frame update
    void Start()
    {
        rb = GetComponent<Rigidbody2D>();
        animator = GetComponent<JimmyAnimator>();
        audio = GetComponent<AudioSource>();
        timeSinceLastRoll = timeBetweenRolls;
    }

    // Update is called once per frame
    void Update()
    {
        buildMovement();
        invert();
    }

    private void buildMovement()
    {
        timeSinceLastRoll += Time.deltaTime;
        if (timeSinceLastRoll >= timeBetweenRolls && currentProduct == Products.None)
        {
            canRoll = true;
        }
        else
        {
            canRoll = false;
        }

        inputsVector = new Vector2(Input.GetAxisRaw("Horizontal"), Input.GetAxisRaw("Vertical")).normalized;

        if (isInRollingAnimation)
        {
            moveVector =  directionInRoll * 2.5f;
        }
        else
        {
            moveVector = inputsVector;
        }
        rb.velocity = moveVector * jimmySpeed;

        if (Input.GetAxisRaw("Horizontal") != 0 || Input.GetAxisRaw("Vertical") != 0)
        {
            if (Input.GetAxisRaw("Roll") != 0 && canRoll)
            {
                audio.Play();
                isInRollingAnimation = true;
                timeSinceLastRoll = 0;
                directionInRoll = inputsVector;
            }

            if (!isInRollingAnimation)
            {
                animator.animateRun(currentProduct);
            }

        }
        else if (!isInRollingAnimation)
        {
            animator.stopped(currentProduct);
        }

        if (isInRollingAnimation)
        {
            isInRollingAnimation = animator.animateRoll();
        }
    }

    private void invert()
    {
        SpriteRenderer sprite = GetComponent<SpriteRenderer>();

        float horizontal = Input.GetAxisRaw("Horizontal") * Time.deltaTime;
        if (horizontal < 0)
        {
            sprite.flipX = true;
        }
        else if (horizontal > 0)
        {
            sprite.flipX = false;
        }
    }

    public float getTimeUntilNextRoll()
    {
        float timeUntilNextRoll = timeBetweenRolls - timeSinceLastRoll;
        if (timeUntilNextRoll < 0)
        {
            timeUntilNextRoll = 0;
        }

        if (currentProduct != Products.None)
        {
            timeUntilNextRoll = timeBetweenRolls;
        }

        return timeUntilNextRoll;
    }

    public float getTimeBetweenRolls()
    {
        return timeBetweenRolls;
    }

    public Products getCurrentProduct()
    {
        return this.currentProduct;
    }

    public void setCurrentProduct(Products product)
    {
        this.currentProduct = product;
    }
}

using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MachineManager : MonoBehaviour
{
    [SerializeField] private Products machineType;

    private bool isMakingProduct = false;
    private bool hasProductReady = false;
    private float timeToMakeProduct = 2f;
    private float timeSinceProductStarted = 0f;

    private float machineBaseHeight;
    private float machineMaxHeight;
    private float currentMachineHeight;
    private bool isGrowing = true;
    private Color baseMachineColor;

    private SpriteRenderer sr;

    private AudioSource audio;
    private bool playedSound = false;

    // Start is called before the first frame update
    void Start()
    {
        sr = GetComponent<SpriteRenderer>();
        audio = GetComponent<AudioSource>();

        machineBaseHeight = transform.lossyScale.y;
        machineMaxHeight = machineBaseHeight + 0.05f;
        currentMachineHeight = machineBaseHeight;
        baseMachineColor = sr.color;
    }

    // Update is called once per frame
    void Update()
    {
        manageMakingProductAnimation();
    }

    private void manageMakingProductAnimation()
    {
        if (isMakingProduct)
        {
            setMachineUsable(true);
            if (isGrowing)
            {
                currentMachineHeight += 0.0005f;
            }
            else
            {
                currentMachineHeight -= 0.0005f;
            }

            if (currentMachineHeight >= machineMaxHeight)
            {
                isGrowing = false;
            }
            else if (currentMachineHeight <= machineBaseHeight)
            {
                isGrowing = true;
            }

            timeSinceProductStarted += Time.deltaTime;
            if (timeSinceProductStarted >= timeToMakeProduct)
            {
                isMakingProduct = false;
                sr.color = Color.green;
                timeSinceProductStarted = 0f;
                hasProductReady = true;
                
                if (!playedSound)
                {
                    audio.Play();
                    playedSound = true;
                }
            }
        }
        else
        {
            currentMachineHeight = machineBaseHeight;
        }

        transform.localScale = new Vector3(transform.lossyScale.x, currentMachineHeight);
    }

    public void setMachineUsable(bool isUsable)
    {
        if (!hasProductReady)
        {
            if (isUsable)
            {
                sr.color = Color.yellow;
            }
            else
            {
                sr.color = baseMachineColor;
            }
            
        }
    }

    public bool useMachine()
    {
        if (hasProductReady)
        {
            hasProductReady = false;
            setMachineUsable(false);
            return true;
        }
        else
        {
            isMakingProduct = true;
            playedSound = false;
            return false;
        }
    }

    public Products getMachineType()
    {
        return this.machineType;
    }
}

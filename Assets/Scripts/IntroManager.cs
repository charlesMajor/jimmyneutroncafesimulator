using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.SceneManagement;

public class IntroManager : MonoBehaviour
{
    [SerializeField] private TMP_Text startText;
    private float timeBetweenToggle = 0.5f;
    private float timeSinceLastToggle = 0f;

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        timeSinceLastToggle += Time.deltaTime;
        if (timeSinceLastToggle >= timeBetweenToggle)
        {
            if (startText.IsActive())
            {
                startText.enabled = false;
            }
            else
            {
                startText.enabled = true;
            }
            timeSinceLastToggle = 0f;
        }

        manageStart();
    }

    private void manageStart()
    {
        if (Input.GetButtonDown("Roll"))
        {
            SceneManager.LoadScene("MainScene");
        }
    }
}

using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.SceneManagement;

public class EndManager : MonoBehaviour
{
    [SerializeField] private TMP_Text restartText;
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
            if (restartText.IsActive())
            {
                restartText.enabled = false;
            }
            else
            {
                restartText.enabled = true;
            }
            timeSinceLastToggle = 0f;
        }

        manageRestart();
    }

    private void manageRestart()
    {
        if (Input.GetButtonDown("Roll"))
        {
            SceneManager.LoadScene("IntroScene");
        }
    }
}

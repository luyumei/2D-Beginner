using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class NonPlayerCharacter : MonoBehaviour
{
    public float displayTime = 4.0f; // store how long in seconds our dialog box is displayed
    public GameObject dialogBox; // store the Canvas GameObject, so you can enable/disable it in the script
    float timerDisplay; // will store how long to display our dialog
    
    // Start is called before the first frame update
    void Start()
    {
        dialogBox.SetActive(false); // make sure that the dialogBox is disabled
        timerDisplay = -1.0f;
    }

    // Update is called once per frame
    void Update()
    {
        if (timerDisplay >= 0) // check whether the dialog is currently displayed by testing if timerDisplay is superior or equal to 0
                               // If it is greater than zero, then the dialog is currently being displayed
        {
            timerDisplay -= Time.deltaTime; //decrease Time.deltaTime to count down and then check if your timerDisplay has reached 0
            if (timerDisplay < 0) // This means it’s time to hide your dialog box again, so you will need to disable the dialog box
            {
                dialogBox.SetActive(false);
            }
        }
    }
    
    public void DisplayDialog() // RubyController will call when Ruby interacts with the NPC frog
                                // This function will show the dialog box and initialize the timeDisplay to the displayTime setup
    {
        timerDisplay = displayTime;
        dialogBox.SetActive(true);
    }
}

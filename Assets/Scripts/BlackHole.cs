using MilkShake;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BlackHole : MonoBehaviour
{
    public bool biggerBox;
    public bool isExit;

    public ShakePreset shakePreset;
    public GameObject panel;
    public AudioSource audioSource1;
    public AudioSource audioSource2;
    public static bool blackLock = false;

    private void Awake()
    {
        if (Manager.isBlack)
        {
            panel.SetActive(true);
            StartCoroutine(StartSequence());
        }

    }


    private void OnTriggerEnter(Collider other)
    {
        //its first going to lock the controls, then load the black hole scene


        //if its the bigger collider, it will lock the controls
        //of the spaceship by setting blackLock to true 
        //and resets the status of the spaceship
        if (biggerBox)
        {
            blackLock = true;
            SpaceshipScript.lockControls = true;
            SpaceshipScript.autoCorrect = true;
            SpaceshipScript.isRotating = false;
        }
        //if its the smaller collider, it will turn 
        //the boolean turnBlack to true and will
        //unlock the controls of the spaceship
        else if (!isExit)
        {
            Manager.turnBlack = true;
            blackLock = false;
        }

        //if this is the exit collider, it will start the exit
        //sequence
        if (isExit)
        {
            StartCoroutine(ExitSequence());
        }
    }



    public IEnumerator ExitSequence()
    {
        //starts a screen shake effect
        Shaker.ShakeAll(shakePreset);
        //plays an earth quake sound effect
        audioSource1.Play();
        //allows the effects to play for a bit
        yield return new WaitForSeconds(4f);
        //shows the panel to cover the screen
        //for 1 second
        panel.SetActive(true);
        yield return new WaitForSeconds(1f);
        //sets goBack to true in the manager
        Manager.goBack = true;
    }


    public IEnumerator StartSequence()
    {
        //waits for 2 seconds for extra tension
        yield return new WaitForSeconds(2f);
        //plays a suspense sound effect
        audioSource2.Play();
        //waits for a small amount due to the sound taking
        //a bit to start
        yield return new WaitForSeconds(0.01f);
        //hides the black panel that was covering the screen
        panel.SetActive(false);

    }
}

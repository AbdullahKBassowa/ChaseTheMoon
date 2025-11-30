using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SpaceshipScript : MonoBehaviour
{
    //invincibilty for debugging
    [Header("Check this for becoming invincible (only for testing purposes)")]
    public bool Invincible;




    private GameObject spaceship;
    private CharacterController spaceshipCC;


    public float speed;
    [HideInInspector]
    public static float speedClone;
    [Header("Boost settings (boost amount must be divisble by 4)")]
    public float boostAmount;
    public float boostDuration;

    private float rotationOrigin = 0f;
    private float rotationAdd;
    private float rotationSubtract;
    private float rotationFlip;
    private float rotationFlipClone;
    [Header("How fast the player can tilt")]
    public float rotationRate = 4f;
    private float rotationRateClone;


    [Header("Booster settings")]
    public Material normalColor;
    public Material boostBoxColor;
    private Material normalColorClone;
    public Material boostColor;
    public Material doubleBoostColor;
    public ParticleSystem burst1;
    public ParticleSystem burst2;


    public static bool autoCorrect = true;
    public static bool lockControls = false;
    public static bool isRotating;
    [HideInInspector]
    public static bool isFlipping;
    public static bool allowFlip = false;
    private bool AllowBoost = false;
    private bool stopBurst = false;


    //pausing
    private bool pauseSwitching = false;
    private bool playSound1 = false;
    private bool playSound2 = false;


    //score
    public static int score;
    /*
    
    Make the GLOWING SPITTING cube increase in volume as you go faster (white noise)

     */












    void Start()
    {
        //plain old setting the objects
        spaceship = gameObject;
        spaceshipCC = gameObject.GetComponent<CharacterController>();

        speedClone = speed;
        rotationRateClone = rotationRate;
        rotationFlipClone = rotationFlip;
        normalColorClone = normalColor;
        


        //setting the speed values to the collisions script so i can enable invincibility
        Collisions.spaceshipSpeedLimit = speedClone + boostAmount;
        Collisions.isInvincible = Invincible;
    }





    private void FixedUpdate()
    {
        if (Manager.isBlack)
        {
            //black hole backwards
            spaceshipCC.Move(speed * (spaceship.transform.forward * -1) * Time.deltaTime);
        }
        else
        {
            //This is the direction of constant movement of the spaceship and its rotation.
            spaceshipCC.Move(speed * spaceship.transform.forward * Time.deltaTime);
        }
        //this constantly updates the speed of the spaceship to the collisions script.
        Collisions.spaceshipSpeed = speed;







        /*This is the constant movement. If the player isnt flipping, they move according to rotationAdd and rotationSubtract which
         are determined by the keys S and W. If they are flipping (when spacebar is pressed), the player moves according to rotationFlip which
        controls the speed of the flip.*/
        if (isFlipping)
        {
            spaceship.transform.rotation = Quaternion.Euler(Mathf.Clamp(rotationOrigin += rotationAdd + rotationSubtract + rotationFlip, -360, 60), 0, 0);
        }
        else
        {
            rotationOrigin = Mathf.Clamp(rotationOrigin, -60, 60);
            spaceship.transform.rotation = Quaternion.Euler(Mathf.Clamp(rotationOrigin += rotationAdd + rotationSubtract, -60, 60), 0, 0);
        }








        //new score system

        //calculate percentage increase in speed
        float percIncr = (speed / speedClone * 100) - 100;
        //i added the 0.1 to cheat a little, so that going fast actually gives more score.
        score += Convert.ToInt32((1 * 1 + (percIncr / 100)) + 0.1);


        //reseting the score
        if (Manager.isMain)
        {
            score = 0;
        }









        //this was for debugging purposed (talk about it)
        //Debug.Log($"Float value:{(1 * 1 + (percIncr / 100)) + 0.1} Int value: {Convert.ToInt32((1 * 1 + (percIncr / 100)) + 0.1)}");
        /*old score system (talk about it)
        if (speed == speedClone)
        {
            score++;
        }
        else if (speed == speedClone + boostAmount)
        {
            score += 2;
        }
        else if (speed >= speedClone + (boostAmount * 2))
        {
            score += 4;
        }
        else if (Manager.isMain)
        {
            score = 0;
        }*/


    }





    // Update is called once per frame
    void Update()
    {
        //pausing

        //this is so you dont have to click esc twice
        if (!Manager.isPaused)
        {
            pauseSwitching = false;
        }
        
        if (Input.GetKeyDown(KeyCode.Escape) && !pauseSwitching && !Manager.isMain)
        {
            pauseSwitching = true;
            Manager.isPaused = true;
            FindObjectOfType<Audio>().Play("Click");
        }
        else if (Input.GetKeyDown(KeyCode.Escape) && pauseSwitching && !Manager.isMain)
        {
            pauseSwitching = false;
            Manager.isPaused = false;
            FindObjectOfType<Audio>().Play("Click");
        }










        //for new boost box
        boostBoxColor.color = new Color(normalColor.color.r, normalColor.color.g, normalColor.color.b, 0.4f);

        //colors
        if (speed == speedClone)
        {
            normalColor.SetColor("_EmissionColor", normalColorClone.color * 4);
            //boostbox
            boostBoxColor.color = new Color(0,0.8f, 1, 0.4f);
            rotationRate = rotationRateClone;
        }
        if(speed == speedClone + boostAmount)
        {
            normalColor.SetColor("_EmissionColor", boostColor.color * 5);
            //boostbox
            boostBoxColor.color = new Color(1, 0.3f, 0, 0.4f);
            rotationRate = rotationRateClone / 2;




            if (playSound1)
            {
                FindObjectOfType<Audio>().Play("Orange");
                playSound1 = false;
            }
        }
        if(speed == speedClone + (boostAmount * 2))
        {
            normalColor.SetColor("_EmissionColor", doubleBoostColor.color * 5);
            //boostbox
            boostBoxColor.color = new Color(1, 1, 1, 0.4f);
            rotationRate = rotationRateClone / 3;




            if (playSound2)
            {
                FindObjectOfType<Audio>().Play("White");
                playSound2 = false;
            }
        }








        //Allows the player to do a full flip
        if(rotationOrigin <= -360)
        {
            spaceship.transform.rotation = Quaternion.Euler(0, 0, 0);
            rotationOrigin = 0;
            rotationFlip = 0;
        }






        //Forces the ship to do a flip if you press spacebar
        if(Input.GetKey(KeyCode.Space) && allowFlip && !isFlipping && !Manager.isMain)
        {
            autoCorrect = false;
            lockControls = true;
            isFlipping = true;
            AllowBoost= true;
            allowFlip = false;


            if(speed == speedClone)
            {
                playSound1 = true;
            }
            else if(speed == speedClone + boostAmount)
            {
                playSound2 = true;
            }
            

            
            
            rotationFlip = rotationRateClone * -1;



            //making the flipping faster
            if (speed == speedClone + boostAmount /*orange level*/)
            {
                rotationFlip *= 1.5f;
            }
            if (speed >= speedClone + (boostAmount * 2) /*white level level*/)
            {
                rotationFlip *= 2;
            }





            spaceship.transform.rotation = Quaternion.Euler(spaceship.transform.rotation.x + Mathf.Clamp(rotationOrigin += rotationAdd + rotationSubtract + rotationFlip, -360, 60), 0, 0);

            rotationAdd = 0;
            rotationSubtract = 0;

        }






        //Boost after flip
        if(AllowBoost && spaceship.transform.rotation.x == 0)
        {
            speed += boostAmount;
            AllowBoost = false;
            StartCoroutine(Boost());
        }















        //If you press S, the rotationAdd will be equal to the rotation rate and will rotate the spaceship down.
        if (!lockControls)
        {
            if ((Input.GetKey(KeyCode.S) || Input.GetKey(KeyCode.DownArrow)) && !Manager.isMain)
            {
                isRotating = true;
                rotationAdd = rotationRate;
            }
            else
            {
                rotationAdd = 0;
            }

        }

        //If you press W, the rotationAdd will be equal to the rotation rate * -1 and will rotate the spaceship up.
        if (!lockControls)
        {
            if ((Input.GetKey(KeyCode.W) || Input.GetKey(KeyCode.UpArrow)) && !Manager.isMain)
            {
                isRotating = true;
                rotationSubtract = rotationRate * -1;
            }
            else
            {
                rotationSubtract = 0;
            }
        }

        // If you arent tilting, then the bool isRotating is false.
        if(!Input.GetKey(KeyCode.S) && !Input.GetKey(KeyCode.W) && !Input.GetKey(KeyCode.UpArrow) && !Input.GetKey(KeyCode.DownArrow)) 
        {
            isRotating = false;
            rotationAdd = 0;
            rotationSubtract = 0;
        }
















        //unlocks the auto correction and controls if the spaceship is facing normally
        //and blackLock is false
        if (spaceship.transform.rotation.x == 0 && !BlackHole.blackLock)
        {
            lockControls = false;
            autoCorrect = true;
            isFlipping = false;

        }




        //auto correction
        if (autoCorrect)
        {

            if (!isRotating)
            {
                if(rotationOrigin < 6 && rotationOrigin > 0)
                {
                    rotationOrigin -= 1;
                }
                else if(rotationOrigin > -6 && rotationOrigin < 0)
                {
                    rotationOrigin += 1;
                }
                else if (rotationOrigin > 0)
                {
                    rotationOrigin -= rotationRate;
                }
                else if (rotationOrigin < 0)
                {
                    rotationOrigin += rotationRate;
                }

            }
        }
        

    }

 






    private IEnumerator Boost()
    {
        burst1.Play();
        burst2.Play();
        //x determines across how many iterations should the FOV be
        //changed (i.e how smooth should the zoom out/in be)
        int x = 10;

        //This divides the amount of FOV to be added by x so it can 
        //be added gradually
        float levels = (CameraMovement.FOVb - CameraMovement.FOV) / x;


        //The FOV gets increased gradually over x iterations
        for (int i = 0; i < x; i++)
        {
            CameraMovement.FOV += levels;
            //this is here so the FOV doesn't increase instantly
            yield return new WaitForSeconds(0.00015f);
        }

        //for how long should the FOV be increased
        yield return new WaitForSeconds(0.5f);

        for(int i = 0; i < x; i++)
        {
            CameraMovement.FOV -= levels;
            //this is here so the FOV doesn't decrease instantly
            yield return new WaitForSeconds(0.0015f);
        }







        //The wait for the boost
        if (speed >= speedClone + (boostAmount * 2))
        {
            yield return new WaitForSeconds(boostDuration * 2);
        }
        else
        {
            yield return new WaitForSeconds(boostDuration);
        }





        for (int i = 0; i < 4; i++)
        {
            speed -= boostAmount / 4;
            yield return new WaitForSeconds(0.005f);
        }

    }

}

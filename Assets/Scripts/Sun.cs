using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using MilkShake;

public class Sun : MonoBehaviour
{
    public GameObject lavaRod;
    public Transform lowerRange;
    public Transform upperRange;

    [Header("Offests")]
    public float X_Offset;
    public float Y_Offset;

    private System.Random rand = new System.Random();


    private bool switcher = false;
    private bool seize = false;



    //shake
    public ShakePreset shakePreset;


    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Spaceship") && !SpaceshipScript.isFlipping)
        {
            switcher = true;
        }
    }

    private void OnTriggerExit(Collider other)
    {
        if (other.CompareTag("Spaceship") && !SpaceshipScript.isFlipping)
        {
            seize = true;
        }
    }



    private void Update()
    {
        if (switcher && !seize)
        {
            switcher = false;
            StartCoroutine(SpawnRod2());
        }
    }




    public void SpawnRod()
    {
        StartCoroutine(SpawnRod2());
    }




    public IEnumerator SpawnRod2()
    {
        int spawnPos = rand.Next(Convert.ToInt32(lowerRange.position.z), Convert.ToInt32(upperRange.position.z));
        GameObject momObj = Instantiate(lavaRod, new Vector3(gameObject.transform.position.x + X_Offset, gameObject.transform.position.y + Y_Offset, spawnPos), Quaternion.Euler(0, 0, 0));

        //camera shake
        Shaker.ShakeAll(shakePreset);

        //Audio
        FindObjectOfType<Audio>().Play("SunRod");

        yield return new WaitForSeconds(1f);

        
        switcher = true;
        
        
        //this is really smart
        //I knew that this function would get called over and over when switcher is true, so i didnt make a new function, but just made it destroy the object
        //after a few seconds WHILE this function gets called again and again
        yield return new WaitForSeconds(3f);
        Destroy(momObj);
    }
}

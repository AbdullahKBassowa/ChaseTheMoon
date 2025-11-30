using System;
using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEditor;
using UnityEngine;

public class ObstacleManager : MonoBehaviour
{
    [Header("Space Junk Sets")]
    public GameObject SpaceJunk1;
    public GameObject SpaceJunk2;
    public GameObject SpaceJunk3;


    [Header("Asteroid sets")]
    public GameObject AsteroidSet1;
    public GameObject AsteroidSet2;
    public GameObject AsteroidSet3;


    [Header("Sun sets")]
    public GameObject SunSet1;
    public GameObject SunSet2;
    public GameObject SunSet3;

    [Header("Black hole set")]
    public GameObject BlackHoleSet;

    [Header("Render settings")]
    public Color FogColor;
    public Color AsteroidColor;
    public Color SunColor;
    public Color AfterColor;

    public static int UniversalCounter;

    System.Random rand = new System.Random();



    void Update()
    {
        FogColor = RenderSettings.fogColor;

        if (Manager.isMain || Manager.isBlack)
        {
            UniversalCounter = 0;
        }

    }







    public void SpawnNextAsteroidSet(GameObject SpawnPoint, int set)
    {
        int blackChance = 25;




        if (UniversalCounter < 8)
        {
            //picking randomly between asteroid sets
            switch (set)
            {
                case 1:
                    Instantiate(SpaceJunk1, SpawnPoint.transform.position, Quaternion.Euler(0, 0, 0));
                    break;
                case 2:
                    Instantiate(SpaceJunk2, SpawnPoint.transform.position, Quaternion.Euler(0, 0, 0));
                    break;
                case 3:
                    Instantiate(SpaceJunk3, SpawnPoint.transform.position, Quaternion.Euler(0, 0, 0));
                    break;
            }


        }
        else if (UniversalCounter < 16)
        {

            //random chance of spawning black hole is 1 in every 25 zones
            int i = rand.Next(1, blackChance);

            if (i == 1)
            {
                Instantiate(BlackHoleSet, SpawnPoint.transform.position, Quaternion.Euler(0, 0, 0));
            }
            else
            {
                switch (set)
                {
                    case 1:
                        Instantiate(AsteroidSet1, SpawnPoint.transform.position, Quaternion.Euler(0, 0, 0));
                        break;
                    case 2:
                        Instantiate(AsteroidSet2, SpawnPoint.transform.position, Quaternion.Euler(0, 0, 0));
                        break;
                    case 3:
                        Instantiate(AsteroidSet3, SpawnPoint.transform.position, Quaternion.Euler(0, 0, 0));
                        break;
                }
            }

        }
        else if (UniversalCounter < 24)
        {
            int i = rand.Next(1, blackChance);

            if (i == 1)
            {
                Instantiate(BlackHoleSet, SpawnPoint.transform.position, Quaternion.Euler(0, 0, 0));
            }
            else
            {
                switch (set)
                {
                    case 1:
                        Instantiate(SunSet1, SpawnPoint.transform.position, Quaternion.Euler(0, 0, 0));
                        break;
                    case 2:
                        Instantiate(SunSet2, SpawnPoint.transform.position, Quaternion.Euler(0, 0, 0));
                        break;
                    case 3:
                        Instantiate(SunSet3, SpawnPoint.transform.position, Quaternion.Euler(0, 0, 0));
                        break;
                }
            }

        }
        else
        {
            int i = rand.Next(1, blackChance);

            if (i == 1)
            {
                Instantiate(BlackHoleSet, SpawnPoint.transform.position, Quaternion.Euler(0, 0, 0));
            }
            else
            {
                //randomness for whoever gets that far lol
                int picker = rand.Next(1, 10);
                switch (picker)
                {
                    case 1:
                        Instantiate(SpaceJunk1, SpawnPoint.transform.position, Quaternion.Euler(0, 0, 0));
                        break;
                    case 2:
                        Instantiate(SpaceJunk2, SpawnPoint.transform.position, Quaternion.Euler(0, 0, 0));
                        break;
                    case 3:
                        Instantiate(SpaceJunk3, SpawnPoint.transform.position, Quaternion.Euler(0, 0, 0));
                        break;
                    case 4:
                        Instantiate(AsteroidSet1, SpawnPoint.transform.position, Quaternion.Euler(0, 0, 0));
                        break;
                    case 5:
                        Instantiate(AsteroidSet2, SpawnPoint.transform.position, Quaternion.Euler(0, 0, 0));
                        break;
                    case 6:
                        Instantiate(AsteroidSet3, SpawnPoint.transform.position, Quaternion.Euler(0, 0, 0));
                        break;
                    case 7:
                        Instantiate(SunSet3, SpawnPoint.transform.position, Quaternion.Euler(0, 0, 0));
                        break;
                    case 8:
                        Instantiate(SunSet3, SpawnPoint.transform.position, Quaternion.Euler(0, 0, 0));
                        break;
                    case 9:
                        Instantiate(SunSet3, SpawnPoint.transform.position, Quaternion.Euler(0, 0, 0));
                        break;
                }
            }

        }


        UniversalCounter++;
        FindObjectOfType<ObstacleManager>().StartCoroutine(ZeroToOne(UniversalCounter));
    }


    public IEnumerator ZeroToOne(int counter)
    {
        Debug.Log("I, zero to one, have started");
        Debug.Log($"My counter, his level is {counter}");
        for (float i = 0; i <= 1; i += 0.01f)
        {
            if (counter == 10)
            {
                RenderSettings.fogColor = Color.Lerp(Color.black, AsteroidColor, i);
            }
            if (counter == 18)
            {
                RenderSettings.fogColor = Color.Lerp(AsteroidColor, SunColor, i);
            }
            if (counter == 26)
            {
                RenderSettings.fogColor = Color.Lerp(SunColor, AfterColor, i);
            }




            yield return new WaitForSeconds(0.04f);
        }
    }
}

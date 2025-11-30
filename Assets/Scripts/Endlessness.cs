using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class Endlessness : MonoBehaviour
{
    public GameObject SpawnPoint;

    /*object refrence to the 
    obstacle manager script
    so i can call the function*/
    public ObstacleManager m;

    private int set;
    private System.Random Rand = new System.Random();




    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        set = Rand.Next(1, 4);
    }



    private void OnTriggerEnter(Collider other)
    {

        if (other.CompareTag("Spaceship") && !SpaceshipScript.isFlipping)
        {
            m.SpawnNextAsteroidSet(SpawnPoint, set);
        }

        
    }




    private void OnTriggerExit(Collider other)
    {
        if (other.CompareTag("Spaceship") && !SpaceshipScript.isFlipping)
        {
            Destroy(gameObject, 2f);
        }

    }


}

using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Collisions : MonoBehaviour
{
    public static float spaceshipSpeed;
    public static float spaceshipSpeedLimit;

    //This is for the boost orb, which will allow
    //me to re-use this collisions script instead
    //of making a whole new script
    public bool isOrb = false;



    public static bool isInvincible;




    /*private void Awake()
    {
        if (!isOrb)
        {
            gameObject.AddComponent<ParticleSystem>();
        }
    }*/
    private void OnTriggerEnter(Collider other)
    {
        if (isOrb && other.CompareTag("Spaceship"))
        {

            gameObject.GetComponent<Animator>().SetBool("Break", true);
            gameObject.GetComponent<AudioSource>().Play();
            GetComponent<ParticleSystem>().Stop();
            Destroy(gameObject, 0.5f);
            SpaceshipScript.allowFlip = true;
        }
        //destroys the spaceship
        else if (other.CompareTag("Spaceship") && spaceshipSpeed < spaceshipSpeedLimit && !isInvincible)
        {
            FindObjectOfType<Audio>().Play("Death");
            Destroy(other.gameObject);

            Manager.isGameOver = true;
        }
        //destroys the obstacle
        else if (other.CompareTag("Spaceship") && spaceshipSpeed >= spaceshipSpeedLimit && !isInvincible)
        {
            FindObjectOfType<Audio>().Play("Hit");
            Destroy(gameObject);
        }
    }

    private void OnCollisionEnter(Collision collision)
    {
        if (collision.gameObject.CompareTag("Spaceship") && spaceshipSpeed < spaceshipSpeedLimit && !isInvincible)
        {
            Destroy(collision.gameObject);
            Manager.isGameOver = true;
        }
    }
}

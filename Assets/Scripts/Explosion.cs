using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using MilkShake;

public class Explosion : MonoBehaviour
{
    private ParticleSystem particles;
    private Animator animator;
    private AudioSource aSource;

    //shake
    public ShakePreset shakePreset;


    // Start is called before the first frame update
    void Start()
    {
        //assigns all the components of the object to these variables
        particles = gameObject.GetComponent<ParticleSystem>();
        animator = gameObject.GetComponent<Animator>();
        aSource = gameObject.GetComponent<AudioSource>();
    }



    private void OnTriggerEnter(Collider other)
    {
        /*when the spaceship enters its collider, it plays the particle
         effect. sets the Explode variable to true to start the animation,
        plays an explosion sound effect and shakes the camera. It then
        destroys the bomb after 10 seconds */
        if (other.CompareTag("Spaceship"))
        {
            particles.Play();
            animator.SetBool("Explode", true);
            aSource.Play();
            Shaker.ShakeAll(shakePreset);

            Destroy(gameObject, 10f);
        }
    }


}

using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SineMovement : MonoBehaviour
{
    private CharacterController Mover;

    public float TimeInterval = 0;
    public float Delay = 0;
    public float speed = 0;


    //this is a lazy fix to the issue of the corridor asteroids just getting higher and higher
    private float lazyFix = -50.23265f;
    [Header("Check for boost orbs")]
    public bool noLazyFix = false;
    private float sinValue = 0;
    private System.Random random = new System.Random();
    private bool constantSin = true;
    private bool delayOnce = true;




    void Awake()
    {
        Mover = gameObject.GetComponent<CharacterController>();
        Mover.height = 0;
        Mover.radius = 0;
        constantSin = true;
        delayOnce = true;


        //part of my lazy fix
        if (noLazyFix)
        {
            gameObject.transform.position = new Vector3(gameObject.transform.position.x, gameObject.transform.position.y, gameObject.transform.position.z);
        }
        else
        {
            gameObject.transform.position = new Vector3(gameObject.transform.position.x, lazyFix, gameObject.transform.position.z);
        }
        
    }

    // Update is called once per frame
    void Update()
    {


        if (constantSin)
        {
            StartCoroutine(SineWave());
            constantSin = false;
        }



        Mover.Move(speed * Vector3.up * Time.deltaTime * (Mathf.Sin(sinValue) * 10));
    }




    public IEnumerator SineWave()
    {

        if (delayOnce)
        {
            yield return new WaitForSeconds(Delay);
            delayOnce = false;
        }
        

        while (sinValue <= 360)
        {
            sinValue++;
            yield return new WaitForSeconds(TimeInterval);
        }

        while (sinValue > 0)
        {
            sinValue--;
            yield return new WaitForSeconds(TimeInterval);
        }

        constantSin = true;
    }
}

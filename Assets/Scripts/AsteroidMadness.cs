using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.AI;
using UnityEngine.UI;

public class AsteroidMadness : MonoBehaviour
{

    //for now, movement 1 will always be true





    [Header("Madness asteroid sets")]
    public GameObject Madness1Set;
    public GameObject Madness2Set;
    public GameObject ManosSet;

    private GameObject MomentaryObject;
    private CharacterController MomObjCC;

    private bool MovementType1 = false;
    private bool MovementType2 = false;
    private bool MovementType3 = false;

    private bool move1 = false;

    [Header("This is for manos's idea of the comets")]
    public bool isManos = false;

    [Header("Type 1 movement settings")]
    public float speed1 = 0;
    public float TimeInterval = 0;
    public float Delay = 0;
    [Header("Type 2 movement settings")]
    public float speed2 = 0;
    public float flashFrequency;
    public Material FlasherColor;





    private float sinValue = 0;
    private int randNum = 0;
    private System.Random random = new System.Random();
    private bool constantSin = false;
    private bool constantFlash = false;
    

    void Awake()
    {

        if(isManos)
        {
            MovementType3 = true;
        }
        else
        {
            randNum = random.Next(1, 3);
            Debug.Log(randNum);

            switch (randNum)
            {
                case 1:
                    MovementType1 = true;
                    break;
                case 2:
                    MovementType2 = true;
                    break;
            }
        }

    }


      
    void Update()
    {
       



        //constantly looping sine value
        if (constantSin)
        {
            StartCoroutine(SineWave());
            constantSin = false;
        }
        //constant flashing
        if (constantFlash)
        {
            StartCoroutine(Flash());
            constantFlash = false;
        }




        //Make all the type movements delete themselves later as they arent spawned as a child
        if (MovementType1)
        {
            //Instantiates the object
            MomentaryObject = Instantiate(Madness1Set, gameObject.transform.position, Quaternion.Euler(90, 0, 0));

            //Adds to it a CC component
            MomentaryObject.AddComponent<CharacterController>();

            //Makes a refrence to the CC component
            MomObjCC = MomentaryObject.GetComponent<CharacterController>();


            MomObjCC.radius = 0;
            MomObjCC.height= 0;

            constantSin = true;
            MovementType1 = false;
            move1 = true;


            OptimizeSet();
        }
        else if (MovementType2)
        {
            //yada yada yada whatever it is
        }
        else if (MovementType3)
        {
            //Instantiates the object
            MomentaryObject = Instantiate(ManosSet, gameObject.transform.position, Quaternion.Euler(0, 90, 0));

            constantFlash = true;
            MovementType3 = false;
            CometMovement.speed = speed2;


            OptimizeSet();
        }







        //Movement type 1
        if (move1)
        {
            MomObjCC.Move(speed1 * Vector3.up * Time.deltaTime * (Mathf.Sin(sinValue) * 10));
        }







    }





    public IEnumerator SineWave()
    {
        yield return new WaitForSeconds(Delay);
        Delay = 0;


        while(sinValue <= 360)
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





    //flashing sequence
    public IEnumerator Flash()
    {
        int x = 20;
        FlasherColor.color = new Color(FlasherColor.color.r, FlasherColor.color.g, FlasherColor.color.b, 1);

        float alpha = FlasherColor.color.a / x;

        for(int i = 0; i < x/2; i++)
        {
            FlasherColor.color = new Color(FlasherColor.color.r, FlasherColor.color.g, FlasherColor.color.b, FlasherColor.color.a - alpha);
            yield return new WaitForSeconds(flashFrequency / 10);
        }

        for (int i = 0; i < x /2; i++)
        {
            FlasherColor.color = new Color(FlasherColor.color.r, FlasherColor.color.g, FlasherColor.color.b, FlasherColor.color.a + alpha);
            yield return new WaitForSeconds(flashFrequency / 10);
        }

        constantFlash = true;
    }



    public void OptimizeSet()
    {
        Destroy(MomentaryObject, 100f);
    }
}

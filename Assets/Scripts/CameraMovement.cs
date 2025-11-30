using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UIElements;

public class CameraMovement : MonoBehaviour
{
    /*I repurposed the camera movemnt script to make the movement for the background scenery, and I did that by adding
 a isCam boolean, and a drag value that basically moves the object back every iteration.*/
    public bool isCam = true;
    private float drag;
    public float dragRate;

    public Transform shipLocation;
    private GameObject Camera;
    private Camera cameraC;

    private Vector3 distance;
    private Vector3 Ylock;

    public static float FOV = 0;
    public static float FOVb = 0;
    //this is so i can edit the FOV from the inspector
    [Header("Field of view of the camera")]
    public float FieldOfView = 80;
    public float FieldOfViewWhileBoosted = 143;


    private float yLockValue;

    private bool doItOnce = true;
    void Awake()
    {
        //FOV stuff
        Camera = gameObject;
        if (isCam)
        {
            cameraC = Camera.GetComponent<Camera>();
            FOV = FieldOfView;
            FOVb = FieldOfViewWhileBoosted;
        }
        //y-lock
        yLockValue = Camera.transform.position.y;




        //This is the vector of the distance between the spaceship and the camera
        distance = Camera.transform.position - shipLocation.position;
    }

    // Update is called once per frame
    void FixedUpdate()
    {
        Debug.Log(BlackHole.blackLock);
        /*
         Due to recent changes, I changed it to only setting the x and z values, and the y value remains the same as to lock its y movement
         */
        if (isCam)
        {

            //I am changing this to allow for camera shake


            //Camera.transform.position = new Vector3(shipLocation.position.x + distance.x, yLockValue, shipLocation.position.z + distance.z);

            cameraC.fieldOfView = FOV;




            //if blackLock is true, it will call
            //the entering sequence method.
            if (BlackHole.blackLock && doItOnce)
            {
                StartCoroutine(BlackHoleEnter());
                doItOnce = false;
            }


        }
        /*I repurposed the camera movemnt script to make the movement for the background scenery, and I did that by adding
         a isCam boolean, and a drag value that basically moves the object back every iteration.*/
        else
        {
            Camera.transform.position = new Vector3(shipLocation.position.x + distance.x, yLockValue, (shipLocation.position.z + distance.z) - drag);
            drag += dragRate;
        }

    }


    //black hole entering sequence
    public IEnumerator BlackHoleEnter()
    {
        /*It wil wait for 4 seconds to give the
         * player time to get near and then
         * over 100 iterations, it will slowly add
         to the FOV to make the zoom out effect.*/
        yield return new WaitForSeconds(4f);
        int x = 100;

        float fovs = (179 - cameraC.fieldOfView) / x;

        for (int i = 0; i < x; i++)
        {
            yield return new WaitForSeconds(0.09f);
            FOV += fovs;
        }
    }
}

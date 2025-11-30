using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Rendering;

public class PlanetRotation : MonoBehaviour
{
    [Header("Which axis to rotate")]
    public bool spinX;
    public bool spinY;
    public bool spinZ;
    [Header("Offsets")]
    public float X_Offset;
    public float Y_Offset;
    public float Z_Offset;
    [Header("This is the delay between degree turns")]
    public float delay;
    public float rotation;
    private float rotationAdd;

    private bool switcher = false;



    void FixedUpdate()
    {
        //this is for repeatedly calling the function
        //without duplicate calls at the same time.
        if (!switcher)
        {
            StartCoroutine(Rotate());
            switcher = true;
        }
    }


    public IEnumerator Rotate()
    {
        //Waits according to delay before adding a degree
        yield return new WaitForSeconds(delay);

        //if spinX is true, it will add to the X_Offset.
        if (spinX)
        {
            gameObject.transform.rotation = Quaternion.Euler(X_Offset + (rotationAdd += rotation), Y_Offset, Z_Offset);
        }
        //if spinY is true, it will add to the Y_Offset.
        if (spinY)
        {
            gameObject.transform.rotation = Quaternion.Euler(X_Offset,Y_Offset + (rotationAdd += rotation), Z_Offset);
        }
        //if spinZ is true, it will add to the Z_Offset.
        if (spinZ)
        {
            gameObject.transform.rotation = Quaternion.Euler(X_Offset, Y_Offset, Z_Offset + (rotationAdd += rotation));
        }


        switcher = false;
    }
}

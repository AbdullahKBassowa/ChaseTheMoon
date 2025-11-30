using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class CometMovement : MonoBehaviour
{
    private CharacterController[] comets;

    private MeshRenderer[] flashers;
    public static float speed;

    private bool shoot = false;


    private void Awake()
    {
        comets = GetComponentsInChildren<CharacterController>();
        
        flashers = GetComponentsInChildren<MeshRenderer>();

    }


    private void Update()
    {
        if (shoot)
        {

            foreach (CharacterController c in comets)
            {
                c.Move(speed * Vector3.back * Time.deltaTime);     
            }
        }
    }


    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Spaceship"))
        {
            shoot = true;
            Debug.Log("Shoot!");


            foreach(MeshRenderer m in flashers)
            {
                if(m.GetComponent<CharacterController>() == null)
                {
                    m.enabled = false;
                }
            }
        } 
    }
}

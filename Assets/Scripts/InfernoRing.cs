using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class InfernoRing : MonoBehaviour
{
    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Spaceship"))
        {
            FindObjectOfType<Audio>().Play("Inferno");
            gameObject.GetComponent<ParticleSystem>().Play();
        }
    }
}

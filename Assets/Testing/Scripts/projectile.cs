using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.XR;

public class projectile : MonoBehaviour
{
    private void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.layer == LayerMask.NameToLayer("Zombie"))
        {
            other.GetComponent<Zombie>().Damage();
        }
    }
}

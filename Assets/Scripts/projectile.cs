using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.XR;

public class projectile : MonoBehaviour
{
    private void OnCollisionEnter(Collision collision)
    {
        if (collision.gameObject.layer == LayerMask.NameToLayer("Zombie"))
        {
            collision.gameObject.GetComponent<Zombie>().Damage();
        }
    }
}

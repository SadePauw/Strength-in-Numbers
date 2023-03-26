using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class Shoot : MonoBehaviour
{
    private InputAction leftMouseClick;
    public GameObject projectile;
    public float projectileSpeed;
    public Vector3 offset;
    public Camera camera;
    private void Awake()
    {
        leftMouseClick = new InputAction(binding: "<Mouse>/leftButton");
        leftMouseClick.performed += ctx => LeftMouseClicked();
        leftMouseClick.Enable();
    }

    private void LeftMouseClicked()
    {
        var obj = Instantiate(projectile, transform.position + offset, transform.rotation);
        obj.GetComponent<Rigidbody>().AddForce(camera.transform.forward * projectileSpeed);
        Destroy(obj, 3);

    }
}

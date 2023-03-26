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
    public Transform projectileParent;

    private void Awake()
    {
        leftMouseClick = new InputAction(binding: "<Mouse>/leftButton");
        leftMouseClick.performed += ctx => LeftMouseClicked();
        leftMouseClick.Enable();
        projectileParent = GameObject.Find("Projectile").transform;
    }

    private void LeftMouseClicked()
    {
        var obj = Instantiate(projectile, transform.position + offset, transform.rotation, projectileParent);
        obj.GetComponent<Rigidbody>().AddForce(camera.transform.forward * projectileSpeed);
        Destroy(obj, 3);

    }

    private void OnDisable()
    {
        leftMouseClick.Disable();
    }
}

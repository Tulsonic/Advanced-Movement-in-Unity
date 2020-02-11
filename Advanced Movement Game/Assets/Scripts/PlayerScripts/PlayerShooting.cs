using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerShooting : MonoBehaviour
{
    [SerializeField] private GameObject projectile;
    [SerializeField] private float bulletSpeed;

    private Transform gun;
    private Camera playerCamera;

    void Start()
    {
        gun = GameObject.Find("GUN").GetComponent<Transform>();
        playerCamera = GameObject.Find("PlayerCamera").GetComponent<Camera>();
    }

    void Update()
    {
        if(Input.GetMouseButton(0))
        {
            RaycastHit hitPoint;
            Physics.Raycast(playerCamera.transform.position, playerCamera.transform.forward, out hitPoint);
            GameObject bullet = Instantiate(projectile, gun.position, Quaternion.identity) as GameObject;
            bullet.GetComponent<Transform>().LookAt(hitPoint.point);
            bullet.GetComponent<Rigidbody>().AddForce(transform.forward * bulletSpeed);
        }
    }
}

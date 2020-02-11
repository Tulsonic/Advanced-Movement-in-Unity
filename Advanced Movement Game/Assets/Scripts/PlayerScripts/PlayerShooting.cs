using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerShooting : MonoBehaviour
{
    [SerializeField] private float bulletSpeed;
    [SerializeField] private float waitTime;

    private float time;

    private Camera playerCamera;
    private GameObject bulletEmitter;
    private ParticleSystem particleSystem;

    void Start()
    {
        playerCamera = GameObject.Find("PlayerCamera").GetComponent<Camera>();
        bulletEmitter = GameObject.Find("BulletEmitter");
        particleSystem = GameObject.Find("MuzzleFlash").GetComponent<ParticleSystem>();
    }

    void Update()
    {
        time += Time.deltaTime;
        if (time >= waitTime)
        {
            shoot();
            time = 0;
        }
        
    }

    void shoot()
    {
        if (Input.GetMouseButton(0))
        {
            particleSystem.Play();
            if (Physics.Raycast(bulletEmitter.transform.position, playerCamera.transform.forward, out RaycastHit hitPoint))
            {

            }
        }
    }
}

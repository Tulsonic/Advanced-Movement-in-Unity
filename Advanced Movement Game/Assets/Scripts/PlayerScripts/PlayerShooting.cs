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
    private ParticleSystem muzzelFlashParticle;

    [SerializeField] private GameObject bulletHitParticle;

    void Start()
    {
        playerCamera = GameObject.Find("PlayerCamera").GetComponent<Camera>();
        bulletEmitter = GameObject.Find("BulletEmitter");
        muzzelFlashParticle = GameObject.Find("MuzzleFlash").GetComponent<ParticleSystem>();
    }

    void Update()
    {
        shoot();
    }

    void shoot()
    {
        time += Time.deltaTime;
        if (time >= waitTime)
        {
            if (Input.GetMouseButton(0))
            {
                muzzelFlashParticle.Play();
                if (Physics.Raycast(bulletEmitter.transform.position, playerCamera.transform.forward, out RaycastHit hitPoint))
                {
                    GameObject bulletHitParticleInWorld = Instantiate(bulletHitParticle, hitPoint.point, Quaternion.identity) as GameObject;
                    bulletHitParticleInWorld.transform.LookAt(bulletHitParticleInWorld.transform.position + hitPoint.normal);
                }
            }
            time = 0;
        }
    }
}

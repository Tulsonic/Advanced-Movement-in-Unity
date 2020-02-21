using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerShooting : MonoBehaviour
{
    [SerializeField] private float bulletSpeed;
    [SerializeField] private float fireRate;
    [SerializeField] private float damage;
    [SerializeField] private float impact;

    private float time;

    private Camera playerCamera;
    private GameObject bulletEmitter;
    private ParticleSystem muzzelFlashParticle;

    private RecoilCamera recoilCamera;
    private RecoilWeapon recoilWeapon;

    [SerializeField] private GameObject bulletHitParticle;

    void Start()
    {
        playerCamera = GameObject.Find("PlayerCamera").GetComponent<Camera>();

        recoilWeapon = GetComponent<RecoilWeapon>();
        recoilCamera = GetComponent<RecoilCamera>();
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
        if (time >= fireRate)
        {
            if (Input.GetMouseButton(0))
            {
                recoilCamera.SetRotation();
                recoilWeapon.SetRecoil();

                if (!muzzelFlashParticle.isPlaying) { muzzelFlashParticle.Play(); }
                if (Physics.Raycast(playerCamera.transform.position, playerCamera.transform.forward, out RaycastHit hitPoint))
                {
                    GameObject bulletHitParticleInWorld = Instantiate(bulletHitParticle, hitPoint.point + hitPoint.normal * 0.05f, Quaternion.LookRotation(hitPoint.normal)) as GameObject;

                    TestTarget target = hitPoint.transform.gameObject.GetComponent<TestTarget>();
                    if (target != null)
                    {
                        target.TakeDamage(damage);
                    }

                    Rigidbody rigidbody = hitPoint.transform.gameObject.GetComponent<Rigidbody>();
                    if (rigidbody != null)
                    {
                        rigidbody.AddForce(-hitPoint.normal * impact);
                    }
                }
            }
            time = 0;
        }

        if (Input.GetMouseButtonUp(0))
        {
            recoilCamera.StopShooting();
        }
    }
}

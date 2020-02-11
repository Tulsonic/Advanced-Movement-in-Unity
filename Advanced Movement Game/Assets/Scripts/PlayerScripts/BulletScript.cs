using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BulletScript : MonoBehaviour
{
    [SerializeField] private float bulletLifeSpan;

    private float time;

    private void OnCollisionEnter(Collision collision)
    {
        Object.Destroy(gameObject);
    }

    private void Update()
    {
        time += Time.deltaTime;
        if (time >= bulletLifeSpan)
        {
            Object.Destroy(gameObject);
        }
    }
}

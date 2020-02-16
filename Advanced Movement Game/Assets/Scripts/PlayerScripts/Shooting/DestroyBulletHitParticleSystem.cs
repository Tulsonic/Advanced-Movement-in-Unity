using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DestroyBulletHitParticleSystem : MonoBehaviour
{
    private bool isFading = false;
    private float time;
    [SerializeField] private float timeBeforeFade;
    [SerializeField] private float fadeTime;

    void Update()
    {
        if (isFading)
        {
            time += Time.deltaTime;
            if (time >= timeBeforeFade) { 
                Material material = GetComponent<ParticleSystem>().GetComponent<Renderer>().material;
                Color color = material.color;

                material.color = new Color(color.r, color.g, color.b, color.a - fadeTime * Time.deltaTime);

                if (material.color.a <= 0)
                {
                    Destroy(gameObject);
                }
            }   
        }
    }

    private void OnParticleCollision(GameObject other)
    {
        isFading = true;
    }
}
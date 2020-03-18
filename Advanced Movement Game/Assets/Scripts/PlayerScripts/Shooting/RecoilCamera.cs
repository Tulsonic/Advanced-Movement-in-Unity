using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RecoilCamera : MonoBehaviour
{
    [SerializeField] private float rotationSpeed;
    [SerializeField] private float returnSpeed;

    [SerializeField] private Vector3 recoilRotation;

    private Vector3 curentRotation;
    private Vector3 Rot;

    private bool hasFired = false;
    private HeadBob headBob;
    private GameObject headJoint;

    private void Start()
    {
        headJoint = GameObject.Find("HeadJoint");
        headBob = GameObject.Find("PlayerCamera").GetComponent<HeadBob>();
    }

    private void FixedUpdate()
    {
        if (hasFired)
        {
            curentRotation = Vector3.Lerp(curentRotation, Vector3.zero, returnSpeed * Time.deltaTime);
            Rot = Vector3.Slerp(Rot, curentRotation, rotationSpeed * Time.fixedDeltaTime);
            headJoint.transform.localRotation = Quaternion.Euler(Rot);
            if (curentRotation.magnitude < 0.1f)
            {
                StopShooting();
            }
        }
    }

    public void SetRotation()
    {
        curentRotation += new Vector3(-recoilRotation.x, Random.Range(-recoilRotation.y, recoilRotation.y), Random.Range(-recoilRotation.z, recoilRotation.z));
        hasFired = true;
        headBob.enabled = false;
    }

    public void StopShooting()
    {
        headBob.enabled = true;
        hasFired = false;
    }
}

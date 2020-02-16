using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RecoilWeapon : MonoBehaviour
{
    [SerializeField] private Transform rotationPosition;
    [SerializeField] private Transform recoilPosition;

    [SerializeField] private float rotationalRecoilSpeed;
    [SerializeField] private float positionalRecoilSpeed;

    [SerializeField] private float rotationalReturnSpeed;
    [SerializeField] private float positionalReturnSpeed;

    [SerializeField] private Vector3 recoilRotation;
    [SerializeField] private Vector3 kickBackRecoil;

    private Vector3 rotationalRecoil;
    private Vector3 positionalRecoil;
    private Vector3 Rot;

    private void FixedUpdate()
    {
        rotationalRecoil = Vector3.Lerp(rotationalRecoil, Vector3.zero, rotationalReturnSpeed * Time.fixedDeltaTime);
        positionalRecoil = Vector3.Lerp(positionalRecoil, Vector3.zero, positionalReturnSpeed * Time.fixedDeltaTime);

        recoilPosition.localPosition = Vector3.Slerp(recoilPosition.localPosition, positionalRecoil, positionalRecoilSpeed * Time.fixedDeltaTime);
        Rot = Vector3.Slerp(Rot, rotationalRecoil, rotationalRecoilSpeed * Time.fixedDeltaTime);
        rotationPosition.localRotation = Quaternion.Euler(Rot);
    }

    public void SetRecoil()
    {
        rotationalRecoil += new Vector3(recoilRotation.x, Random.Range(-recoilRotation.y, recoilRotation.y), Random.Range(-recoilRotation.z, recoilRotation.z));
        positionalRecoil += new Vector3(Random.Range(-kickBackRecoil.x, kickBackRecoil.x), Random.Range(-kickBackRecoil.y, kickBackRecoil.y), Random.Range(-kickBackRecoil.z, kickBackRecoil.z));
    }
}
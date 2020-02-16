using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HeadBob : MonoBehaviour
{
    #region  variables

    [Header("Walking and breathing")] 
    [SerializeField] private Transform head;

    [SerializeField] private float headBobFrequency = 1.5f;
    [SerializeField] private float headBobSwayAngle = 0.5f;
    [SerializeField] private float headBobHeightMove = 0.3f;
    [SerializeField] private float headBobHeightIdle = 0.3f;
    [SerializeField] private float headBobSideMovement = 0.05f;
    [SerializeField] private float headBobSpeedMultiplier = 0.3f;
    [SerializeField] private float bobStrideSpeedLengthen = 0.3f;
    [SerializeField] private float jumpLandMove = 3f;
    [SerializeField] private float jumpLandTilt = 60f;

    [SerializeField] private GameObject player;
    [SerializeField] private Rigidbody rb;
    [SerializeField] private PlayerMotion playerMotion;

    [HideInInspector] public Vector3 originalLocalPosition;

    private float headBobHeight;

    private float nextStepTime = 0.5f;
    private float headBobCycle = 0.0f;
    private float headBobFade = 0.0f;

    private float springPosition = 0.0f;
    private float springVelocity = 0.0f;
    private float springElastic = 1.1f;
    private float springDampen = 0.8f;
    private float springVelocityTreshold = 0.05f;
    private float springPositionTreshold = 0.05f;

    private Vector3 previousPosition;
    private Vector3 previousVelocity = Vector3.zero;

    private bool prevGrounded;

    private float breath = 0f;

    #endregion

    private void Start()
    {
        originalLocalPosition = head.localPosition;
        previousPosition = rb.position;
    }

    private void FixedUpdate()
    {
        if (playerMotion.isGrounded && !playerMotion.isCrouching)
        {
            Vector3 velocity = (rb.position - previousPosition) / Time.deltaTime;
            Vector3 deltaVelocity = velocity - previousVelocity;
            previousPosition = rb.position;
            previousVelocity = velocity;

            springVelocity = deltaVelocity.y;
            springVelocity -= springPosition * springElastic;
            springVelocity *= springDampen;

            springPosition += springVelocity * Time.deltaTime;
            springPosition = Mathf.Clamp(springPosition, -0.3f, 0.3f);

            if (Mathf.Abs(springVelocity) < springVelocityTreshold &&
                 Mathf.Abs(springPosition) < springPositionTreshold)
            {
                springPosition = 0;
                springVelocity = 0;
            }

            float flatVelocity = new Vector3(velocity.x, 0, velocity.z).magnitude;

            float strideLengthen = 1 + (flatVelocity * bobStrideSpeedLengthen);

            headBobCycle += (flatVelocity / strideLengthen) * (Time.deltaTime / headBobFrequency);

            float bobFactor = Mathf.Sin(headBobCycle * Mathf.PI * 2);
            float bobSwayFactor = Mathf.Sin(Mathf.PI * (2 * headBobCycle + 0.5f));

            bobFactor = 1 - (bobFactor * 0.5f + 1);
            bobFactor *= bobFactor;

            if (new Vector3(velocity.x, 0, velocity.z).magnitude < 0.1f)
            {
                headBobHeight = headBobHeightIdle;
                headBobFade = Mathf.Lerp(headBobFade, 0.5f, Time.deltaTime);
                breath += 1f * (Time.deltaTime / headBobFrequency);
                bobFactor = Mathf.Sin(breath * Mathf.PI * 2);
            }
            else
            {
                headBobHeight = headBobHeightMove;
                headBobFade = Mathf.Lerp(headBobFade, 1.0f, Time.deltaTime);
            }

            float speedHeightFactor = 1 + (flatVelocity * headBobSpeedMultiplier);

            float xPos = -headBobSideMovement * bobSwayFactor;
            float yPos = springPosition * jumpLandMove + bobFactor * headBobHeight * headBobFade * speedHeightFactor;

            float xTilt = -springPosition * jumpLandTilt;
            float zTilt = bobSwayFactor * headBobSwayAngle * headBobFade;

            head.localPosition = originalLocalPosition + new Vector3(xPos, yPos, 0);
            head.localRotation = Quaternion.Euler(xTilt, 0.0f, zTilt);

            foreach (Transform child in GetComponent<Transform>())
            {
                GameObject childParent = child.gameObject;
                Vector3 getStartingPoint = childParent.GetComponent<GetStartingPosition>().startingPoint;
                Vector3 getStartingRotation = childParent.GetComponent<GetStartingPosition>().startingRotation;
                childParent.GetComponent<Transform>().localPosition = getStartingPoint - new Vector3(xPos, yPos, 0);
                childParent.GetComponent<Transform>().localRotation = Quaternion.Euler(-xTilt, getStartingRotation.y, -zTilt);
            }
        }
        else
        {
            head.localPosition = originalLocalPosition;
            foreach (Transform child in GetComponent<Transform>())
            {
                GameObject childParent = child.gameObject;
                Vector3 getStartingPoint = childParent.GetComponent<GetStartingPosition>().startingPoint;
                Vector3 getStartingRotation = childParent.GetComponent<GetStartingPosition>().startingRotation;
                childParent.GetComponent<Transform>().localPosition = getStartingPoint;
                childParent.GetComponent<Transform>().localRotation = Quaternion.Euler(getStartingRotation.x, getStartingRotation.y, getStartingRotation.z);
            }
        }
    }

}

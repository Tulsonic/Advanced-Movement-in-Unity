using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerMotion : MonoBehaviour
{
    #region variables

    [Header("Speed related")]
    [SerializeField] private float normalSpeed;
    [SerializeField] private float sprintSpeed;
    [SerializeField] private float maxSpeed;
    [SerializeField] private float airSpeed;
    [SerializeField] private float crouchSpeed;
    [SerializeField] private float sprintFOVModifier;
    [SerializeField] private float speedModifier;

    [Header("Wall Run")]
    [SerializeField] private float wallRunMaxTime;
    [SerializeField] private float wallRunMaxCoolDownTime;
    [SerializeField] private float wallRunJumpOffForce;
    [SerializeField] private float wallRunJumpUpForce;
    [SerializeField] private float wallRunDrag;

    [Header("Other")]
    [SerializeField] private float jumpForce;
    [SerializeField] private float grappleSpeed;
    [SerializeField] private float slidingHorizontalSpeed;
    [SerializeField] private float grappleRange;

    [HideInInspector] public RaycastHit grappleTarget;
    [HideInInspector] public bool isGrounded;
    [HideInInspector] public bool isSprinting;
    [HideInInspector] public Vector3 direction;
    [HideInInspector] public bool isCrouching;

    private LayerMask layerMask;

    private RaycastHit wallHit;
    private RaycastHit wallImpactRight;
    private RaycastHit wallImpactLeft;

    private Rigidbody playerRigidbody;
    private Camera playerCamera;
    private Transform playerTransform;
    private LineRenderer ropeRenderer;

    private GameObject ropeStart;
    private ParticleSystem partSystem;

    private float baseFOV;

    private float hMove;
    private float vMove;
    private float speedFinal;

    private float wallRunTime;
    private float wallRunCoolDownTime;

    private float ropeSize;
    private float counter;

    private bool jump;
    private bool sprint;
    private bool crouch;
    private bool canWallRun;

    private bool isWalking;
    private bool isJumping;
    private bool isWallRunning;
    private bool isWallJumping;
    private bool grappleThrown;
    private bool isGrappling;
    private bool isDrawingGrappleBack;
    private bool isCameraRotating;

    private Vector3 rotateValue;

    #endregion

    private void Start()
    {
        Cursor.lockState = CursorLockMode.Locked;
        Cursor.visible = false;

        playerRigidbody = GetComponent<Rigidbody>();
        playerTransform = GetComponent<Transform>();

        layerMask = LayerMask.GetMask("Enviroment");

        playerCamera = GameObject.Find("PlayerCamera").GetComponent<Camera>();
        baseFOV = playerCamera.fieldOfView;

        speedFinal = 100;

        canWallRun = true;
        wallRunCoolDownTime = 0;

        ropeStart = GameObject.Find("RopeStart");
        ropeRenderer = ropeStart.GetComponent<LineRenderer>();
        grappleThrown = false;
        isGrappling = false;

        rotateValue = Vector3.zero;
        isCameraRotating = false;

        partSystem = GameObject.Find("ParticleSystem").GetComponent<ParticleSystem>(); ;
    }

    private RaycastHit DoWallRunCheck()
    {
        Ray rayRight = new Ray(playerTransform.position, playerTransform.TransformDirection(Vector3.right));
        Ray rayLeft = new Ray(playerTransform.position, playerTransform.TransformDirection(Vector3.left));

        bool rightImpact = Physics.Raycast(rayRight.origin, rayRight.direction, out wallImpactRight, 1f, layerMask);
        bool leftInpact = Physics.Raycast(rayLeft.origin, rayLeft.direction, out wallImpactLeft, 1f, layerMask);

        if (rightImpact && Vector3.Angle(playerTransform.TransformDirection(Vector3.forward), wallImpactRight.normal) > 50)
        {
            return wallImpactRight;
        }
        else if (leftInpact && Vector3.Angle(playerTransform.TransformDirection(Vector3.forward), wallImpactLeft.normal) > 50)
        {
            wallImpactLeft.normal = wallImpactLeft.normal * -1;
            return wallImpactLeft;
        }
        else
        {
            return new RaycastHit();
        }
    }

    private void WallRunning()
    {
        if (!isGrounded && wallRunTime < wallRunMaxTime)
        {
            float startJumpHeight = playerRigidbody.velocity.y;

            if (wallRunTime == 0f)
            {
                direction.y = jumpForce;
            }
            else
            {
                direction.y = startJumpHeight / 4;
            }
            wallRunTime += Time.deltaTime;

            if (jump)
            {
                isWallJumping = true;
                isCameraRotating = false;
                playerRigidbody.AddForce(Vector3.up * wallRunJumpUpForce);
                if (wallImpactRight.normal != Vector3.zero) { playerRigidbody.AddForce(wallHit.normal * wallRunJumpOffForce); }
                else if (wallImpactLeft.normal != Vector3.zero) { playerRigidbody.AddForce(wallHit.normal * wallRunJumpOffForce * -1); }
                canWallRun = false;
            }

            Vector3 crossProduct = Vector3.Cross(Vector3.up, wallHit.normal);

            if (crossProduct != Vector3.zero)
            {
                Quaternion lookDirection = Quaternion.LookRotation(crossProduct);
                playerTransform.rotation = Quaternion.Slerp(playerTransform.rotation, lookDirection, 3.5f * Time.deltaTime);
            }
            Vector3 setRotation = Vector3.zero;

            if (wallImpactRight.collider != null)
            {
                if (playerCamera.transform.localEulerAngles.z < 15 || playerCamera.transform.localEulerAngles.z > 350)
                {
                    isCameraRotating = true;
                    setRotation = new Vector3(0, 0, 1);
                    rotateValue += setRotation * 80 * Time.deltaTime;
                    rotateValue.x = playerCamera.transform.localEulerAngles.x;
                    rotateValue.y = playerCamera.transform.localEulerAngles.y;
                    playerCamera.transform.localEulerAngles = rotateValue;
                }
            }
            else if (wallImpactLeft.collider != null)
            {
                if (playerCamera.transform.localEulerAngles.z > 350 || playerCamera.transform.localEulerAngles.z < 15)
                {
                    isCameraRotating = true;
                    setRotation = new Vector3(0, 0, -1);
                    rotateValue += setRotation * 80 * Time.deltaTime;
                    rotateValue.x = playerCamera.transform.localEulerAngles.x;
                    rotateValue.y = playerCamera.transform.localEulerAngles.y;
                    playerCamera.transform.localEulerAngles = rotateValue;
                }
            }

            direction = crossProduct;
            direction.Normalize();
        }
        else
        {
            isWallRunning = false;
            isCameraRotating = false;
        }
    }

    private void AddComponentConfigurableJoint()
    {
        gameObject.AddComponent<ConfigurableJoint>();

        ConfigurableJoint cJ = GetComponent<ConfigurableJoint>();

        cJ.autoConfigureConnectedAnchor = false;
        cJ.connectedAnchor = grappleTarget.point;
        cJ.xMotion = ConfigurableJointMotion.Limited;
        cJ.zMotion = ConfigurableJointMotion.Limited;
        cJ.yMotion = ConfigurableJointMotion.Limited;

        SoftJointLimit softJointLimit = new SoftJointLimit();
        softJointLimit.limit = (playerRigidbody.transform.position - grappleTarget.point).magnitude;
        softJointLimit.bounciness = 0;
        softJointLimit.contactDistance = 1;
        cJ.linearLimit = softJointLimit;

        SoftJointLimitSpring softJointLimitSpring = new SoftJointLimitSpring();
        softJointLimitSpring.spring = 100;
        softJointLimitSpring.damper = 20;
        cJ.linearLimitSpring = softJointLimitSpring;

        cJ.configuredInWorldSpace = true;
    }

    private void HandleGrapple()
    {
        ropeRenderer.SetPosition(0, ropeStart.transform.position);
        float distance = Vector3.Distance(ropeStart.transform.position, grappleTarget.point);
        counter += grappleSpeed * Time.deltaTime;
        ropeSize = Mathf.Lerp(0, distance, counter);

        Vector3 pointA = ropeStart.transform.position;
        Vector3 pointB = grappleTarget.point;

        Vector3 pointAlongLine = ropeSize * Vector3.Normalize(pointB - pointA) + pointA;

        ropeRenderer.SetPosition(1, pointAlongLine);

        if (Mathf.Abs(ropeSize - distance) < 0.1)
        {
            isGrappling = true;
        }
    }

    private void Grappling()
    {
        ConfigurableJoint cJ = GetComponent<ConfigurableJoint>();
        if (cJ == null) { AddComponentConfigurableJoint(); }
        if (Input.GetKey("e") && cJ != null)
        {
            SoftJointLimit softJointLimit = cJ.linearLimit;
            softJointLimit.limit -= 50f * Time.deltaTime;
            cJ.linearLimit = softJointLimit;
        }
    }

    private void DrawBackGrapple()
    {
        ropeRenderer.SetPosition(0, ropeStart.transform.position);
        float distance = Vector3.Distance(ropeStart.transform.position, grappleTarget.point);
        counter += grappleSpeed * Time.deltaTime;
        ropeSize = Mathf.Lerp(ropeSize, 0, counter);

        Vector3 pointA = ropeStart.transform.position;
        Vector3 pointB = grappleTarget.point;

        Vector3 pointAlongLine = ropeSize * Vector3.Normalize(pointB - pointA) + pointA;

        ropeRenderer.SetPosition(1, pointAlongLine);

        if (ropeSize < 0.3)
        {
            isDrawingGrappleBack = false;
        }
    }

    private void Update()
    {
        // Imput for movement
        if (!Input.GetKey("a") && !Input.GetKey("d")) { hMove = 0; }
        else if (Input.GetKey("a")) { hMove = -1; }
        else if (Input.GetKey("d")) { hMove = 1; }

        if (!Input.GetKey("w") && !Input.GetKey("s")) { vMove = 0; }
        else if (Input.GetKey("w")) { vMove = 1; }
        else if (Input.GetKey("s")) { vMove = -1; }

        // Controls
        jump = Input.GetKeyDown(KeyCode.Space);
        sprint = Input.GetKey(KeyCode.LeftShift);
        crouch = Input.GetKey(KeyCode.LeftControl);

        // Grapple 
        if (Input.GetMouseButtonDown(1))
        {
            counter = 0;
            if (Physics.Raycast(playerCamera.transform.position, playerCamera.transform.forward, out grappleTarget, grappleRange))
            {
                grappleThrown = true;
                ropeRenderer.enabled = true;
            }
        }
        if (Input.GetMouseButtonUp(1))
        {
            isDrawingGrappleBack = true;
            isGrappling = false;
            grappleThrown = false;
            ropeRenderer.enabled = false;
            counter = 0;
            Destroy(GetComponent<ConfigurableJoint>());
        }
        if (grappleThrown) { HandleGrapple(); }
        if (isGrappling) { Grappling(); }
        if (isDrawingGrappleBack) { DrawBackGrapple(); }

        // Jump
        if (isJumping && !isWallRunning)
        {
            playerRigidbody.AddForce(Vector3.up * jumpForce);
        }

        // Wall running 
        if (!isGrounded) { wallHit = DoWallRunCheck(); }
        if (isWallRunning) { WallRunning(); }

        // Crouch 
        if (isCrouching)
        {
            Vector3 changePlayerScale = playerTransform.localScale;
            changePlayerScale.y = 0.5f;
            playerTransform.localScale = changePlayerScale;
            if(isJumping) { speedFinal = sprintSpeed; }
        }
        else
        {
            Vector3 changePlayerScale = playerTransform.localScale;
            changePlayerScale.y = 1;
            playerTransform.localScale = changePlayerScale;
        }

        // States
        isGrounded = UnityEngine.Physics.Raycast(playerTransform.position, Vector3.down, playerTransform.localScale.y / 2 + 0.6f, layerMask);
        if (vMove != 0 || hMove != 0) { isWalking = true; }
        isJumping = (jump && isGrounded);
        isCrouching = crouch;
        if (sprint && vMove > 0 && isGrounded && !isCrouching) { isSprinting = true; } else if (!sprint || vMove == 0) { isSprinting = false; }
        if (wallHit.collider != null && canWallRun) { isWallRunning = true; wallRunTime = 0f; } else { isWallRunning = false; isCameraRotating = false; }

        // Wall jump state manager
        if (isWallJumping)
        {
            wallRunCoolDownTime += Time.fixedDeltaTime;
            if (wallRunCoolDownTime > wallRunMaxCoolDownTime) { canWallRun = true; wallRunCoolDownTime = 0; }

            if (isGrounded || isWallRunning)
            {
                wallRunCoolDownTime = 0f;
                isWallJumping = false;
            }
        }
    }

    private void FixedUpdate()
    {
        direction = new Vector3(hMove, 0, vMove);
        direction.Normalize();

        // Edit FOV and speed based on sprint state
        if (isSprinting && speedFinal < sprintSpeed && (isGrounded || isWallRunning))
        {
            speedFinal += speedModifier * Time.fixedDeltaTime;
        }
        else if (!isSprinting && speedFinal > normalSpeed && isGrounded)
        {
            speedFinal -= speedModifier * Time.fixedDeltaTime;
        }
        else if (!isSprinting && !isCrouching && speedFinal < normalSpeed && isGrounded)
        {
            speedFinal += speedModifier * Time.fixedDeltaTime;
        }
        else if (!isWalking && isGrounded) { speedFinal = 100; }

        // Edit speed based on crouch state
        if (isCrouching && speedFinal > crouchSpeed)
        {
            speedFinal -= speedModifier * Time.fixedDeltaTime;
        }

        // Edit camera FOV
        float FOV = isSprinting ? Mathf.Lerp(playerCamera.fieldOfView, baseFOV * sprintFOVModifier, 8f * Time.fixedDeltaTime) : Mathf.Lerp(playerCamera.fieldOfView, baseFOV, 8f * Time.fixedDeltaTime);

        // Reset camera rortation
        if (wallImpactLeft.collider == null && wallImpactRight.collider == null && playerCamera.transform.localEulerAngles.z != 0 && !isCameraRotating)
        {
            float resetCameraRotation = playerCamera.transform.localEulerAngles.z;
            if (resetCameraRotation > 340 && resetCameraRotation < 360) { resetCameraRotation += 80 * Time.fixedDeltaTime; }
            if (resetCameraRotation < 25 && resetCameraRotation > 0) { resetCameraRotation -= 80 * Time.fixedDeltaTime; }
            if (resetCameraRotation < 2) { resetCameraRotation = 0; }
            if (resetCameraRotation > 358) { resetCameraRotation = 0; }
            playerCamera.transform.localEulerAngles = new Vector3(playerCamera.transform.localEulerAngles.x, playerCamera.transform.localEulerAngles.y, resetCameraRotation);
            rotateValue = Vector3.zero;
        }

        // Particle effect for sprinting
        if (playerRigidbody.velocity.magnitude > 12) { partSystem.Play(); } else { partSystem.Stop(); }

        // Set movement vector for player
        if (((isGrounded && !isCrouching) || (isCrouching && isGrounded && playerRigidbody.velocity.magnitude < 12)) && !(isGrappling && Input.GetKey("e")) && !isWallRunning)
        {
            Vector3 targetVelocity = transform.TransformDirection(direction) * speedFinal * Time.fixedDeltaTime;
            Vector3 deltaVelocity = (targetVelocity - playerRigidbody.velocity);

            deltaVelocity.x = Mathf.Clamp(deltaVelocity.x, -maxSpeed, maxSpeed);
            deltaVelocity.z = Mathf.Clamp(deltaVelocity.z, -maxSpeed, maxSpeed);
            deltaVelocity.y = 0;

            playerRigidbody.AddForce(deltaVelocity, ForceMode.VelocityChange);
        }
        else if (isWallRunning)
        {
            Vector3 newDirection = new Vector3(hMove / 4, 0, vMove).normalized;
            Vector3 targetVelocity = transform.TransformDirection(newDirection) * speedFinal * Time.fixedDeltaTime;
            Vector3 deltaVelocity = (targetVelocity - playerRigidbody.velocity);

            if (deltaVelocity.magnitude > 0) 
            { 
                deltaVelocity.x = Mathf.Clamp(deltaVelocity.x, -maxSpeed, maxSpeed);
                deltaVelocity.z = Mathf.Clamp(deltaVelocity.z, -maxSpeed, maxSpeed);

                deltaVelocity.y = 0;

                playerRigidbody.AddForce(deltaVelocity, ForceMode.VelocityChange);
            }

            playerRigidbody.AddRelativeForce(Vector3.back * wallRunDrag * Time.deltaTime);
            Vector2 XZComponent = new Vector2(playerRigidbody.velocity.x, playerRigidbody.velocity.z);
            GetComponent<ConstantForce>().relativeForce = (Vector3.up * 9.81f * 4f * playerRigidbody.mass * XZComponent.magnitude * Time.fixedDeltaTime);
        }
        else if (isCrouching && isGrounded && playerRigidbody.velocity.magnitude >= 12)
        {
            GetComponent<ConstantForce>().relativeForce = (Vector3.zero);
            if (Input.GetKey("d")) { playerRigidbody.AddRelativeForce(Vector3.right * slidingHorizontalSpeed * Time.fixedDeltaTime); }
            if (Input.GetKey("a")) { playerRigidbody.AddRelativeForce(Vector3.left * slidingHorizontalSpeed * Time.fixedDeltaTime); }
        }
        else
        {
            GetComponent<ConstantForce>().relativeForce = (Vector3.zero);
            if (Input.GetKey("w")) { playerRigidbody.AddRelativeForce(Vector3.forward * airSpeed * Time.fixedDeltaTime); }
            if (Input.GetKey("s")) { playerRigidbody.AddRelativeForce(Vector3.back * airSpeed * Time.fixedDeltaTime); }
            if (Input.GetKey("d")) { playerRigidbody.AddRelativeForce(Vector3.right * airSpeed * Time.fixedDeltaTime); }
            if (Input.GetKey("a")) { playerRigidbody.AddRelativeForce(Vector3.left * airSpeed * Time.fixedDeltaTime); }
        }

        // Set camera FOV
        playerCamera.fieldOfView = FOV;

    }
}

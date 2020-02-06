using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerCameraController : MonoBehaviour
{
    [SerializeField] public float mouseSensitivity = 100f;


    private float xAxisClamp = 0f;

    private Transform playerTransform;
    private Transform cameraTransform;

    private PlayerMotion playerMotion;
    private Rigidbody playerRigidbody;

    private Vector3 getStartingPoint;
    private Vector3 startingPosition;

    private void Awake()
    {
        Cursor.lockState = CursorLockMode.Locked;
    }

    private void Start()
    {
        playerTransform = GameObject.Find("Player").GetComponent<Transform>();
        playerMotion = GameObject.Find("Player").GetComponent<PlayerMotion>();
        cameraTransform = GetComponent<Transform>();
        startingPosition = cameraTransform.localPosition;
        playerRigidbody = GameObject.Find("Player").GetComponent<Rigidbody>();
    }

    private void Update()
    {
        Debug.Log(mouseSensitivity);
    }

    private void LateUpdate()
    {
        CameraRotation();
    }

    private void CameraRotation()
    {
        float mouseX = Input.GetAxis("Mouse X") * mouseSensitivity * 5 * Time.deltaTime;
        float mouseY = Input.GetAxis("Mouse Y") * mouseSensitivity * 5 * Time.deltaTime;

        xAxisClamp += mouseY;

        if (xAxisClamp > 90f)
        {
            xAxisClamp = 90f;
            mouseY = 0;
            ClampXAxisRotation(270f);

        }
        else if (xAxisClamp < -90f)
        {
            xAxisClamp = -90f;
            mouseY = 0;
            ClampXAxisRotation(90f);

        }

        transform.Rotate(Vector3.left * mouseY);
        playerTransform.Rotate(Vector3.up * mouseX);
    }

    private void ClampXAxisRotation(float value)
    {
        Vector3 eulerRotation = transform.eulerAngles;
        eulerRotation.x = value;
        transform.eulerAngles = eulerRotation;
    }
}

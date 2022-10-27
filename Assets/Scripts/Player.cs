using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Player : MonoBehaviour
{
    public float speed = 12.5f;

    public CharacterController myController;

    public float mouseSensitivity = 700f;

    public Transform myCameraHead;

    private float cameraVerticalRotation;

    public GameObject bullet;
    public Transform firePosition;


    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        PlayerMovement();

        CameraMovement();
        Shoot();
    }

    private void Shoot()
    {
        if (Input.GetMouseButtonDown(0))
        {
            Instantiate(bullet, firePosition.position, firePosition.rotation);
        }
    }

    private void PlayerMovement()
    {
        float x = Input.GetAxis("Horizontal");
        float z = Input.GetAxis("Vertical");

        Vector3 movement = x * transform.right + z * transform.forward;

        movement = movement * speed * Time.deltaTime;

        myController.Move(movement);
    }

    private void CameraMovement()
    {
        float mouseX = Input.GetAxisRaw("Mouse X") * mouseSensitivity * Time.deltaTime;

        float mouseY = Input.GetAxisRaw("Mouse Y") * mouseSensitivity * Time.deltaTime;

        cameraVerticalRotation -= mouseY;
        cameraVerticalRotation = Mathf.Clamp(cameraVerticalRotation, -90f, 90f);

        transform.Rotate(Vector3.up * mouseX);
        myCameraHead.localRotation = Quaternion.Euler(cameraVerticalRotation, 0f, 0f);
    }
}

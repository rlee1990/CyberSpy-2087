using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Player : MonoBehaviour
{
    public float speed = 12.5f;
    public float crouchSpeed = 6f;
    private bool isCrouching = false;

    public Vector3 velocity;

    public float gravityModifier;

    public float jumpHeight = 100f;
    private bool readyToJump;
    public Transform ground;
    public LayerMask groundLayer;
    public float groundDistance = 0.5f;

    public CharacterController myController;

    public float mouseSensitivity = 700f;

    public Transform myCameraHead;

    private float cameraVerticalRotation;

    public GameObject bullet;
    public Transform firePosition;

    public GameObject muzzleFlash, bulletHole, waterLeak;


    private Vector3 crouchScale = new Vector3(1, 0.5f, 1);
    private Vector3 playerScale;


    // Start is called before the first frame update
    void Start()
    {
        playerScale = transform.localScale;
    }

    // Update is called once per frame
    void Update()
    {
        PlayerMovement();

        CameraMovement();
        Jump();
        Shoot();
        Crouching();
    }

    private void Crouching()
    {
        if (Input.GetKeyDown(KeyCode.LeftAlt))
        {
            StartCrouching();
        }
        if (Input.GetKeyUp(KeyCode.LeftAlt))
        {
            StopCrouching();
        }
    }

    private void StartCrouching()
    {
        transform.localScale = crouchScale;
        isCrouching = true;
    }

    private void StopCrouching()
    {
        transform.localScale = playerScale;
        isCrouching = false;
    }

    void Jump()
    {
        readyToJump = Physics.OverlapSphere(ground.position, groundDistance, groundLayer).Length > 0;
        if (Input.GetButtonDown("Jump") && readyToJump)
        {
            velocity.y = Mathf.Sqrt(jumpHeight * -2f * Physics.gravity.y) * Time.deltaTime;
        }
        myController.Move(velocity);
    }

    private void Shoot()
    {
        if (Input.GetMouseButtonDown(0))
        {
            RaycastHit hit;

            if (Physics.Raycast(myCameraHead.position, myCameraHead.forward, out hit, 100f))
            {
                if (Vector3.Distance(myCameraHead.position, hit.point) > 2f)
                {
                    firePosition.LookAt(hit.point);
                    if (hit.collider.tag == "Shootable")
                        Instantiate(bulletHole, hit.point, Quaternion.LookRotation(hit.normal));
                    if (hit.collider.CompareTag("plane"))
                        Instantiate(waterLeak, hit.point, Quaternion.LookRotation(hit.normal));
                }

                if (hit.collider.CompareTag("enemy"))
                {
                    Destroy(hit.collider.gameObject);
                }
                
            } else
            {
                firePosition.LookAt(myCameraHead.position + (myCameraHead.forward * 50f));
            }
            Instantiate(muzzleFlash, firePosition.position, firePosition.rotation, firePosition);
            Instantiate(bullet, firePosition.position, firePosition.rotation);
        }
    }

    private void PlayerMovement()
    {
        float x = Input.GetAxis("Horizontal");
        float z = Input.GetAxis("Vertical");

        Vector3 movement = x * transform.right + z * transform.forward;

        if (isCrouching) {
            movement = movement * crouchSpeed * Time.deltaTime;
        } else
        {
            movement = movement * speed * Time.deltaTime;
        }

        myController.Move(movement);
        velocity.y += Physics.gravity.y * Mathf.Pow(Time.deltaTime, 2) * gravityModifier;

        if (myController.isGrounded)
            velocity.y = Physics.gravity.y * Time.deltaTime;
       
         myController.Move(velocity);
        
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

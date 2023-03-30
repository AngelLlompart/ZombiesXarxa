using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using Photon.Pun;
public class PlayerMovement2 : MonoBehaviour
{
    [SerializeField] private CharacterController _controller;

    public float speed;
    private float walkSpeed = 5;
    private float runSpeed = 10;
    public float gravity = -9.81f;

    private Vector3 fallVelocity;

    private bool isGrounded;
    public Transform groundCheck;
    public float groundDistance = 0.4f; //Umbral de distància enterra
    public LayerMask groundMask;
    public float jumpHeight = 2f;

    public PhotonView photonView;
    // Start is called before the first frame update
    void Start()
    {
        groundMask = LayerMask.GetMask("Ground");
    }

    // Update is called once per frame
    void Update()
    {
        if (PhotonNetwork.InRoom && !photonView.IsMine)
        {
            return;
        }
        float x = Input.GetAxis("Horizontal");
        float z = Input.GetAxis("Vertical");
        Vector3 movement = new Vector3(x, 0, z);
        Vector3 movement2 = transform.right * x + transform.forward * z;
        _controller.Move(movement2 * speed * Time.deltaTime);
        
        fallVelocity.y += gravity * Time.deltaTime;
        _controller.Move(fallVelocity * Time.deltaTime);
        
        isGrounded = Physics.CheckSphere(groundCheck.position,groundDistance,groundMask);

        if (isGrounded && fallVelocity.y < 0)
        {
            fallVelocity.y = -2;
        }

        if (Input.GetKeyDown(KeyCode.Space) && isGrounded)
        {
            Jump();
        }

        if (Input.GetKey(KeyCode.LeftShift))
        {
            speed = runSpeed;
        }
        else
        {
            speed = walkSpeed;
        }
    }

    private void Jump()
    {
        fallVelocity.y = Mathf.Sqrt(jumpHeight * -2 * gravity);
    }
}

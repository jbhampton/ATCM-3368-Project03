using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class rbPlayerMovement : MonoBehaviour
{
    

    [SerializeField] private Rigidbody PlayerBody;
    [SerializeField] private CapsuleCollider PlayerCollider;
    [Space]

    #region MOVE VARS
    //move vars
    private Vector3 PlayerMovementInput;
    [SerializeField] public float moveSpeed;
    [SerializeField] float minMoveSpeed;
    [SerializeField] float maxMoveSpeed;
    #endregion

    #region JUMP VARS
    //jump vars
    [SerializeField] private float jumpForce;
    private Vector3 jumpDirection;
    public bool isGrounded;
    public Transform groundCheck;
    public float groundDistance = 0.4f;
    public LayerMask groundMask;
    public LayerMask obstacleMask;
    #endregion

    #region CROUCH VARS
    //crouch vars
    private bool Crouching = false;
    private float crouchHeight;
    private float standHeight = 2f;
    public float currentHeight;
    private float minStandHeight = 1f;
    private float maxStandHeight = 2f;
    #endregion

    #region STAMINA VARS
    //stamina vars
    public float stamina = 100f;
    public float fatigue = 10f;
    public int adrenaline = 5;
    #endregion

    //UI vars
    [SerializeField] rbUIManager rbUIManager;
    [SerializeField] Camera playerCamera;
    public CameraShake cameraShake;
    

    private void Awake()
    {
        Crouching = false;
        standHeight = 2f;
        currentHeight = standHeight;
    }

    void Update()
    {
        //==PLAYER MOVEMENT==
        #region PLAYER MOVEMENT
        //adjust move speed
        moveSpeed = Mathf.Clamp(moveSpeed, minMoveSpeed, maxMoveSpeed);
        if (Input.GetKey(KeyCode.LeftControl) == false)
        {
            moveSpeed += Input.GetAxis("Mouse ScrollWheel");
        }
        //move player
        PlayerMovementInput = new Vector3(Input.GetAxis("Horizontal"), 0f, Input.GetAxis("Vertical"));
        MovePlayer();
        #endregion

        //==PLAYER JUMPING==
        #region PLAYER JUMPING
        //check if grounded
        isGrounded = Physics.CheckSphere(groundCheck.position, groundDistance, groundMask);
        if (Input.GetKeyDown(KeyCode.Space) && isGrounded == true)
        {
            Jump();
        }
        #endregion

        //==CROUCHING==
        #region CROUCHING
        //crouch
        if (Input.GetKeyDown(KeyCode.C) && Crouching == false)
        {
            Crouch();
        } else if (Input.GetKeyDown(KeyCode.C) && Crouching == true)
        {
            Stand();
        }
        //adjust crouchHeight
        standHeight = Mathf.Clamp(standHeight, minStandHeight, maxStandHeight);
        if (Input.GetKey(KeyCode.LeftControl)) {
            standHeight += Input.GetAxis("Mouse ScrollWheel");
            PlayerCollider.height = standHeight;
            currentHeight = standHeight;
        }
        if (PlayerCollider.height < maxStandHeight)
        {
            Crouching = true;
        } else
        {
            Crouching = false;
        }
        if (Crouching == true)
        {
            maxMoveSpeed = 3;
        } else
        {
            maxMoveSpeed = 5;
        }
        #endregion
    }

    private void MovePlayer()
    {
        Vector3 MoveVector = transform.TransformDirection(PlayerMovementInput) * moveSpeed;
        PlayerBody.velocity = new Vector3(MoveVector.x, PlayerBody.velocity.y, MoveVector.z);
        //StartCoroutine(cameraShake.Shake(5f, 0.4f));
    }

    private void Jump()
    {
        jumpDirection = new Vector3(PlayerMovementInput.x, 1f, PlayerMovementInput.z);
        PlayerBody.AddForce(jumpDirection * jumpForce, ForceMode.Impulse);
    }

    private void Crouch()
    {
        PlayerCollider.height = crouchHeight;
        Crouching = true;
        currentHeight = 1;
    }

    private void Stand()
    {
        PlayerCollider.height = maxStandHeight;
        standHeight = maxStandHeight;
        currentHeight = standHeight;
        Crouching = false;
    }
}

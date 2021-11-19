using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class rbSoundController : MonoBehaviour
{
    [SerializeField] rbPlayerMovement rbPlayerBody;
    [SerializeField] Rigidbody rb;

    private AudioSource walkingSound;
    public AudioClip[] footsteps;
    private AudioClip footstepClip;
    public AudioClip sprintingClip;
    private AudioSource sprintingSound;
    
    // Start is called before the first frame update
    void Start()
    {
        //playerBody = GetComponent<Rigidbody>();
        walkingSound = gameObject.GetComponent<AudioSource>();
        sprintingSound = gameObject.GetComponent<AudioSource>();
    }

    // Update is called once per frame
    void Update()
    {
        if(rbPlayerBody.isGrounded == true && rb.velocity.magnitude > 1f && GetComponent<AudioSource>().isPlaying == false && rbPlayerBody.isSprinting == false)
        {
            int index = Random.Range(0, footsteps.Length);
            footstepClip = footsteps[index];
            walkingSound.clip = footstepClip;

            walkingSound.volume = rb.velocity.magnitude/5;
            //walkingSound.pitch = rb.velocity.magnitude/2;
            
            //walkingSound.PlayDelayed(rb.velocity.magnitude/10);
            walkingSound.Play();
        }

        /*
        else if (rbPlayerBody.isGrounded == true && rbPlayerBody.isSprinting == true)
        {
            sprintingSound.clip = sprintingClip;
            sprintingSound.loop = true;
            sprintingSound.Play();
        }
        else
        {
            sprintingSound.loop = false;
            sprintingSound.Stop();
        }
        */


        if (Input.GetKeyDown(KeyCode.LeftShift) && rbPlayerBody.isGrounded == true)
        {
            sprintingSound.clip = sprintingClip;
            sprintingSound.Play();
            sprintingSound.loop = true;
        }
        else if (Input.GetKeyUp(KeyCode.LeftShift))
        {
            sprintingSound.Stop();
            sprintingSound.loop = false;
        }
        
    }
}

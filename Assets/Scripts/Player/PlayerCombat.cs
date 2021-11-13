using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerCombat : MonoBehaviour
{
    [SerializeField] GameObject weapon;
    public PlayerMovement isSprinting;
    bool weaponReady;

    private void Awake()
    {
        weaponReady = false;
    }

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        //---WEAPON READY---
        if (weaponReady == false)
        {
            weapon.SetActive(false);
        }
        
        

        if (Input.GetKey(KeyCode.LeftAlt) && isSprinting.sprinting != true)
        {
            weaponReady = true;
        }
        else
        {
            weaponReady = false;
        }

        if (weaponReady == true)
        {
            weapon.SetActive(true);
        }

        //---USE WEAPON---
        if (Input.GetMouseButton(0))
        {
            //use weapon
            UseWeapon();
        }

        
    }

    //use weapon function
    private void UseWeapon()
    {
        //get weapon type
        //detect semi or full auto
        //use weapon primary attack
    }
}

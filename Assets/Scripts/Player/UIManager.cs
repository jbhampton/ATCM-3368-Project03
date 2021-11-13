using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class UIManager : MonoBehaviour
{
    //public PlayerHealth playerHealth;
    public PlayerMovement playerMoveSpeed;
    public Slider moveSpeedSlider;
    public PlayerMovement playerStamina;
    public Slider staminaSlider;
    
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        UpdateMoveSpeedSlider();
        UpdateStaminaSlider();
    }

    public void UpdateMoveSpeedSlider()
    {
        moveSpeedSlider.value = playerMoveSpeed.moveSpeed;
    }

    public void UpdateStaminaSlider()
    {
        staminaSlider.value = playerStamina.stamina;
    }
}

using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class rbUIManager : MonoBehaviour
{
    //public PlayerHealth playerHealth;
    public rbPlayerMovement playerMoveSpeed;
    public Slider moveSpeedSlider;
    public rbPlayerMovement playerStamina;
    public Slider staminaSlider;
    public rbPlayerMovement playerHeight;
    public Slider heightSlider;
    public Image heightImage;

    // Start is called before the first frame update
    void Start()
    {

    }

    // Update is called once per frame
    void Update()
    {
        UpdateMoveSpeedSlider();
        UpdateStaminaSlider();
        UpdateHeightVisuals();
    }

    public void UpdateMoveSpeedSlider()
    {
        moveSpeedSlider.value = playerMoveSpeed.moveSpeed;
    }

    public void UpdateStaminaSlider()
    {
        staminaSlider.value = playerStamina.stamina;
    }

    public void UpdateHeightVisuals()
    {
        heightSlider.value = playerHeight.currentHeight;
        heightImage.transform.localScale = new Vector3 (1, playerHeight.currentHeight, 1);
    }
}

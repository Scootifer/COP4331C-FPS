using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class InputManager : MonoBehaviour
{
    private PlayerInput playerInput;
    private PlayerInput.OnFootActions onFoot;

    private PlayerMotor motor;
    private PlayerLook look;

    private PlayerShootingScript shootingScript;

    private int _curDelay = 0;
    private int _maxDelay = 2;

    // Start is called before the first frame update
    void Awake()
    {
        playerInput = new PlayerInput();
        onFoot = playerInput.OnFoot;

        motor = GetComponent<PlayerMotor>();
        look = GetComponent<PlayerLook>();

        onFoot.Jump.performed += ctx => motor.Jump(); // started, cancelled

        shootingScript = this.GetComponent<PlayerShootingScript>();
    }

    // Update is called once per frame
    void FixedUpdate()
    {
        if (_curDelay != _maxDelay)
        {
            _curDelay++;
            return;
        }

        // player moves using value from movement action
        motor.ProcessMove(onFoot.Movement.ReadValue<Vector2>());
    }

    private void Update()
    { 
        if (onFoot.Shoot.IsPressed()) shootingScript.shooting = true;
        else shootingScript.shooting = false;

        if (onFoot.Grenade.IsPressed()) shootingScript.grenade = true;
        else shootingScript.grenade = false;

        if (onFoot.Reload.IsPressed()) shootingScript.reloading = true;
        //else shootingScript.reloading = false;

        if (onFoot.Melee.IsPressed()) shootingScript.melee = true;
        else shootingScript.melee = false;

    }

    private void LateUpdate()
    {
        look.ProcessLook(onFoot.Look.ReadValue<Vector2>());
    }


    private void OnEnable()
    {
        onFoot.Enable();
    }

    private void OnDisable()
    {
        onFoot.Disable();
    }
}

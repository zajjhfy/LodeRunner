using System;
using UnityEngine;
using UnityEngine.InputSystem;

public class ButtonPressedEventArgs : EventArgs{
    public InputControl control;
}

public class PlayerController: MonoBehaviour
{
    private PlayerControllerIA _playerControllerIA;
    public event EventHandler<ButtonPressedEventArgs> OnBreakButtonPressed;
    public event EventHandler OnPauseButtonPressed;

    private void Start(){
        _playerControllerIA = new PlayerControllerIA();
        _playerControllerIA.Input.Enable();
        _playerControllerIA.Input.Break.performed += _playerControllerIA_Input_Break_performed;
        _playerControllerIA.Input.Pause.performed += _playerControllerIA_Input_Pause_performed;
    }

    private void _playerControllerIA_Input_Pause_performed(InputAction.CallbackContext context)
    {
        OnPauseButtonPressed?.Invoke(this, EventArgs.Empty);
    }

    private void _playerControllerIA_Input_Break_performed(InputAction.CallbackContext context)
    {
        OnBreakButtonPressed?.Invoke(this, new ButtonPressedEventArgs{control = context.control});
    }

    public Vector3 GetMovementVector(){
        return _playerControllerIA.Input.Movement.ReadValue<Vector2>();
    }
}

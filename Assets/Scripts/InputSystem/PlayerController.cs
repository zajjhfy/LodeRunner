using System;
using UnityEngine;
using UnityEngine.InputSystem;

public class ButtonPressedEventArgs : EventArgs{
    public KeyCode key;
}

public class PlayerController: MonoBehaviour
{
    private PlayerControllerIA _playerControllerIA;
    public event EventHandler<ButtonPressedEventArgs> OnBreakButtonPressed;
    public event EventHandler OnPauseButtonPressed;
    public event EventHandler OnExitButtonPressed;
    public event EventHandler OnGameStartPressed;

    private void Start(){
        _playerControllerIA = new PlayerControllerIA();
        _playerControllerIA.GameStart.Enable();
        _playerControllerIA.GameStart.Go.performed += _playerControllerIA_GameStart_Go_performed;
        _playerControllerIA.Input.Break.performed += _playerControllerIA_Input_Break_performed;
        _playerControllerIA.Input.Pause.performed += _playerControllerIA_Input_Pause_performed;
        _playerControllerIA.Input.Exit.performed += _playerControllerIA_Input_Exit_performed;
    }

    private void _playerControllerIA_GameStart_Go_performed(InputAction.CallbackContext context)
    {
        OnGameStartPressed?.Invoke(this, EventArgs.Empty);
    }

    private void _playerControllerIA_Input_Exit_performed(InputAction.CallbackContext context)
    {
        OnExitButtonPressed?.Invoke(this, EventArgs.Empty);
    }

    private void _playerControllerIA_Input_Pause_performed(InputAction.CallbackContext context)
    {
        OnPauseButtonPressed?.Invoke(this, EventArgs.Empty);
    }

    private void _playerControllerIA_Input_Break_performed(InputAction.CallbackContext context)
    {
        KeyCode keyCode = KeyCode.None;
        if(char.Parse(context.control.name) == 'z'){
            keyCode = KeyCode.Z;
        }
        else if(char.Parse(context.control.name) == 'x'){
            keyCode = KeyCode.X;
        }
        OnBreakButtonPressed?.Invoke(this, new ButtonPressedEventArgs{key = keyCode});
    }

    public Vector3 GetMovementVector(){
        return _playerControllerIA.Input.Movement.ReadValue<Vector2>();
    }

    public void DisableInputMap() => _playerControllerIA.Input.Disable();

    public void EnableInputMap() => _playerControllerIA.Input.Enable();

    public void DisableGameStartMap() => _playerControllerIA.GameStart.Disable();

}

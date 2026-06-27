using UnityEngine;
using UnityEngine.InputSystem;

public class InteractionHelpers
{
    public static Vector2 GetMousePosition()
    {
        return Camera.main.ScreenToWorldPoint(Mouse.current.position.ReadValue());
    }
}

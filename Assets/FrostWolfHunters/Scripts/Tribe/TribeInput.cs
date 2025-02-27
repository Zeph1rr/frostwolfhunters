using UnityEngine;
using UnityEngine.InputSystem;

public class TribeInput : MonoBehaviour
{
    public static Vector3 GetMousePosition() {
        Vector3 mousePos = Mouse.current.position.ReadValue();
        return mousePos;
    }


}

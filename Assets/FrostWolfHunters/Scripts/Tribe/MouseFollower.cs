using UnityEngine;

public class MouseFollower : MonoBehaviour
{
    private void Update()
    {
        Vector3 mousePos = Camera.main.ScreenToWorldPoint(TribeInput.GetMousePosition());
        transform.position = new Vector3(mousePos.x, mousePos.y, 0);
    }
}

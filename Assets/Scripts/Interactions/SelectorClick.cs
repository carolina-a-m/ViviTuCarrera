using UnityEngine;
using UnityEngine.InputSystem;

public class SelectorClick : MonoBehaviour
{
    void Update()
    {
        Vector2 posicionInput = Vector2.zero;
        bool hayInput = false;

        if (Mouse.current != null && Mouse.current.leftButton.wasPressedThisFrame)
        {
            posicionInput = Mouse.current.position.ReadValue();
            hayInput = true;
        }
        else if (Touchscreen.current != null && Touchscreen.current.primaryTouch.press.wasPressedThisFrame)
        {
            posicionInput = Touchscreen.current.primaryTouch.position.ReadValue();
            hayInput = true;
        }

        if (!hayInput) return;
        if (Camera.main == null) return;

        Ray ray = Camera.main.ScreenPointToRay(posicionInput);
        if (Physics.Raycast(ray, out RaycastHit hit) && hit.collider.gameObject == gameObject)
        {
            OpcionCubo cubo = GetComponent<OpcionCubo>();
            if (cubo != null)
            {
                GameManager.Instancia.ElegirOpcion(cubo.indiceOpcion);
            }
        }
    }
}
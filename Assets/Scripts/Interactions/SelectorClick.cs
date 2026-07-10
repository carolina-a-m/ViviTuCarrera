using UnityEngine;
using UnityEngine.InputSystem;

public class SelectorClick : MonoBehaviour
{
    void Update()
    {
        if (Mouse.current != null && Mouse.current.leftButton.wasPressedThisFrame)
        {
            Ray ray = Camera.main.ScreenPointToRay(Mouse.current.position.ReadValue());
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
}
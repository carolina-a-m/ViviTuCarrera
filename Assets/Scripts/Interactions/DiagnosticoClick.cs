using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.EventSystems;

public class DiagnosticoClick : MonoBehaviour
{
    void Update()
    {
        if (Mouse.current != null && Mouse.current.leftButton.wasPressedThisFrame)
        {
            bool sobreUI = EventSystem.current.IsPointerOverGameObject();
            Debug.Log($"Click de mouse detectado. ¿Está sobre un objeto de UI?: {sobreUI}");
        }
    }
}
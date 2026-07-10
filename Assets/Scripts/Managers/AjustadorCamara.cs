using UnityEngine;
using UnityEngine.InputSystem;
using System.Collections;

public class AjustadorCamara : MonoBehaviour
{
    [Tooltip("Ángulo horizontal mínimo deseado, en grados")]
    public float fovHorizontalDeseado = 90f;

    private Camera camara;

    void Awake()
    {
        camara = GetComponent<Camera>();
    }

    IEnumerator Start()
    {
        yield return null;
        yield return null;
        Recalcular();
    }

    void Update()
    {
        if (Keyboard.current != null && Keyboard.current.rKey.wasPressedThisFrame)
        {
            Recalcular();
        }
    }

    void Recalcular()
    {
        bool hayXRActivo = UnityEngine.XR.XRSettings.isDeviceActive;
        if (hayXRActivo) return;
        if (camara == null) return;

        float aspecto = (float)Screen.width / Screen.height;

        if (aspecto < 1f)
        {
            float fovVerticalNecesario = 2f * Mathf.Atan(Mathf.Tan(fovHorizontalDeseado * Mathf.Deg2Rad / 2f) / aspecto) * Mathf.Rad2Deg;
            camara.fieldOfView = fovVerticalNecesario;
            Debug.Log($"FOV ajustado a: {fovVerticalNecesario}");
        }
        else
        {
            Debug.Log("Aspecto >= 1, no se ajusta");
        }
    }
}
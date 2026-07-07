using UnityEngine;

public class DetectorDecision : MonoBehaviour
{
    // Referencia visual para dar feedback (cambiamos el color del plano)
    private Renderer miRenderer;

    void Start()
    {
        miRenderer = GetComponent<Renderer>();
    }

    // Se ejecuta automáticamente cuando otro objeto con Collider entra en esta zona
    private void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.name == "OpciónA")
        {
            Debug.Log("Decisión A elegida: Pedir estudios antes de diagnosticar.");
            miRenderer.material.color = Color.green;
        }
        else if (other.gameObject.name == "OpciónB")
        {
            Debug.Log("Decisión B elegida: Diagnosticar y medicar directamente.");
            miRenderer.material.color = new Color(1f, 0.6f, 0f); // naranja
        }
    }
}
using UnityEngine;

public class DetectorDecision : MonoBehaviour
{
    private void OnTriggerEnter(Collider other)
    {
        OpcionCubo cubo = other.GetComponent<OpcionCubo>();
        if (cubo == null) return;

        GameManager.Instancia.ElegirOpcion(cubo.indiceOpcion);
    }
}
using UnityEngine;

public class GameManager : MonoBehaviour
{
    public static GameManager Instancia { get; private set; }

    [Header("Escenas del recorrido, en orden")]
    public EscenaDecisionData[] escenas;

    private int indiceActual = -1;

    void Awake()
    {
        if (Instancia != null && Instancia != this)
        {
            Destroy(gameObject);
            return;
        }
        Instancia = this;
    }

    void Start()
    {
        AvanzarEscena();
    }

    public void AvanzarEscena()
    {
        indiceActual++;

        if (indiceActual >= escenas.Length)
        {
            Debug.Log("Recorrido terminado. Acá va el cierre / feedback final.");
            return;
        }

        EscenaDecisionData escenaActual = escenas[indiceActual];
        Debug.Log($"Cargando escena: {escenaActual.idEscena}");
        Debug.Log($"Contexto: {escenaActual.textoContexto}");
    }

    public EscenaDecisionData ObtenerEscenaActual()
    {
        if (indiceActual < 0 || indiceActual >= escenas.Length) return null;
        return escenas[indiceActual];
    }
}
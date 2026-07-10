using UnityEngine;
using System.Collections;
using TMPro;

public class GameManager : MonoBehaviour
{
    public static GameManager Instancia { get; private set; }

    [Header("Escenas del recorrido, en orden")]
    public EscenaDecisionData[] escenas;

    [Header("UI")]
    public TMP_Text textoContexto;
    public GameObject botonContinuar;

    private int indiceActual = -1;
    private bool esperandoAvance = false;

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

        if (textoContexto != null)
        {
            textoContexto.text = escenaActual.textoContexto;
        }

        bool esBloqueDeSoloTexto = escenaActual.opciones == null || escenaActual.opciones.Length == 0;
        if (botonContinuar != null)
        {
            botonContinuar.SetActive(esBloqueDeSoloTexto);
        }
    }

    public void ContinuarSinDecision()
    {
        AvanzarEscena();
    }

    public EscenaDecisionData ObtenerEscenaActual()
    {
        if (indiceActual < 0 || indiceActual >= escenas.Length) return null;
        return escenas[indiceActual];
    }

    public void ElegirOpcion(int indiceOpcion)
    {
        if (esperandoAvance) return;

        EscenaDecisionData escenaActual = ObtenerEscenaActual();
        if (escenaActual == null) return;

        if (indiceOpcion < 0 || indiceOpcion >= escenaActual.opciones.Length)
        {
            Debug.LogWarning($"Índice de opción fuera de rango: {indiceOpcion}");
            return;
        }

        OpcionDecision opcionElegida = escenaActual.opciones[indiceOpcion];
        esperandoAvance = true;

        if (textoContexto != null)
        {
            textoContexto.text = opcionElegida.textoFeedback;
        }

        StartCoroutine(EsperarYAvanzar());
    }

    private IEnumerator EsperarYAvanzar()
    {
        yield return new WaitForSeconds(4f);
        esperandoAvance = false;
        AvanzarEscena();
    }
}
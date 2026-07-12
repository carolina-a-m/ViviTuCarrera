using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using TMPro;

public class GameManager : MonoBehaviour
{
    public static GameManager Instancia { get; private set; }

    [Header("Escenas del recorrido, en orden")]
    public EscenaDecisionData[] escenas;

    [Header("UI")]
    public TMP_Text textoContexto;
    public GameObject botonContinuar;

    [Header("Puntos de spawn para los prefabs de cada escena")]
    public Transform[] puntosSpawn;

    private int indiceActual = -1;
    private bool esperandoAvance = false;
    private List<GameObject> objetosInstanciados = new List<GameObject>();

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
        LimpiarEscenarioAnterior();

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

        InstanciarEscenario(escenaActual);
    }

    private void LimpiarEscenarioAnterior()
    {
        foreach (GameObject obj in objetosInstanciados)
        {
            if (obj != null)
            {
                Destroy(obj);
            }
        }
        objetosInstanciados.Clear();
    }

    private void InstanciarEscenario(EscenaDecisionData escena)
    {
        if (escena.prefabsEscenario == null) return;

        for (int i = 0; i < escena.prefabsEscenario.Length; i++)
        {
            GameObject prefab = escena.prefabsEscenario[i];
            if (prefab == null) continue;

            if (puntosSpawn == null || i >= puntosSpawn.Length || puntosSpawn[i] == null)
            {
                Debug.LogWarning($"No hay Punto Spawn definido para el índice {i}");
                continue;
            }

            Transform spawn = puntosSpawn[i];
            GameObject instancia = Instantiate(prefab, spawn.position, spawn.rotation);
            objetosInstanciados.Add(instancia);
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
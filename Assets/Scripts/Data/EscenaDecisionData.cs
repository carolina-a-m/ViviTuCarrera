using UnityEngine;

[CreateAssetMenu(fileName = "NuevaEscenaDecision", menuName = "ViviTuCarrera/Escena de Decision")]
public class EscenaDecisionData : ScriptableObject
{
    [Header("Identificación")]
    public string idEscena;

    [Header("Contexto")]
    [TextArea(3, 6)]
    public string textoContexto;

    [Header("Escenario (prefabs a instanciar en esta escena)")]
    public GameObject[] prefabsEscenario;

    [Header("Opciones")]
    public OpcionDecision[] opciones;
}

[System.Serializable]
public class OpcionDecision
{
    public string nombreCorto;
    public Color colorCubo = Color.white;

    [TextArea(3, 8)]
    public string textoFeedback;

    public bool esOpcionEsperada;
}
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.InputSystem.EnhancedTouch;

public class SelectorClick : MonoBehaviour
{
    private bool agarrado = false;
    private bool usandoTouch = false;
    private Vector3 posicionOriginal;
    private Plane planoArrastre;
    private Rigidbody rb;

    private const float ALTURA_ZONA = 0.3f;
    private const string TAG_ZONA = "ZonaDecision";

    void Awake()
    {
        rb = GetComponent<Rigidbody>();
        EnhancedTouchSupport.Enable();

        if (rb != null)
        {
            rb.isKinematic = true;
        }
    }

    void Update()
    {
        Vector2 posicionInput = Vector2.zero;
        bool inputPresionado = false;
        bool inputSoltado = false;
        bool haySeguimiento = false;

        // Mouse y touch se chequean de forma INDEPENDIENTE (no else-if a nivel global),
        // porque algunos navegadores de PC exponen un Touchscreen "fantasma"
        // que nunca se presiona, y un else-if bloquearía el mouse real.
        if (Mouse.current != null && Mouse.current.leftButton.wasPressedThisFrame)
        {
            posicionInput = Mouse.current.position.ReadValue();
            inputPresionado = true;
        }
        else if (Touchscreen.current != null && Touchscreen.current.primaryTouch.press.wasPressedThisFrame)
        {
            posicionInput = Touchscreen.current.primaryTouch.position.ReadValue();
            inputPresionado = true;
        }

        if (!agarrado && inputPresionado)
        {
            if (Camera.main == null) return;

            Ray ray = Camera.main.ScreenPointToRay(posicionInput);
            if (Physics.Raycast(ray, out RaycastHit hit) && hit.collider.gameObject == gameObject)
            {
                // Guardamos con qué dispositivo se inició el agarre: única fuente de verdad
                // para todo el ciclo de arrastre/suelte. Evita que un mouse fantasma en WebGL
                // mobile le gane la mano al touch real durante el arrastre.
                usandoTouch = (Touchscreen.current != null && Touchscreen.current.primaryTouch.press.wasPressedThisFrame);
                AgarrarCubo();
            }
            return;
        }

        if (agarrado)
        {
            // isPressed (no solo "el device existe") para no seguir arrastrando
            // con datos viejos si el evento de suelte se llegara a perder.
            if (usandoTouch && Touchscreen.current != null && Touchscreen.current.primaryTouch.press.isPressed)
            {
                posicionInput = Touchscreen.current.primaryTouch.position.ReadValue();
                haySeguimiento = true;
                inputSoltado = Touchscreen.current.primaryTouch.press.wasReleasedThisFrame;
            }
            else if (!usandoTouch && Mouse.current != null && Mouse.current.leftButton.isPressed)
            {
                posicionInput = Mouse.current.position.ReadValue();
                haySeguimiento = true;
                inputSoltado = Mouse.current.leftButton.wasReleasedThisFrame;
            }
            else
            {
                // Se perdió el input sin pasar por wasReleasedThisFrame (ej: el
                // dedo se levantó fuera de foco). Soltamos igual para no quedar pegado.
                inputSoltado = true;
                haySeguimiento = true;
                posicionInput = ScreenPosicionActual();
            }

            if (haySeguimiento)
            {
                ArrastrarCubo(posicionInput);

                if (inputSoltado)
                {
                    SoltarCubo(posicionInput);
                    usandoTouch = false;
                }
            }
        }
    }

    private void AgarrarCubo()
    {
        agarrado = true;
        posicionOriginal = transform.position;
        planoArrastre = new Plane(Vector3.up, new Vector3(0f, transform.position.y, 0f));

        if (rb != null)
        {
            rb.isKinematic = true;
        }

        EscenaDecisionData escenaActual = GameManager.Instancia.ObtenerEscenaActual();
        if (escenaActual != null && escenaActual.idEscena == "practica_tutorial")
        {
            if (GameManager.Instancia.textoContexto != null)
            {
                GameManager.Instancia.textoContexto.text = "¡Bien! Ahora arrastralo hasta la zona marcada.";
            }
        }
    }

    private void ArrastrarCubo(Vector2 posicionPantalla)
    {
        if (Camera.main == null) return;

        Ray ray = Camera.main.ScreenPointToRay(posicionPantalla);
        if (planoArrastre.Raycast(ray, out float distancia))
        {
            Vector3 puntoMundo = ray.GetPoint(distancia);
            transform.position = puntoMundo;
        }
    }

    private void SoltarCubo(Vector2 posicionPantalla)
    {
        bool cayoEnZona = false;

        if (Camera.main != null)
        {
            Ray ray = Camera.main.ScreenPointToRay(posicionPantalla);
            // RaycastAll: con el ray pasando justo por el cubo que estamos soltando,
            // un Raycast simple suele pegarle al propio cubo antes que a la zona
            // debajo. Filtramos explícitamente por tag y descartamos nuestro collider.
            RaycastHit[] hits = Physics.RaycastAll(ray);
            foreach (RaycastHit hit in hits)
            {
                if (hit.collider.gameObject != gameObject && hit.collider.CompareTag(TAG_ZONA))
                {
                    cayoEnZona = true;
                    break;
                }
            }
        }

        if (cayoEnZona)
        {
            Vector3 posFinal = transform.position;
            posFinal.y = ALTURA_ZONA;
            transform.position = posFinal;

            OpcionCubo cubo = GetComponent<OpcionCubo>();
            if (cubo != null)
            {
                GameManager.Instancia.ElegirOpcion(cubo.indiceOpcion);
            }
        }
        else
        {
            VolverAPosicionOriginal();
        }

        // Todo el sistema es script-driven (nunca dependimos de física real),
        // así que el Rigidbody se queda kinematic siempre. Nada de gravedad
        // peleando contra la reposición manual.
        if (rb != null)
        {
            rb.isKinematic = true;
        }

        agarrado = false;
        usandoTouch = false;
    }

    private void VolverAPosicionOriginal()
    {
        transform.position = posicionOriginal;
    }

    private Vector2 ScreenPosicionActual()
    {
        if (usandoTouch && Touchscreen.current != null)
        {
            return Touchscreen.current.primaryTouch.position.ReadValue();
        }
        if (Mouse.current != null)
        {
            return Mouse.current.position.ReadValue();
        }
        return Vector2.zero;
    }
}
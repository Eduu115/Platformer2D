using UnityEngine;

public class NivelManager : MonoBehaviour
{
    public static NivelManager Instance;

    private int totalPlatanos;
    private int platanosRecogidos;

    private void Awake()
    {
        Instance = this;
    }

    private void Start()
    {
        // Cuenta automáticamente todos los plátanos en la escena
        totalPlatanos = FindObjectsOfType<Platano>().Length;
    }

    public void RecogerPlatano()
    {
        platanosRecogidos++;
    }

    public bool TodosRecogidos()
    {
        return platanosRecogidos >= totalPlatanos;
    }
}
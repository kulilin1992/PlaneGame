using UnityEngine;

public class OverdriveMaterialController : MonoBehaviour
{
    [SerializeField] Material overdriveMaterial;
    
    Material defaultMaterial;

    new Renderer renderer;
    
    void Awake()
    {
        renderer = GetComponent<Renderer>();
        defaultMaterial = renderer.material;
    }
    
    void OnEnable()
    {
        PlayerPowerDrive.on += PlayerOverdriveOn;
        PlayerPowerDrive.off += PlayerOverdriveOff;
    }

    void OnDisable()
    {
        PlayerPowerDrive.on -= PlayerOverdriveOn;
        PlayerPowerDrive.off -= PlayerOverdriveOff;     
    }

    void PlayerOverdriveOn() => renderer.material = overdriveMaterial;

    void PlayerOverdriveOff() => renderer.material = defaultMaterial;
}
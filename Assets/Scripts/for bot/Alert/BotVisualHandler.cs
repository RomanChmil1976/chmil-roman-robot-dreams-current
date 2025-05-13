using UnityEngine;

public class BotVisualHandler : MonoBehaviour
{
    [SerializeField] private Renderer[] eyeRenderers;
    [SerializeField] private Material alertMaterial;
    [SerializeField] private Material defaultMaterial;

    public void SetAlertVisuals(bool alert)
    {
        Material mat = alert ? alertMaterial : defaultMaterial;
        foreach (var rend in eyeRenderers)
        {
            if (rend != null) rend.material = mat;
        }
    }
}
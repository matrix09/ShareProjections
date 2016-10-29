using UnityEngine;

[ExecuteInEditMode]
public class CustomImageEffect : MonoBehaviour
{


    public Material EffectMaterial;
    
    void OnRenderImage(RenderTexture src, RenderTexture dst)
    {
        if (null != EffectMaterial)
            Graphics.Blit(src, dst, EffectMaterial);
    }
}




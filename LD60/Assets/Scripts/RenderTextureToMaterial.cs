using UnityEngine;

public class RenderTextureToMaterial : MonoBehaviour
{

    public Material material;
    public RenderTexture rt;

    private Texture2D ConvertRenderTextureToTexture2D(RenderTexture renderTex)
    {
        
        var prevActive = RenderTexture.active;
        
        RenderTexture.active = renderTex;
        var texture = new Texture2D(renderTex.width, renderTex.height, TextureFormat.RGBA32, false);
        texture.ReadPixels(new Rect(0, 0, renderTex.width, renderTex.height), 0, 0);
        texture.Apply();

        RenderTexture.active = prevActive;
        
        return texture;
    }
    
    private void Update()
    {
        material.mainTexture = ConvertRenderTextureToTexture2D(rt);
    }
}

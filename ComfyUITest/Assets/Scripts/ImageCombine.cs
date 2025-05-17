using UnityEngine;
using UnityEngine.UI;
using System;
using System.IO;

public class ImageCombine : MonoBehaviour
{
    [Header("UI References")]
    public RawImage baseRawImage;      // background
    public RawImage overlayRawImage;   // the graphic with alpha
    public RawImage resultRawImage;    // where to preview
    public UploadImage m_imageUpload;

    [Header("Scaling Settings")]
    [Range(0.1f, 1f)]
    public float fillPercentage = 0.9f;

    [Header("Vertical Offset")]
    public int yOffset = 0;
    // Positive moves the overlay up from center; negative moves it down.

    [ContextMenu("Composite → Scale & Center → Display & Save")]
    public void CompositeScaleCenterSave()
    {
        // 1) get CPU‐readable copies
        var baseTex = GetReadableTexture(baseRawImage.texture);
        var overlayTex = GetReadableTexture(overlayRawImage.texture);
        if (baseTex == null || overlayTex == null)
        {
            Debug.LogError("Both base and overlay must be Texture2D (or RenderTexture) and valid.");
            return;
        }

        int W = baseTex.width;
        int H = baseTex.height;

        // 2) compute scale factor
        float maxW = W * fillPercentage;
        float maxH = H * fillPercentage;
        float scale = Mathf.Min(maxW / overlayTex.width, maxH / overlayTex.height);

        int newW = Mathf.RoundToInt(overlayTex.width * scale);
        int newH = Mathf.RoundToInt(overlayTex.height * scale);

        // 3) scale the overlay
        Texture2D scaledOverlay = ResizeTexture(overlayTex, newW, newH);

        // 4) prepare the result
        var result = new Texture2D(W, H, TextureFormat.RGBA32, false);
        result.SetPixels(baseTex.GetPixels());

        // 5) compute offsets
        int offsetX = (W - newW) / 2;              // always centered horizontally
        int offsetY = (H - newH) / 2 + yOffset;    // centered + manual tweak

        // 6) blend
        for (int y = 0; y < newH; y++)
            for (int x = 0; x < newW; x++)
            {
                Color o = scaledOverlay.GetPixel(x, y);
                if (o.a <= 0f) continue;

                int dx = offsetX + x;
                int dy = offsetY + y;
                if (dx < 0 || dx >= W || dy < 0 || dy >= H) continue;

                Color b = result.GetPixel(dx, dy);
                result.SetPixel(dx, dy, Color.Lerp(b, o, o.a));
            }

        result.Apply();

        // 7) preview
        if (resultRawImage != null)
        {
            resultRawImage.texture = result;
            resultRawImage.SetNativeSize();
        }

        // 8) save
        string imagesDir = Path.Combine(Application.streamingAssetsPath, "images");
        if (!Directory.Exists(imagesDir))
            Directory.CreateDirectory(imagesDir);

        string ts = DateTime.Now.ToString("yyyyMMdd_HHmmss");
        string fileName = $"overlayed_{ts}.png";
        string path = Path.Combine(imagesDir, fileName);
        File.WriteAllBytes(path, result.EncodeToPNG());
        m_imageUpload.StartUpload(fileName);
        Debug.Log($"Saved scaled & centered overlay to: {path}");
    }

    private Texture2D GetReadableTexture(Texture tex)
    {
        if (tex is Texture2D t2d && t2d.isReadable)
            return t2d;

        int w = tex.width, h = tex.height;
        var rt = new RenderTexture(w, h, 0, RenderTextureFormat.ARGB32);
        Graphics.Blit(tex, rt);
        var prev = RenderTexture.active;
        RenderTexture.active = rt;
        var copy = new Texture2D(w, h, TextureFormat.RGBA32, false);
        copy.ReadPixels(new Rect(0, 0, w, h), 0, 0);
        copy.Apply();
        RenderTexture.active = prev;
        rt.Release();
        return copy;
    }

    private Texture2D ResizeTexture(Texture2D source, int newW, int newH)
    {
        var rt = new RenderTexture(newW, newH, 0, RenderTextureFormat.ARGB32);
        Graphics.Blit(source, rt);
        var prev = RenderTexture.active;
        RenderTexture.active = rt;
        var result = new Texture2D(newW, newH, TextureFormat.RGBA32, false);
        result.ReadPixels(new Rect(0, 0, newW, newH), 0, 0);
        result.Apply();
        RenderTexture.active = prev;
        rt.Release();
        return result;
    }
}

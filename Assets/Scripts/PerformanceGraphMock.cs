using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

[RequireComponent(typeof(RawImage))]
public class PerformanceGraphTexture : MonoBehaviour
{
    public int width = 640;
    public int height = 320;
    public int maxPoints = 300;

    [Header("Colors")]
    public Color background = new Color(0.10f, 0.12f, 0.16f, 1f);
    public Color accuracyColor = new Color(0.00f, 0.78f, 0.67f, 1f);
    public Color fatigueColor  = new Color(0.96f, 0.35f, 0.33f, 1f);
    public Color velocityColor = new Color(0.20f, 0.55f, 0.95f, 1f);

    [Range(1, 4)] public int lineThickness = 2;

    struct Sample { public float a,f,v; public Sample(float A,float F,float V){a=A;f=F;v=V;} }
    readonly List<Sample> samples = new List<Sample>();
    public bool HasSamples => samples.Count > 0;
    Texture2D tex;
    RawImage img;

    void Awake()
    {
        img = GetComponent<RawImage>();
        tex = new Texture2D(width, height, TextureFormat.RGBA32, false);
        tex.wrapMode = TextureWrapMode.Clamp;
        img.texture = tex;
        Clear();
    }

    public void Clear()
    {
        Color[] fill = new Color[width * height];
        for (int i=0;i<fill.Length;i++) fill[i] = background;
        tex.SetPixels(fill);
        tex.Apply();
        samples.Clear();
    }

    public void AddSample(float accuracy01, float fatigue01, float velocity01)
    {
        samples.Add(new Sample(Mathf.Clamp01(accuracy01), Mathf.Clamp01(fatigue01), Mathf.Clamp01(velocity01)));
        if (samples.Count > maxPoints) samples.RemoveAt(0);
        Redraw();
    }

    void Redraw()
    {
        // clear
        Color[] fill = new Color[width * height];
        for (int i=0;i<fill.Length;i++) fill[i] = background;
        tex.SetPixels(fill);

        // plot three lines
        Plot(samples, s => s.a, accuracyColor);
        Plot(samples, s => s.f, fatigueColor);
        Plot(samples, s => s.v, velocityColor);

        tex.Apply();
    }

    delegate float Channel(Sample s);
    void Plot(List<Sample> data, Channel ch, Color col)
    {
        if (data.Count < 2) return;

        int n = data.Count;
        float xStep = (float)width / Mathf.Max(1, maxPoints - 1);

        int prevX = 0, prevY = (int)(ch(data[0]) * (height - 1));
        for (int i = 1; i < n; i++)
        {
            int x = Mathf.Clamp((int)(i * xStep), 0, width - 1);
            int y = Mathf.Clamp((int)(ch(data[i]) * (height - 1)), 0, height - 1);
            DrawLine(prevX, prevY, x, y, col);
            prevX = x; prevY = y;
        }
    }

    // Bresenham with thickness
    void DrawLine(int x0, int y0, int x1, int y1, Color col)
    {
        int dx = Mathf.Abs(x1 - x0), dy = -Mathf.Abs(y1 - y0);
        int sx = x0 < x1 ? 1 : -1;
        int sy = y0 < y1 ? 1 : -1;
        int err = dx + dy, e2;

        while (true)
        {
            DrawThick(x0, y0, col);
            if (x0 == x1 && y0 == y1) break;
            e2 = 2 * err;
            if (e2 >= dy) { err += dy; x0 += sx; }
            if (e2 <= dx) { err += dx; y0 += sy; }
        }
    }

    void DrawThick(int x, int y, Color col)
    {
        int r = Mathf.Max(1, lineThickness);
        for (int oy = -r; oy <= r; oy++)
        for (int ox = -r; ox <= r; ox++)
        {
            int xx = x + ox, yy = y + oy;
            if (xx < 0 || xx >= width || yy < 0 || yy >= height) continue;
            tex.SetPixel(xx, yy, col);
        }
    }
}

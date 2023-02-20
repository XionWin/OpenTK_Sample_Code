using Common;
using OpenTK.Graphics.OpenGL4;
using OpenTK.Windowing.Common;
using System.Drawing;

namespace HelloTriangle;
public class GLWindow : Window
{
    public GLWindow() : base ("Hello Triangle")
    {}

    private readonly float[] _vertices =
    {
        -0.5f, -0.5f, 0.0f, // Bottom-left vertex
        0.5f, -0.5f, 0.0f, // Bottom-right vertex
        0.0f,  0.5f, 0.0f  // Top vertex
    };

    protected override void OnRenderFrame(FrameEventArgs args)
    {
        base.OnRenderFrame(args);

        GL.ClearColor(Color.MidnightBlue);
        GL.Clear(ClearBufferMask.ColorBufferBit);

        SwapBuffers();
    }

    protected override void OnUpdateFrame(FrameEventArgs args)
    {
        base.OnUpdateFrame(args);
    }
}





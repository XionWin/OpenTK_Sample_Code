using OpenTK.Graphics.OpenGL4;
using OpenTK.Mathematics;
using OpenTK.Windowing.Common;
using OpenTK.Windowing.Desktop;
using SemanticExtension;
using System.Drawing;

namespace WindowCreation;
public class GLWindow : GameWindow
{
    public GLWindow() : this ("Window Creation", 720, 720, "Resources/Icon.png")
    {}

    private GLWindow(string title, int width, int height, string? iconPath = null) :
        base(
            GameWindowSettings.Default,
            new NativeWindowSettings()
            {
                Title = title,
                Size = new Vector2i(width, height),
                API = ContextAPI.OpenGL,
                APIVersion = new Version(4, 5),
                Icon = iconPath?.Then(x => WindowExtension.CreateWindowIcon(x)),
            }
        )
    {}

    protected override void OnRenderFrame(FrameEventArgs args)
    {
        var version = this.APIVersion;
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





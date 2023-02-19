using OpenTK.Graphics.OpenGL4;
using OpenTK.Mathematics;
using OpenTK.Windowing.Common;
using OpenTK.Windowing.Desktop;
using SemanticExtension;
using System.Drawing;

namespace WindowCreation;
internal class Window : GameWindow
{
    public Window(string title, int width, int height, string? iconPath = null) :
        base(
            GameWindowSettings.Default,
            new NativeWindowSettings()
            {
                Title = title,
                Size = new Vector2i(width, height),
                API = ContextAPI.OpenGLES,
                APIVersion = new Version(3, 2),
                Icon = iconPath?.Then(x => WindowExtension.CreateWindowIcon(x)),
            }
        )
    {
        this.Title = title;
    }

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





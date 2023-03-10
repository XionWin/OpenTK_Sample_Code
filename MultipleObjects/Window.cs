using Common;
using MultipleObjects.Objects;
using OpenTK.Graphics.OpenGL4;
using OpenTK.Windowing.Common;
using OpenTK.Windowing.GraphicsLibraryFramework;
using System.Diagnostics;
using System.Drawing;
using System.Numerics;

namespace MultipleObjects;
public class Window : GLWindow
{
    public Window() : base("MultipleObjects")
    { }

    private IRenderObject _renderObject = new RenderObject();
    private IRenderObject _renderObject2 = new RenderObject2();

    private int _uniformViewPort;

    protected override void OnLoad()
    {
        base.OnLoad();

        GL.ClearColor(Color.MidnightBlue);

        _renderObject.OnLoad(this.Shader);

        _renderObject2.OnLoad(this.Shader);

        this._uniformViewPort = GL.GetUniformLocation(this.Shader.ProgramHandle, "aViewport");
    }

    protected override void OnRenderFrame(FrameEventArgs args)
    {
        base.OnRenderFrame(args);

        GL.Clear(ClearBufferMask.ColorBufferBit);

        _renderObject.OnRenderFrame(this.Shader);
        _renderObject2.OnRenderFrame(this.Shader);

        SwapBuffers();
    }

    protected override void OnUpdateFrame(FrameEventArgs args)
    {
        base.OnUpdateFrame(args);

        if (this.KeyboardState.IsKeyDown(Keys.Escape))
        {
            Close();
        }
    }

    protected override void OnResize(ResizeEventArgs e)
    {
        base.OnResize(e);

        // When the window gets resized, we have to call GL.Viewport to resize OpenGL's viewport to match the new size.
        // If we don't, the NDC will no longer be correct.
        GL.Viewport(0, 0, Size.X, Size.Y);
        GL.Uniform3(this._uniformViewPort, this.Size.X, this.Size.Y, 1.0f);
    }

    protected override void OnUnload()
    {
        GL.UseProgram(Shader.ProgramHandle);
        // Unbind all the resources by binding the targets to 0/null.
        _renderObject.OnUnload();
        _renderObject2.OnUnload();

        GL.DeleteProgram(Shader.ProgramHandle);

        base.OnUnload();
    }
}





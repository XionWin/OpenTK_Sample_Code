using Shapes.Objects;
using Common;
using Extension;
using OpenTK.Graphics.OpenGL4;
using OpenTK.Windowing.Common;
using OpenTK.Windowing.GraphicsLibraryFramework;
using System;
using System.Drawing;
using static System.Collections.Specialized.BitVector32;

namespace Shapes;
public class Window : GLWindow
{
    private const int ZOOM_FACTOR = 2;

    private readonly static Texture TEXTURE_0 = new Texture(TextureUnit.Texture0, TextureMinFilter.Nearest).With(x => x.Load("Resources/Bg.png"));
    public Window() : base("Shapes", 288 * ZOOM_FACTOR * 2, 200 * ZOOM_FACTOR * 2)
    {
        this._renderObjects.Add(new SceneObject(new SizeF(288 * ZOOM_FACTOR * 2, 200 * ZOOM_FACTOR * 2), new RectangleF(0, 0, 288, 200), TEXTURE_0));
    }


    private List<IRenderObject> _renderObjects = new List<IRenderObject>();

    private int _uniformViewPort;

    protected override void OnLoad()
    {
        base.OnLoad();

        _renderObjects.Add(new ShapeObject(new SizeF(200, 200), new RectangleF(0, 0, 288, 200), TEXTURE_0) { Location = new PointF(50, 50) });
        _renderObjects.Add(new ShapeObject(new SizeF(100, 100), new RectangleF(0, 0, 288, 200), TEXTURE_0) { Location = new PointF(100, 100) });
        _renderObjects.Add(new ShapeObject(new SizeF(50, 50), new RectangleF(0, 0, 288, 200), TEXTURE_0) { Location = new PointF(125, 125) });
        _renderObjects.Add(new ShapeObject(new SizeF(10, 10), new RectangleF(0, 0, 288, 200), TEXTURE_0) { Location = new PointF(145, 145) });


        GL.ClearColor(Color.FromArgb(96, 96, 168));

        foreach (var renderObject in _renderObjects)
        {
            renderObject.OnLoad(this.Shader);
        }

        this._uniformViewPort = GL.GetUniformLocation(this.Shader.ProgramHandle, "aViewport");
    }

    protected override void OnRenderFrame(FrameEventArgs args)
    {
        base.OnRenderFrame(args);

        GL.Clear(ClearBufferMask.ColorBufferBit);

        var tick = DateTime.Now.Ticks / 1200000;
        foreach (var renderObject in _renderObjects)
        {
            renderObject.OnRenderFrame(this.Shader);
        }

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
        foreach (var renderObject in _renderObjects)
        {
            renderObject.OnUnload();
        }

        GL.DeleteProgram(Shader.ProgramHandle);

        base.OnUnload();
    }
}





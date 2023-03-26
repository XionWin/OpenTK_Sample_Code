using Shapes.Objects;
using Common;
using Extension;
using OpenTK.Graphics.OpenGL4;
using OpenTK.Windowing.Common;
using OpenTK.Windowing.GraphicsLibraryFramework;
using System;
using System.Drawing;
using static System.Collections.Specialized.BitVector32;
using System.Diagnostics;

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

        Random random = new Random();
        for (int i = 0; i < 100; i++)
        {
            var p1 = new Point(random.Next(this.Size.X), random.Next(this.Size.Y));
            var p2 = new Point(random.Next(this.Size.X), random.Next(this.Size.Y));


            _renderObjects.Add(new PointObject(p1, new OpenTK.Mathematics.Vector3(1, 1, 1)) { Size = 20 });
            _renderObjects.Add(new PointObject(p2, new OpenTK.Mathematics.Vector3(1, 1, 1)) { Size = 20 });
            _renderObjects.Add(new LineObject(p1, p2 , new OpenTK.Mathematics.Vector3(1, 1, 1)));
        }


        GL.ClearColor(Color.FromArgb(96, 96, 168));

        foreach (var renderObject in _renderObjects)
        {
            renderObject.OnLoad(this.Shader);
        }

        this._uniformViewPort = GL.GetUniformLocation(this.Shader.ProgramHandle, "aViewport");

        watch.Start();
    }

    int fpsCounter = 0;
    Stopwatch watch = new Stopwatch();
    protected override void OnRenderFrame(FrameEventArgs args)
    {
        base.OnRenderFrame(args);

        GL.Clear(ClearBufferMask.ColorBufferBit);

        Random random = new Random();
        foreach (var renderObject in _renderObjects)
        {
            if (renderObject is PointObject pointObject)
            {
                pointObject.Location = new Point(random.Next(this.Size.X), random.Next(this.Size.Y));
            }
            if (renderObject is LineObject lineObject)
            {
                var p1 = new Point(random.Next(this.Size.X), random.Next(this.Size.Y));
                var p2 = new Point(random.Next(this.Size.X), random.Next(this.Size.Y));
                lineObject.Start = p1;
                lineObject.End = p2;
            }
            renderObject.OnRenderFrame(this.Shader);
        }

        fpsCounter++;
        if (fpsCounter == 60)
        {
            
            System.Console.WriteLine((float)fpsCounter / watch.ElapsedMilliseconds * 1000);
            fpsCounter = 0;
            watch.Restart();
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





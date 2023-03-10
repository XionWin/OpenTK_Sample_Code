using Common;
using OpenTK.Graphics.OpenGL4;
using OpenTK.Windowing.Common;
using OpenTK.Windowing.GraphicsLibraryFramework;
using System.Diagnostics;
using System.Drawing;
using System.Numerics;

namespace Textures;
public class Window : GLWindow
{
    public Window() : base("Textures")
    { }

    private readonly Vertex2[] _vertices = new[]
    {
        new Vertex2(new Vector2(620f, 100f), new Vector2(1.0f, 0.0f)),
        new Vertex2(new Vector2(620f, 620f), new Vector2(1.0f, 1.0f)),
        new Vertex2(new Vector2(100f, 620f), new Vector2(0.0f, 1.0f)),
        new Vertex2(new Vector2(100f, 100f), new Vector2(0.0f, 0.0f)),
    };

    private readonly uint[] _indices =
    {
        0, 1, 3,
        1, 2, 3
    };

    private int _vbo;

    private int _vao;

    private int _ebo;

    private Texture? _texture;
    private Texture? _texture2;

    private int _uniformViewPort;

    protected override void OnLoad()
    {
        base.OnLoad();

        GL.ClearColor(Color.MidnightBlue);

        _vao = GL.GenVertexArray();
        _vbo = GL.GenBuffer();
        _ebo = GL.GenBuffer();

        GL.BindVertexArray(_vao);

        // bind vbo and set data for vbo
        GL.BindBuffer(BufferTarget.ArrayBuffer, _vbo);
        var vertices = _vertices.GetRaw();
        GL.BufferData(BufferTarget.ArrayBuffer, vertices.Length * sizeof(float), vertices, BufferUsageHint.StaticDraw);

        // bind ebo and set data for ebo
        GL.BindBuffer(BufferTarget.ElementArrayBuffer, _ebo);
        GL.BufferData(BufferTarget.ElementArrayBuffer, _indices.Length * sizeof(uint), _indices, BufferUsageHint.StaticDraw);

        this.Shader.EnableAttribs(Vertex2.AttribLocations);

        _texture = Texture.Load("Resources/container.png", TextureUnit.Texture0);
        _texture.Use(TextureUnit.Texture0);


        _texture2 = Texture.Load("Resources/container2.png", TextureUnit.Texture1);
        _texture2.Use(TextureUnit.Texture1);


        this.Shader.SetInt("texture0", 0);
        this.Shader.SetInt("texture1", 1);

        this._uniformViewPort = GL.GetUniformLocation(this.Shader.ProgramHandle, "aViewport");
    }

    protected override void OnRenderFrame(FrameEventArgs args)
    {
        base.OnRenderFrame(args);

        GL.Clear(ClearBufferMask.ColorBufferBit);

        // Bind the VAO
        GL.BindVertexArray(_vao);

        // Enable Alpha
        GL.Enable(EnableCap.Blend);
        GL.BlendFunc(BlendingFactor.SrcAlpha, BlendingFactor.OneMinusSrcAlpha);

        GL.DrawElements(PrimitiveType.Triangles, _indices.Length, DrawElementsType.UnsignedInt, 0);

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
        // Unbind all the resources by binding the targets to 0/null.
        GL.BindBuffer(BufferTarget.ArrayBuffer, 0);
        GL.BindVertexArray(0);
        GL.UseProgram(0);

        // Delete all the resources.
        GL.DeleteBuffer(_vbo);
        GL.DeleteVertexArray(_vao);

        GL.DeleteProgram(Shader.ProgramHandle);

        base.OnUnload();
    }
}





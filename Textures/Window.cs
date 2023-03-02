using Common;
using OpenTK.Graphics.OpenGL4;
using OpenTK.Windowing.Common;
using OpenTK.Windowing.GraphicsLibraryFramework;
using System.Diagnostics;
using System.Drawing;

namespace Textures;
public class Window : GLWindow
{
    public Window() : base("Textures")
    { }

    private readonly float[] _vertices =
    {
        // positions        Texture coordinates
        620f, 100f,         0.0f, 1.0f, 0.0f, // top right
        620f, 620f,         0.0f, 1.0f, 1.0f, // bottom right
        100f, 620f,         0.0f, 0.0f, 1.0f, // bottom left
        100f, 100f,         0.0f, 0.0f, 0.0f  // top left
    };

    private readonly uint[] _indices =
    {
        0, 1, 3,
        1, 2, 3
    };

    private int _vbo;

    private int _vao;

    private int _ebo;

    private Texture _texture;

    private int _uniformViewPort;

    protected override void OnLoad()
    {
        base.OnLoad();

        GL.ClearColor(Color.MidnightBlue);

        _vbo = GL.GenBuffer();
        _vao = GL.GenVertexArray();
        _ebo = GL.GenBuffer();

        GL.BindVertexArray(_vao);

        // bind vbo and set data for vbo
        GL.BindBuffer(BufferTarget.ArrayBuffer, _vbo);
        GL.BufferData(BufferTarget.ArrayBuffer, _vertices.Length * sizeof(float), _vertices, BufferUsageHint.StaticDraw);

        // bind ebo and set data for ebo
        GL.BindBuffer(BufferTarget.ElementArrayBuffer, _ebo);
        GL.BufferData(BufferTarget.ElementArrayBuffer, _indices.Length * sizeof(uint), _indices, BufferUsageHint.StaticDraw);

        // Because there's now 5 floats between the start of the first vertex and the start of the second,
        // we modify the stride from 3 * sizeof(float) to 5 * sizeof(float).
        // This will now pass the new vertex array to the buffer.
        var vertexLocation = this.Shader.GetAttribLocation("aPosition");
        GL.EnableVertexAttribArray(vertexLocation);
        GL.VertexAttribPointer(vertexLocation, 3, VertexAttribPointerType.Float, false, 5 * sizeof(float), 0);

        // Next, we also setup texture coordinates. It works in much the same way.
        // We add an offset of 3, since the texture coordinates comes after the position data.
        // We also change the amount of data to 2 because there's only 2 floats for texture coordinates.
        var texCoordLocation = this.Shader.GetAttribLocation("aTexCoord");
        GL.EnableVertexAttribArray(texCoordLocation);
        GL.VertexAttribPointer(texCoordLocation, 2, VertexAttribPointerType.Float, false, 5 * sizeof(float), 3 * sizeof(float));

        _texture = Texture.Load("Resources/container.png");
        _texture.Use(TextureUnit.Texture0);

        this._uniformViewPort = GL.GetUniformLocation(this.Shader.ProgramHandle, "aViewport");

    }

    protected override void OnRenderFrame(FrameEventArgs args)
    {
        base.OnRenderFrame(args);

        GL.Clear(ClearBufferMask.ColorBufferBit);

        // Bind the VAO
        GL.BindVertexArray(_vao);
        _texture.Use(TextureUnit.Texture0);


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





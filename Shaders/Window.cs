using Common;
using OpenTK.Graphics.OpenGL4;
using OpenTK.Windowing.Common;
using OpenTK.Windowing.GraphicsLibraryFramework;
using System.Diagnostics;
using System.Drawing;

namespace Shaders;
public class Window : GLWindow
{
    public Window() : base("Shaders")
    { }

    private readonly float[] _vertices =
    {
             // positions        // colors
             360f, 200f,        1.0f, 0.0f, 0.0f,   // bottom right
             560f, 600f,      0.0f, 1.0f, 0.0f,   // bottom left
             160f, 600f,    0.0f, 0.0f, 1.0f    // top 
        };

    private int _vbo;

    private int _vao;

    private int _vp;

    protected override void OnLoad()
    {
        base.OnLoad();

        GL.ClearColor(Color.MidnightBlue);

        _vbo = GL.GenBuffer();
        _vao = GL.GenVertexArray();

        // bind vbo and set data for vbo
        GL.BindBuffer(BufferTarget.ArrayBuffer, _vbo);
        GL.BufferData(BufferTarget.ArrayBuffer, _vertices.Length * sizeof(float), _vertices, BufferUsageHint.StaticDraw);

        // bind vao and set data for vao[0]
        GL.BindVertexArray(_vao);
        GL.VertexAttribPointer(0, 2, VertexAttribPointerType.Float, false, 5 * sizeof(float), 0);
        GL.EnableVertexAttribArray(0);

        // We create a new pointer for the color values.
        // Much like the previous pointer, we assign 6 in the stride value.
        // We also need to correctly set the offset to get the color values.
        // The color data starts after the position data, so the offset is the size of 3 floats.
        GL.VertexAttribPointer(1, 3, VertexAttribPointerType.Float, false, 5 * sizeof(float), 2 * sizeof(float));
        // We then enable color attribute (location=1) so it is available to the shader.
        GL.EnableVertexAttribArray(1);

        // Vertex attributes are the data we send as input into the vertex shader from the main program.
        // So here we're checking to see how many vertex attributes our hardware can handle.
        // OpenGL at minimum supports 16 vertex attributes. This only needs to be called 
        // when your intensive attribute work and need to know exactly how many are available to you.
        GL.GetInteger(GetPName.MaxVertexAttribs, out int maxAttributeCount);
        Debug.WriteLine($"Maximum number of vertex attributes supported: {maxAttributeCount}");

        this._vp = GL.GetUniformLocation(this.Shader.ProgramHandle, "aViewport");
    }

    protected override void OnRenderFrame(FrameEventArgs args)
    {
        base.OnRenderFrame(args);

        GL.Clear(ClearBufferMask.ColorBufferBit);

        // Bind the VAO
        GL.BindVertexArray(_vao);

        GL.DrawArrays(PrimitiveType.Triangles, 0, 3);

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
        GL.Uniform3(this._vp, this.Size.X, this.Size.Y, 1.0f);
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





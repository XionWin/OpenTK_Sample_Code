using Common;
using OpenTK.Graphics.OpenGL4;
using OpenTK.Windowing.Common;
using OpenTK.Windowing.GraphicsLibraryFramework;
using System.Drawing;

namespace ElementBuffer;
public class Window : GLWindow
{
    public Window() : base("Element Buffer")
    { }

    private readonly float[] _vertices =
    {
        .5f, .5f, 0f,   //top right
        .5f, -.5f, 0f,  //bottom right
        -.5f, -.5f, 0f,  //bottom left
        -.5f, .5f, 0f   //top left
    };

    //  3-----------0
    //  |           |
    //  |           |
    //  |           |
    //  2-----------1

    private readonly uint[] _indices =
    {
        0, 1, 3,  // TR -> BR -> TL
        1, 2, 3   // BR -> BL -> TL
    };

    private int _vbo;

    private int _vao;

    // Add a handle for the EBO
    private int _ebo;

    protected override void OnLoad()
    {
        base.OnLoad();

        GL.ClearColor(Color.MidnightBlue);

        _vbo = GL.GenBuffer();
        _vao = GL.GenVertexArray();
        _ebo = GL.GenBuffer();

        GL.BindBuffer(BufferTarget.ArrayBuffer, _vbo);
        GL.BufferData(BufferTarget.ArrayBuffer, _vertices.Length * sizeof(float), _vertices, BufferUsageHint.StaticDraw);

        GL.BindVertexArray(_vao);
        GL.VertexAttribPointer(0, 3, VertexAttribPointerType.Float, false, 3 * sizeof(float), 0);
        GL.EnableVertexAttribArray(0);

        // We create/bind the Element Buffer Object EBO the same way as the VBO, except there is a major difference here which can be REALLY confusing.
        // The binding spot for ElementArrayBuffer is not actually a global binding spot like ArrayBuffer is. 
        // Instead it's actually a property of the currently bound VertexArrayObject, and binding an EBO with no VAO is undefined behaviour.
        // This also means that if you bind another VAO, the current ElementArrayBuffer is going to change with it.
        // Another sneaky part is that you don't need to unbind the buffer in ElementArrayBuffer as unbinding the VAO is going to do this,
        // and unbinding the EBO will remove it from the VAO instead of unbinding it like you would for VBOs or VAOs.
        GL.BindBuffer(BufferTarget.ElementArrayBuffer, _ebo);
        // We also upload data to the EBO the same way as we did with VBOs.
        GL.BufferData(BufferTarget.ElementArrayBuffer, _indices.Length * sizeof(uint), _indices, BufferUsageHint.StaticDraw);
        // The EBO has now been properly setup. Go to the Render function to see how we draw our rectangle now!

        this.Shader.Use();
    }

    protected override void OnRenderFrame(FrameEventArgs args)
    {
        base.OnRenderFrame(args);

        GL.Clear(ClearBufferMask.ColorBufferBit);


        this.Shader.Use();

        // Bind the VAO
        GL.BindVertexArray(_vao);

        // And then call our drawing function.
        // For this tutorial, we'll use GL.DrawArrays, which is a very simple rendering function.
        // Arguments:
        //   Primitive type; What sort of geometric primitive the vertices represent.
        //     OpenGL used to support many different primitive types, but almost all of the ones still supported
        //     is some variant of a triangle. Since we just want a single triangle, we use Triangles.
        //   Starting index; this is just the start of the data you want to draw. 0 here.
        //   How many vertices you want to draw. 3 for a triangle.
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





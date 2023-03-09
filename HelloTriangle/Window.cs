using Common;
using OpenTK.Graphics.OpenGL4;
using OpenTK.Windowing.Common;
using OpenTK.Windowing.GraphicsLibraryFramework;
using System.Drawing;

namespace HelloTriangle;
public class Window : GLWindow
{
    public Window() : base ("HelloTriangle")
    {}

    private readonly float[] _vertices =
    {
        -0.5f, -0.5f, 0.0f, // Bottom-left vertex
        0.5f, -0.5f, 0.0f, // Bottom-right vertex
        0.0f,  0.5f, 0.0f  // Top vertex
    };

    private int _vbo;

    private int _vao;

    protected override void OnLoad()
    {
        base.OnLoad();

        GL.ClearColor(Color.MidnightBlue);


        // We need to send our vertices over to the graphics card so OpenGL can use them.
        // To do this, we need to create what's called a Vertex Buffer Object (VBO).
        // These allow you to upload a bunch of data to a buffer, and send the buffer to the graphics card.
        // This effectively sends all the vertices at the same time.

        // First, we need to create a buffer. This function returns a handle to it, but as of right now, it's empty.
        _vbo = GL.GenBuffer();

        // Now, bind the buffer. OpenGL uses one global state, so after calling this,
        // all future calls that modify the VBO will be applied to this buffer until another buffer is bound instead.
        // The first argument is an enum, specifying what type of buffer we're binding. A VBO is an ArrayBuffer.
        // There are multiple types of buffers, but for now, only the VBO is necessary.
        // The second argument is the handle to our buffer.
        GL.BindBuffer(BufferTarget.ArrayBuffer, _vbo);

        // Finally, upload the vertices to the buffer.
        // Arguments:
        //   Which buffer the data should be sent to.
        //   How much data is being sent, in bytes. You can generally set this to the length of your array, multiplied by sizeof(array type).
        //   The vertices themselves.
        //   How the buffer will be used, so that OpenGL can write the data to the proper memory space on the GPU.
        //   There are three different BufferUsageHints for drawing:
        //     StaticDraw: This buffer will rarely, if ever, update after being initially uploaded.
        //     DynamicDraw: This buffer will change frequently after being initially uploaded.
        //     StreamDraw: This buffer will change on every frame.
        //   Writing to the proper memory space is important! Generally, you'll only want StaticDraw,
        //   but be sure to use the right one for your use case.
        GL.BufferData(BufferTarget.ArrayBuffer, _vertices.Length * sizeof(float), _vertices, BufferUsageHint.StaticDraw);

        // One notable thing about the buffer we just loaded data into is that it doesn't have any structure to it. It's just a bunch of floats (which are actaully just bytes).
        // The opengl driver doesn't know how this data should be interpreted or how it should be divided up into vertices. To do this opengl introduces the idea of a 
        // Vertex Array Obejct (VAO) which has the job of keeping track of what parts or what buffers correspond to what data. In this example we want to set our VAO up so that 
        // it tells opengl that we want to interpret 12 bytes as 3 floats and divide the buffer into vertices using that.
        // To do this we generate and bind a VAO (which looks deceptivly similar to creating and binding a VBO, but they are different!).
        _vao = GL.GenVertexArray();
        GL.BindVertexArray(_vao);

        // Now, we need to setup how the vertex shader will interpret the VBO data; you can send almost any C datatype (and a few non-C ones too) to it.
        // While this makes them incredibly flexible, it means we have to specify how that data will be mapped to the shader's input variables.

        // To do this, we use the GL.VertexAttribPointer function
        // This function has two jobs, to tell opengl about the format of the data, but also to associate the current array buffer with the VAO.
        // This means that after this call, we have setup this attribute to source data from the current array buffer and interpret it in the way we specified.
        // Arguments:
        //   Location of the input variable in the shader. the layout(location = 0) line in the vertex shader explicitly sets it to 0.
        //   How many elements will be sent to the variable. In this case, 3 floats for every vertex.
        //   The data type of the elements set, in this case float.
        //   Whether or not the data should be converted to normalized device coordinates. In this case, false, because that's already done.
        //   The stride; this is how many bytes are between the last element of one vertex and the first element of the next. 3 * sizeof(float) in this case.
        //   The offset; this is how many bytes it should skip to find the first element of the first vertex. 0 as of right now.
        // Stride and Offset are just sort of glossed over for now, but when we get into texture coordinates they'll be shown in better detail.
        GL.VertexAttribPointer(0, 3, VertexAttribPointerType.Float, false, 3 * sizeof(float), 0);

        // Enable variable 0 in the shader.
        GL.EnableVertexAttribArray(0);
    }

    protected override void OnRenderFrame(FrameEventArgs args)
    {
        base.OnRenderFrame(args);

        GL.Clear(ClearBufferMask.ColorBufferBit);

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





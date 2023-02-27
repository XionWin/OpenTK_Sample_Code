using Extension;
using OpenTK.Compute.OpenCL;
using OpenTK.Graphics.OpenGL4;
using System.Reflection.Metadata;
using static Extension.SemanticExtension;

namespace Common;

public class Shader
{
    public int ProgramHandle { get; init; }

    public readonly Dictionary<string, int> UniformLocations = new Dictionary<string, int>();

    public Shader(string vertexPath, string fragmentPath)
    {
        if (File.ReadAllText(vertexPath) is string vertexSource && File.ReadAllText(fragmentPath) is string fragmentSource)
        {
            var vertexShader = vertexSource.CompileShader(ShaderType.VertexShader);
            var fragmentShader = fragmentSource.CompileShader(ShaderType.FragmentShader);
            // Next, allocate the dictionary to hold the locations.
            this.ProgramHandle = GL.CreateProgram();
            this.LinkProgram(vertexShader, fragmentShader);
        }
        else
            throw new Exception($"Create shader failed");
    }

    public void Use()
    {
        GL.UseProgram(ProgramHandle);
    }
}

static class ShaderExtension
{
    public static int CompileShader(this string shaderSource, ShaderType shaderType) =>
        GL.CreateShader(shaderType) is var shader
        && shaderSource
        .With(x => GL.ShaderSource(shader, shaderSource))
        .Then(x => CompileShader(shader)) is var code
        && code is (int)All.True ?
            shader :
            throw new Exception($"Error occurred whilst compiling Shader({shader}).\n\n{GL.GetShaderInfoLog(shader)}");

    private static int CompileShader(int shader)
    {
        // Try to compile the shader
        GL.CompileShader(shader);

        // Check for compilation errors
        GL.GetShader(shader, ShaderParameter.CompileStatus, out var code);
        return code;
    }

    public static void LinkProgram(this Shader shader, int vertexShader, int fragmentShader)
    {
        // Attach both shaders...
        GL.AttachShader(shader.ProgramHandle, vertexShader);
        GL.AttachShader(shader.ProgramHandle, fragmentShader);

        // And then link them together.
        var code = LinkProgram(shader.ProgramHandle);
        if (code is not (int)All.True)
        {
            // We can use `GL.GetProgramInfoLog(program)` to get information about the error.
            throw new Exception($"Error occurred whilst linking Program({shader.ProgramHandle})");
        }

        // When the shader program is linked, it no longer needs the individual shaders attached to it; the compiled code is copied into the shader program.
        // Detach them, and then delete them.
        GL.DetachShader(shader.ProgramHandle, vertexShader);
        GL.DetachShader(shader.ProgramHandle, fragmentShader);
        GL.DeleteShader(fragmentShader);
        GL.DeleteShader(vertexShader);

        // The shader is now ready to go, but first, we're going to cache all the shader uniform locations.
        // Querying this from the shader is very slow, so we do it once on initialization and reuse those values
        // later.

        // First, we have to get the number of active uniforms in the shader.
        GL.GetProgram(shader.ProgramHandle, GetProgramParameterName.ActiveUniforms, out var numberOfUniforms);

        // Loop over all the uniforms,
        for (var i = 0; i < numberOfUniforms; i++)
        {
            // get the name of this uniform,
            var key = GL.GetActiveUniform(shader.ProgramHandle, i, out _, out _);

            // get the location,
            var location = GL.GetUniformLocation(shader.ProgramHandle, key);

            // and then add it to the dictionary.
            shader.UniformLocations.Add(key, location);

        }
    }

    private static int LinkProgram(int program)
    {
        // We link the program
        GL.LinkProgram(program);

        // Check for linking errors
        GL.GetProgram(program, GetProgramParameterName.LinkStatus, out var code);
        return code;
    }
}
using Character.Objects;
using Common;
using Extension;
using OpenTK.Graphics.OpenGL4;
using OpenTK.Windowing.Common;
using OpenTK.Windowing.GraphicsLibraryFramework;
using System.Drawing;

namespace Character;
public class Window : GLWindow
{
    private const int ZOOM_FACTOR = 4;

    private readonly static Texture TEXTURE_0 = new Texture(TextureUnit.Texture0, TextureMinFilter.Nearest).With(x => x.Load("Resources/Bg.png"));
    private readonly static Texture TEXTURE_1 = new Texture(TextureUnit.Texture1, TextureMinFilter.Nearest).With(x => x.Load("Resources/Character.png"));
    private readonly static Texture TEXTURE_2 = new Texture(TextureUnit.Texture2, TextureMinFilter.Nearest).With(x => x.Load("Resources/Shield.png"));
    public Window() : base("Character", 288 * ZOOM_FACTOR, 200 * ZOOM_FACTOR)
    {
        this._renderObjects.Add(new SceneObject(new SizeF(288 * ZOOM_FACTOR, 200 * ZOOM_FACTOR), new RectangleF(0, 0, 288, 200), TEXTURE_0));
    }

    private static CharacterObject[] _renderObjectSamples = new[]
    {
        new CharacterObject(new SizeF(64 * ZOOM_FACTOR, 64 * ZOOM_FACTOR), new RectangleF(0, 0, 64, 64), TEXTURE_1),
    };

    private List<IRenderObject> _renderObjects = new List<IRenderObject>();

    private int _uniformViewPort;

    protected override void OnLoad()
    {
        base.OnLoad();

        var random = new Random();

        var len = _renderObjectSamples.Length;
        for (int i = 0; i < 50; i++)
        {
            var index = random.NextInt64(len);
            var original = _renderObjectSamples[index];
            var action = random.Next(21);
            var item = new CharacterObject(original.Size, original.Coordinate, original.Texture)
            {
                Action = action,
                Location = new PointF(random.Next((int)(this.Size.X - original.Size.Width)), random.Next((int)(this.Size.Y - original.Size.Height))),
            };
            var equipment = new EquipmentObject(original.Size, original.Coordinate, TEXTURE_2)
            {
                Action = action,
                Location = new PointF(item.Location.X - 0, item.Location.Y + 0),
            };
            _renderObjects.Add(item);
            _renderObjects.Add(equipment);
        }

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

        var tick = DateTime.Now.Ticks / 800000;
        foreach (var renderObject in _renderObjects)
        {
            renderObject.Tick = tick;
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





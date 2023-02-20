using Extension;
using OpenTK.Mathematics;
using OpenTK.Windowing.Common;
using OpenTK.Windowing.Desktop;

namespace Common;
public class Window : GameWindow
{
    public Window(string title, int? width = null, int? height = null, string? iconPath = null) : this(title, width ?? 720, height ?? 720, iconPath ?? "Resources/Images/Icon.png")
    { }

    public Shader Shader { get; init; }

    private Window(string title, int width, int height, string? iconPath = null) :
        base(
            GameWindowSettings.Default,
            new NativeWindowSettings()
            {
                Title = title,
                Size = new Vector2i(width, height),
                WindowBorder = WindowBorder.Fixed,
                API = ContextAPI.OpenGL,
                APIVersion = new Version(4, 5),
                Icon = iconPath?.Then(x => WindowExtension.CreateWindowIcon(x)),
            }
        )
    {
        this.Title = this.Title + $" | {this.API} {this.APIVersion.Major}.{this.APIVersion.Minor}";
        this.Shader = new Shader("Shaders/shader.vert", "Shaders/shader.frag");
    }
}

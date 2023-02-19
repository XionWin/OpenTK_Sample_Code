using OpenTK.Mathematics;
using OpenTK.Windowing.Common;
using OpenTK.Windowing.Desktop;

namespace WindowCreation;

internal class Program
{
    static void Main(string[] args)
    {

        // To create a new window, create a class that extends GameWindow, then call Run() on it.
        using (var window = new GLWindow("Window Creation", 720, 720, "Resources/Icon.png"))
        {
            window.Run();
        }
    }
}
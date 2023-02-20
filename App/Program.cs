namespace App;
internal class Program
{
    static void Main(string[] args)
    {

        // To create a new window, create a class that extends GameWindow, then call Run() on it.
        using (var window = new HelloTriangle.GLWindow())
        {
            window.Run();
        }
    }
}

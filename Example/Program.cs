using System;
using Treehouse.Runtime.System;
using Treehouse.Runtime.System.Windowing;

namespace Treehouse.Example;

public class Program
{
    [STAThread]
    private static void Main(string[] args)
    {
        bool isRunning = true;

        using (Engine engine = new Engine())
        {
            engine.AddSubsystem<WindowSubsystem>();

            WindowSubsystem windowSubmodule = engine.GetSubsystem<WindowSubsystem>()!;

            Window window1 = windowSubmodule.CreateWindow(new WindowOptions());
            window1.OnClosed += () => { isRunning = false; };

            Window window2 = windowSubmodule.CreateWindow(new WindowOptions());
            window2.OnClosed += () => { Console.WriteLine("Window 2 has been closed!");};

            while (isRunning)
            {
                engine.Tick();
            }
        }
    }
}

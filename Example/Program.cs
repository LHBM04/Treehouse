using Treehouse.Runtime.System;
using Treehouse.Runtime.System.Windowing;

namespace Treehouse.Example;

public class Program
{
    private static void Main(string[] args)
    {
        bool isRunning = true;

        using (Engine engine = new Engine())
        {
            engine.AddSubmodule<WindowSubmodule>();

            WindowSubmodule windowSubmodule = engine.GetSubmodule<WindowSubmodule>()!;
            windowSubmodule.OnWindowRemoved += _ => isRunning = false;

            windowSubmodule.CreateWindow(new WindowOptions());

            while (isRunning)
            {
                engine.Tick();
            }
        }
    }
}

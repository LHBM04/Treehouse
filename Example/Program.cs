using System;
using System.Numerics;
using Treehouse.Runtime.System;
using Treehouse.Runtime.System.Rendering;
using Treehouse.Runtime.System.Windowing;

namespace Treehouse.Example;

public class Program
{
    /// <summary>
    /// 해당 프로그램의 주 창.
    /// </summary>
    private static Window? mMainWindow = null;

    /// <summary>
    /// 프로그램 가동 중인가 여부.
    /// </summary>
    private static bool mIsRunning = false;

    [STAThread]
    private static void Main(string[] args)
    {
        using (Engine engine = new Engine())
        {
            engine.AddSubsystem<WindowSubsystem>();
            engine.AddSubsystem<RenderSubsystem>();

            WindowSubsystem? windowSubsystem = engine.GetSubsystem<WindowSubsystem>();
            if (windowSubsystem == null)
            {
                throw new Exception("Window Subsystem이 시스템에 등록되지 않았습니다!");
            }

            RenderSubsystem? renderSubsystem = engine.GetSubsystem<RenderSubsystem>();
            if (renderSubsystem == null)
            {
                throw new Exception("Render Subsystem이 시스템에 등록되지 않았습니다!");
            }

            windowSubsystem.OnWindowCreated += (Window window) =>
            {
                renderSubsystem.CreateOpenGL(window);
            };
            windowSubsystem.OnWindowClosed += (Window window) =>
            {
                renderSubsystem.DestroyOpenGL(window);
                mIsRunning = false;
            };

            WindowOptions windowOptions = new WindowOptions
            {
                Title = "Treehouse Example #1",
                Position = new Vector2(100, 100),
                Size = new Vector2(1280, 720),
                Flags = WindowFlags.None
            };

            mMainWindow = Window.Create(windowOptions);
            windowSubsystem.AddWindow(mMainWindow);

            mIsRunning = true;
            while (mIsRunning)
            {
                engine.Tick();
            }
        }
    }
}

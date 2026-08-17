using System;
using Treehouse.Runtime.Maths;
using Treehouse.Runtime.Core;
using Treehouse.Runtime.Core.Rendering;
using Treehouse.Runtime.Core.Windowing;

namespace Treehouse.Example;

public static class Core
{
    /// <summary>
    /// 해당 프로그램의 메인 창.
    /// </summary>
    private static Window? mMainWindow = null;

    /// <summary>
    /// 해당 프로그램의 서브 창.
    /// </summary>
    private static Window? mSubWindow = null;

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
            windowSubsystem.OnInitialize();

            RenderSubsystem? renderSubsystem = engine.GetSubsystem<RenderSubsystem>();
            if (renderSubsystem == null)
            {
                throw new Exception("Render Subsystem이 시스템에 등록되지 않았습니다!");
            }
            renderSubsystem.OnInitialize();

            windowSubsystem.OnWindowAdded += (Window window) =>
            {
                renderSubsystem.AddRenderer(window);
            };
            windowSubsystem.OnWindowRemoved += (Window window) =>
            {
                renderSubsystem.DestroyOpenGL(window);

                if (window == mMainWindow)
                {
                    mIsRunning = false;
                }
                else if (window == mSubWindow)
                {
                    Console.WriteLine("서브 창 파괴!");
                }
            };

            WindowOptions windowOptions = new WindowOptions
            {
                Title = "Treehouse Example #1",
                Position = new Vector2D<int>(100, 100),
                Size = new Vector2D<int>(1280, 720),
                Flags = WindowFlags.None
            };

            mMainWindow = Window.Create(windowOptions);
            windowSubsystem.AddWindow(mMainWindow);

            mSubWindow = Window.Create(windowOptions);
            windowSubsystem.AddWindow(mSubWindow);

            mIsRunning = true;
            while (mIsRunning)
            {
                engine.Tick();
            }
        }
    }
}

using System;
using Treehouse.Runtime.Core.Rendering;
using Treehouse.Runtime.Core.Windowing;
using Treehouse.Runtime.Maths;

namespace Treehouse.Runtime.Core;

/// <summary>
/// 
/// </summary>
public static class Entry
{
    /// <summary>
    /// 
    /// </summary>
    public static Engine Engine { get; private set; }

    /// <summary>
    /// 
    /// </summary>
    public static Game Game { get; private set; }

    /// <summary>
    /// 엔진을 돌립니다.
    /// </summary>
    /// <param name="args"></param>
    public static void Run(string[] args)
    {
        using (Engine = new Engine())
        {
            Engine.AddSubsystem<WindowSubsystem>();
            Engine.AddSubsystem<RenderSubsystem>();

            Engine.Initialize();

            GameOptions gameOptions = new GameOptions
            {
                Title = "Treehouse Game",
                Resolution = new Vector2D<int>(1280, 720),
                Flags = ScreenFlags.Windowed
            };

            using (Game = new Game())
            {
                WindowSubsystem windowSubsystem = Engine.GetSubsystem<WindowSubsystem>();
                if (windowSubsystem == null)
                {
                    throw new NullReferenceException("창 서브시스템이 null입니다!");
                }

                RenderSubsystem renderSubsystem = Engine.GetSubsystem<RenderSubsystem>();
                if (renderSubsystem == null)
                {
                    throw new NullReferenceException("렌더 서브시스템이 null입니다!");
                }

                windowSubsystem.OnWindowAdded += (Window window) =>
                {
                    Game.IsRunning = true;
                    renderSubsystem.AddRenderer(window);
                };
                windowSubsystem.OnWindowRemoved += (Window window) =>
                {
                    renderSubsystem.DestroyOpenGL(window);
                    Game.IsRunning = false;
                };

                WindowOptions windowOptions = new WindowOptions
                {
                    Title = gameOptions.Title,
                    Position = new Vector2D<int>(100, 100),
                    Size = gameOptions.Resolution,
                    Flags = WindowFlags.None
                };

                windowSubsystem.AddWindow(Window.Create(windowOptions));

                // Engine.AddSubsystem<SceneSubsystem>();
                // Engine.AddSubsystem<AudioSubsystem>();

                Game.Initialize();
                Game.IsRunning = true;

                while (Game.IsRunning)
                {
                    Engine.Tick();
                    Game.Tick();
                }

                Game.Release();
            }

            Engine.Release();
        }
    }
}

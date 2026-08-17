using System;
using System.IO;
using System.Text.Json;
using Treehouse.Runtime.Core.Rendering;
using Treehouse.Runtime.Core.Windowing;

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
    public static GameSettings GameSettings { get; private set; }

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

            using (Game = new Game())
            {
                string gameOptionsText = File.ReadAllText("./Config/GameSettings.json");

                GameSettings = JsonSerializer.Deserialize<GameSettings>(gameOptionsText)!;
                if (GameSettings == null)
                {
                    throw new NullReferenceException("GameSettings.json이 없습니다!");
                }

                WindowSubsystem? windowSubsystem = Engine.GetSubsystem<WindowSubsystem>();
                if (windowSubsystem == null)
                {
                    throw new NullReferenceException("창 서브시스템이 null입니다!");
                }

                RenderSubsystem? renderSubsystem = Engine.GetSubsystem<RenderSubsystem>();
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
                    Title = GameSettings.DisplayedTitle,
                    PositionX = 100,
                    PositionY = 100,
                    SizeX = GameSettings.ScreenSizeX,
                    SizeY = GameSettings.ScreenSizeY,
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

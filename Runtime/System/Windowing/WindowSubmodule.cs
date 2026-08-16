using System;
using System.Collections.Generic;
using System.Linq;
using System.Numerics;
using SDL3;

namespace Treehouse.Runtime.System.Windowing;

/// <summary>
/// 애플리케이션 내 창을 제어하는 서브모듈을 정의합니다.
/// </summary>
public class WindowSubmodule : EngineSubmodule
{
    /// <summary>
    /// 해당 서브모듈이 관리하는 모든 창.
    /// </summary>
    private readonly List<Window> mWindows;

    /// <summary>
    /// 창이 생성될 때 호출되는 이벤트.
    /// </summary>
    public event Action<Window>? OnWindowCreated;

    /// <summary>
    /// 창이 닫힐 때 호출되는 이벤트.
    /// </summary>
    public event Action<Window>? OnWindowClosed;

    /// <summary>
    /// 창이 추가될 때 호출되는 이벤트.
    /// </summary>
    public event Action<Window>? OnWindowAdded;

    /// <summary>
    /// 창이 제거될 때 호출되는 이벤트.
    /// </summary>
    public event Action<Window>? OnWindowRemoved;

    /// <summary>
    /// 창의 위치가 변경되었을 때 호출되는 이벤트.
    /// </summary>
    public event Action<Window, Vector2>? OnWindowMoved;

    /// <summary>
    /// 창의 크기가 변경되었을 때 호출되는 이벤트.
    /// </summary>
    public event Action<Window, Vector2>? OnWindowResized;

    /// <summary>
    /// 창의 포커스 상태가 변경되었을 때 호출되는 이벤트.
    /// </summary>
    public event Action<Window, bool>? OnWindowFocusChanged;

    public WindowSubmodule()
    {
        mWindows = new List<Window>();
    }

    /// <summary>
    /// 
    /// </summary>
    /// <param name="options"></param>
    /// <returns></returns>
    public Window CreateWindow(WindowOptions options)
    {
        uint properties = SDL.CreateProperties();

        SDL.SetStringProperty(properties, "SDL_PROP_WINDOW_CREATE_TITLE_STRING", options.Title ?? "New Treehouse Window");

        SDL.SetFloatProperty(properties, "SDL_PROP_WINDOW_CREATE_X_NUMBER", options.Position.X);
        SDL.SetFloatProperty(properties, "SDL_PROP_WINDOW_CREATE_Y_NUMBER", options.Position.Y);
        SDL.SetFloatProperty(properties, "SDL_PROP_WINDOW_CREATE_WIDTH_NUMBER", options.Size.X);
        SDL.SetFloatProperty(properties, "SDL_PROP_WINDOW_CREATE_HEIGHT_NUMBER", options.Size.Y);

        SDL.SetBooleanProperty(properties, "SDL_PROP_WINDOW_CREATE_FULLSCREEN_BOOLEAN", options.Flags.HasFlag(WindowFlags.Fullscreen));
        SDL.SetBooleanProperty(properties, "SDL_PROP_WINDOW_CREATE_RESIZABLE_BOOLEAN", options.Flags.HasFlag(WindowFlags.Resizable));
        SDL.SetBooleanProperty(properties, "SDL_PROP_WINDOW_CREATE_BORDERLESS_BOOLEAN", options.Flags.HasFlag(WindowFlags.Borderless));
        SDL.SetBooleanProperty(properties, "SDL_PROP_WINDOW_CREATE_HIDDEN_BOOLEAN", options.Flags.HasFlag(WindowFlags.Hidden));
        SDL.SetBooleanProperty(properties, "SDL_PROP_WINDOW_CREATE_ALWAYS_ON_TOP_BOOLEAN", options.Flags.HasFlag(WindowFlags.AlwaysOnTop));

        SDL.SetBooleanProperty(properties, "SDL_PROP_WINDOW_CREATE_HIGH_PIXEL_DENSITY_BOOLEAN", false);

        Window window = new Window(SDL.CreateWindowWithProperties(properties));
        OnWindowCreated?.Invoke(window);

        mWindows.Add(window);
        OnWindowAdded?.Invoke(window);

        return window;
    }

    /// <summary>
    /// 
    /// </summary>
    /// <param name="window"></param>
    public void DestroyWindow(Window window)
    {
        if (mWindows.Remove(window))
        {
            OnWindowRemoved?.Invoke(window);
            window.Dispose();
        }
    }

    internal override void OnInitialize()
    {
        base.OnInitialize();

        if (!SDL.InitSubSystem(SDL.InitFlags.Video | SDL.InitFlags.Events))
        {
            throw new InvalidOperationException("SDL 서브시스템 초기화에 실패했습니다!");
        }
    }

    internal override void OnTick()
    {
        base.OnTick();

        SDL.Event @event;
        while (SDL.PollEvent(out @event))
        {
            Window? target = mWindows.FirstOrDefault(window => window.ID == @event.Window.WindowID);
            
            switch ((SDL.EventType)@event.Type)
            {
                case SDL.EventType.WindowMoved:
                {
                    if (target != null)
                    {
                        OnWindowMoved?.Invoke(target, new Vector2(@event.Window.Data1, @event.Window.Data2));
                    }

                    break;
                }
                case SDL.EventType.WindowResized:
                {
                    if (target != null)
                    {
                        OnWindowResized?.Invoke(target, new Vector2(@event.Window.Data1, @event.Window.Data2));
                    }

                    break;
                }
                case SDL.EventType.WindowCloseRequested:
                {
                    if (target != null)
                    {
                        OnWindowClosed?.Invoke(target);
                        DestroyWindow(target);
                    }

                    break;
                }
                case SDL.EventType.WindowFocusGained:
                {
                    if (target != null)
                    {
                        OnWindowFocusChanged?.Invoke(target, true);
                    }

                    break;
                }
                case SDL.EventType.WindowFocusLost:
                {
                    if (target != null)
                    {
                        OnWindowFocusChanged?.Invoke(target, false);
                    }

                    break;
                }
            }
        }
    }

    internal override void OnRelease()
    {
        base.OnRelease();

        foreach (Window window in mWindows)
        {
            window.Close();
            window.Dispose();
        }

        mWindows.Clear();

        SDL.QuitSubSystem(SDL.InitFlags.Video | SDL.InitFlags.Events);
    }
}

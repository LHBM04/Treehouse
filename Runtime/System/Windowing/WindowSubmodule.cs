using System;
using System.Collections.Generic;
using System.Linq;
using System.Numerics;

using static SDL3.SDL;

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
        uint properties = SDL_CreateProperties();

        SDL_SetStringProperty(properties, SDL_PROP_WINDOW_CREATE_TITLE_STRING, options.Title ?? "New Treehouse Window");

        SDL_SetFloatProperty(properties, SDL_PROP_WINDOW_CREATE_X_NUMBER, options.Position.X);
        SDL_SetFloatProperty(properties, SDL_PROP_WINDOW_CREATE_Y_NUMBER, options.Position.Y);
        SDL_SetFloatProperty(properties, SDL_PROP_WINDOW_CREATE_WIDTH_NUMBER, options.Size.X);
        SDL_SetFloatProperty(properties, SDL_PROP_WINDOW_CREATE_HEIGHT_NUMBER, options.Size.Y);

        SDL_SetBooleanProperty(properties, SDL_PROP_WINDOW_CREATE_FULLSCREEN_BOOLEAN, options.Flags.HasFlag(WindowFlags.Fullscreen));
        SDL_SetBooleanProperty(properties, SDL_PROP_WINDOW_CREATE_RESIZABLE_BOOLEAN, options.Flags.HasFlag(WindowFlags.Resizable));
        SDL_SetBooleanProperty(properties, SDL_PROP_WINDOW_CREATE_BORDERLESS_BOOLEAN, options.Flags.HasFlag(WindowFlags.Borderless));
        SDL_SetBooleanProperty(properties, SDL_PROP_WINDOW_CREATE_HIDDEN_BOOLEAN, options.Flags.HasFlag(WindowFlags.Hidden));
        SDL_SetBooleanProperty(properties, SDL_PROP_WINDOW_CREATE_ALWAYS_ON_TOP_BOOLEAN, options.Flags.HasFlag(WindowFlags.AlwaysOnTop));

        SDL_SetBooleanProperty(properties, SDL_PROP_WINDOW_CREATE_HIGH_PIXEL_DENSITY_BOOLEAN, false);

        Window window = new Window(SDL_CreateWindowWithProperties(properties));
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

        if (!SDL_InitSubSystem(SDL_InitFlags.SDL_INIT_VIDEO | SDL_InitFlags.SDL_INIT_EVENTS))
        {
            throw new InvalidOperationException("SDL 서브시스템 초기화에 실패했습니다!");
        }
    }

    internal override void OnTick()
    {
        base.OnTick();

        SDL_Event @event;
        while (SDL_PollEvent(out @event))
        {
            Window? target = mWindows.FirstOrDefault(window => window.ID == @event.window.windowID);
            
            switch ((SDL_EventType)@event.type)
            {
                case SDL_EventType.SDL_EVENT_WINDOW_MOVED:
                {
                    if (target != null)
                    {
                        OnWindowMoved?.Invoke(target, new Vector2(@event.window.data1, @event.window.data2));
                    }

                    break;
                }
                case SDL_EventType.SDL_EVENT_WINDOW_RESIZED:
                {
                    if (target != null)
                    {
                        OnWindowResized?.Invoke(target, new Vector2(@event.window.data1, @event.window.data2));
                    }

                    break;
                }
                case SDL_EventType.SDL_EVENT_WINDOW_CLOSE_REQUESTED:
                {
                    if (target != null)
                    {
                        OnWindowClosed?.Invoke(target);
                        DestroyWindow(target);
                    }

                    break;
                }
                case SDL_EventType.SDL_EVENT_WINDOW_FOCUS_GAINED:
                {
                    if (target != null)
                    {
                        OnWindowFocusChanged?.Invoke(target, true);
                    }

                    break;
                }
                case SDL_EventType.SDL_EVENT_WINDOW_FOCUS_LOST:
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

        SDL_QuitSubSystem(SDL_InitFlags.SDL_INIT_VIDEO | SDL_InitFlags.SDL_INIT_EVENTS);
    }
}

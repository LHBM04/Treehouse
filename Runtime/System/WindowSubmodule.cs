using System;
using System.Collections.Generic;
using System.Linq;

using static SDL3.SDL;

namespace Treehouse.Runtime.System;

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
    /// 
    /// </summary>
    public event Action<Window>? OnWindowAdded;

    /// <summary>
    /// 
    /// </summary>
    public event Action<Window>? OnWindowRemoved;

    internal WindowSubmodule()
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

        mWindows.Add(window);
        OnWindowAdded(window);

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
            OnWindowRemoved.Invoke(window);
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
            switch ((SDL_EventType)@event.type)
            {
                case SDL_EventType.SDL_EVENT_WINDOW_CLOSE_REQUESTED:
                {
                    Window? target = mWindows.FirstOrDefault(window => window.ID == @event.window.windowID);
                    if (target != null)
                    {
                        target.Close();
                        DestroyWindow(target);
                    }

                    break;
                }
                default:
                {
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

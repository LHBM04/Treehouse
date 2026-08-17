using System;
using System.Collections.Generic;
using System.Linq;
using System.Numerics;
using SDL3;

namespace Treehouse.Runtime.System.Windowing;

/// <summary>
/// 애플리케이션 내 창을 제어하는 서브시스템을 정의합니다.
/// </summary>
public class WindowSubsystem : EngineSubsystem
{
    /// <summary>
    /// 해당 서브시스템이 관리하는 모든 창.
    /// </summary>
    private readonly List<Window> mWindows;

    /// <summary>
    /// 창이 추가될 때 호출되는 이벤트.
    /// </summary>
    public Action<Window>? OnWindowAdded;

    /// <summary>
    /// 창이 제거될 때 호출되는 이벤트.
    /// </summary>
    public Action<Window>? OnWindowRemoved;

    public WindowSubsystem()
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

        SDL.SetStringProperty(properties, SDL.Props.WindowCreateTitleString, options.Title);

        SDL.SetFloatProperty(properties, SDL.Props.WindowCreateXNumber, options.Position.X);
        SDL.SetFloatProperty(properties, SDL.Props.WindowCreateYNumber, options.Position.Y);
        SDL.SetFloatProperty(properties, SDL.Props.WindowCreateWidthNumber, options.Size.X);
        SDL.SetFloatProperty(properties, SDL.Props.WindowCreateHeightNumber, options.Size.Y);

        SDL.SetBooleanProperty(properties, SDL.Props.WindowCreateFullscreenBoolean, options.Flags.HasFlag(WindowFlags.Fullscreen));
        SDL.SetBooleanProperty(properties, SDL.Props.WindowCreateResizableBoolean, options.Flags.HasFlag(WindowFlags.Resizable));
        SDL.SetBooleanProperty(properties, SDL.Props.WindowCreateBorderlessBoolean, options.Flags.HasFlag(WindowFlags.Borderless));
        SDL.SetBooleanProperty(properties, SDL.Props.WindowCreateHiddenBoolean, options.Flags.HasFlag(WindowFlags.Hidden));
        SDL.SetBooleanProperty(properties, SDL.Props.WindowCreateAlwaysOnTopBoolean, options.Flags.HasFlag(WindowFlags.AlwaysOnTop));
        SDL.SetBooleanProperty(properties, SDL.Props.WindowCreateMaximizedBoolean, options.Flags.HasFlag(WindowFlags.Maximized));
        SDL.SetBooleanProperty(properties, SDL.Props.WindowCreateMaximizedBoolean, options.Flags.HasFlag(WindowFlags.Minimized));
        SDL.SetBooleanProperty(properties, SDL.Props.WindowCreateOpenGLBoolean, true); // TODO: 이거 테스트용이니까, 반드시 나중에 설정으로 분리할 것.

        SDL.SetBooleanProperty(properties, SDL.Props.WindowCreateHighPixelDensityBoolean, false);

        Window window = new Window(SDL.CreateWindowWithProperties(properties));
        window?.OnCreated?.Invoke();

        SDL.DestroyProperties(properties);

        mWindows.Add(window);
        OnWindowAdded?.Invoke(window);

        SDL.GLMakeCurrent(window.Handle, SDL.GLCreateContext(window.Handle));

        return window;
    }

    /// <summary>
    /// 지정한 창을 파괴합니다.
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

        // 테스트용 GL 설정
        SDL.GLSetAttribute(SDL.GLAttr.ContextMajorVersion, 4);
        SDL.GLSetAttribute(SDL.GLAttr.ContextMinorVersion, 3);
        SDL.GLSetAttribute(SDL.GLAttr.ContextProfileMask, (int)SDL.GLProfile.Core);
        SDL.GLSetAttribute(SDL.GLAttr.DepthSize, 24);
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
                    target?.OnMoved?.Invoke(new Vector2(@event.Window.Data1, @event.Window.Data2));
                    break;
                }
                case SDL.EventType.WindowResized:
                {
                    target?.OnResized?.Invoke(new Vector2(@event.Window.Data1, @event.Window.Data2));
                    break;
                }
                case SDL.EventType.WindowCloseRequested:
                {
                    target?.OnClosed?.Invoke();
                    DestroyWindow(target);
                    break;
                }
                case SDL.EventType.WindowFocusGained:
                {
                    target?.OnFocusGained?.Invoke();
                    break;
                }
                case SDL.EventType.WindowFocusLost:
                {
                    target?.OnFocusLost?.Invoke();
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
            window.Dispose();
        }

        mWindows.Clear();

        SDL.QuitSubSystem(SDL.InitFlags.Video | SDL.InitFlags.Events);
    }
}

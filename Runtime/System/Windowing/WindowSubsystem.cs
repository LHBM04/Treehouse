using System;
using System.Collections.Generic;
using System.Linq;
using System.Numerics;
using SDL3;

namespace Treehouse.Runtime.System.Windowing;

/// <summary>
/// 애플리케이션 내 창을 제어하는 서브시스템을 정의합니다.
/// </summary>
public class WindowSubsystem : IEngineSubsystem
{
    public uint Priority
    {
        get { return 0; }
    }

    /// <summary>
    /// 해당 서브시스템이 관리하는 모든 창.
    /// </summary>
    private readonly List<Window> mWindows;

    /// <summary>
    /// 창이 생성될 때 호출되는 이벤트.
    /// </summary>
    public event Action<Window>? OnWindowAdded;

    /// <summary>
    /// 창이 닫힐 때 호출되는 이벤트.
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
    /// 창이 포커싱되었을 때 호출되는 이벤트.
    /// </summary>
    public event Action<Window, bool>? OnWindowFocusChanged;

    public WindowSubsystem()
    {
        mWindows = new List<Window>();
    }

    /// <summary>
    /// 
    /// </summary>
    /// <param name="options"></param>
    /// <returns></returns>
    public void AddWindow(Window window)
    {
        mWindows.Add(window);
        OnWindowAdded?.Invoke(window);
    }

    /// <summary>
    /// 지정한 창을 제거합니다.
    /// </summary>
    /// <param name="window"></param>
    public void RemoveWindow(Window window)
    {
        if (mWindows.Remove(window))
        {
            OnWindowRemoved?.Invoke(window);
            window.Dispose();
        }
    }

    public void OnInitialize()
    {
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

    public void OnTick()
    {
        SDL.Event @event;
        while (SDL.PollEvent(out @event))
        {
            Window? target = mWindows.FirstOrDefault(window => window.ID == @event.Window.WindowID);

            switch ((SDL.EventType)@event.Type)
            {
                case SDL.EventType.WindowMoved:
                {
                    OnWindowMoved?.Invoke(target, new Vector2(@event.Window.Data1, @event.Window.Data2));
                    break;
                }
                case SDL.EventType.WindowResized:
                {
                    OnWindowResized?.Invoke(target, new Vector2(@event.Window.Data1, @event.Window.Data2));
                    break;
                }
                case SDL.EventType.WindowCloseRequested:
                {
                    RemoveWindow(target);
                    break;
                }
                case SDL.EventType.WindowFocusGained:
                {
                    OnWindowFocusChanged?.Invoke(target, true);
                    break;
                }
                case SDL.EventType.WindowFocusLost:
                {
                    OnWindowFocusChanged?.Invoke(target, false);
                    break;
                }
            }
        }
    }

    public void OnRelease()
    {
        foreach (Window window in mWindows)
        {
            window.Dispose();
        }

        mWindows.Clear();

        SDL.QuitSubSystem(SDL.InitFlags.Video | SDL.InitFlags.Events);
    }
}

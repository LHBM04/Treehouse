using SDL3;
using System;
using System.Numerics;
using Vulkan.Xlib;

namespace Treehouse.Runtime.System.Windowing;

public class Window : IDisposable
{
    /// <summary>
    /// 해당 창의 제목.
    /// </summary>
    public string Title
    {
        get { return SDL.GetWindowTitle(Handle); }
        set { SDL.SetWindowTitle(Handle, value); }
    }

    /// <summary>
    /// 해당 창의 위치.
    /// </summary>
    public Vector2 Position
    {
        get
        {
            SDL.GetWindowPosition(Handle, out int x, out int y);
            return new Vector2(x, y);
        }
        set { SDL.SetWindowPosition(Handle, (int)value.X, (int)value.Y); }
    }

    /// <summary>
    /// 해당 창의 크기.
    /// </summary>
    public Vector2 Size
    {
        get
        {
            SDL.GetWindowSize(Handle, out int x, out int y);
            return new Vector2(x, y);
        }
        set { SDL.SetWindowSize(Handle, (int)value.X, (int)value.Y); }
    }

    /// <summary>
    /// 해당 창의 핸들.
    /// </summary>
    internal nint Handle { get; private set; }

    /// <summary>
    /// 해당 창의 고유 ID.
    /// </summary>
    internal uint ID
    {
        get { return SDL.GetWindowID(Handle); }
    }

    public void Dispose()
    {
        if (Handle != nint.Zero)
        {
            SDL.DestroyWindow(Handle);
            Handle = nint.Zero;
        }
    }

    /// <summary>
    /// 
    /// </summary>
    /// <param name="options"></param>
    /// <returns></returns>
    public static Window Create(WindowOptions options)
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

        nint handle = SDL.CreateWindowWithProperties(properties);
        if (handle == nint.Zero)
        {
            throw new Exception($"SDL 창 생성에 실패했습니다! {SDL.GetError()}");
        }

        SDL.DestroyProperties(properties);
        SDL.GLMakeCurrent(handle, SDL.GLCreateContext(handle));

        Window window = new Window();
        window.Handle = handle;

        return window;
    }

    /// <summary>
    /// 
    /// </summary>
    /// <param name="window"></param>
    public static void Destroy(Window window)
    {
        window?.Dispose();
    }
}

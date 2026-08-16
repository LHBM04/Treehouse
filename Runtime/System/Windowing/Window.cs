using System;
using System.Numerics;
using SDL3;

namespace Treehouse.Runtime.System.Windowing;

public class Window : IDisposable
{
    /// <summary>
    /// 해당 창의 제목.
    /// </summary>
    public string Title
    {
        get => SDL.GetWindowTitle(Handle);
        set => SDL.SetWindowTitle(Handle, value);
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
        set => SDL.SetWindowPosition(Handle, (int)value.X, (int)value.Y);
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
        set => SDL.SetWindowSize(Handle, (int)value.X, (int)value.Y);
    }

    /// <summary>
    /// 해당 창의 핸들.
    /// </summary>
    internal nint Handle { get; private set; }

    /// <summary>
    /// 해당 창의 고유 ID.
    /// </summary>
    internal uint ID => SDL.GetWindowID(Handle);

    internal Window(nint handle)
    {
        Handle = handle;
    }

    public void Dispose()
    {
        Handle = nint.Zero;
    }

    public void Show()
    {
        SDL.ShowWindow(Handle);
    }

    public void Close()
    {
        SDL.HideWindow(Handle);
    }
}

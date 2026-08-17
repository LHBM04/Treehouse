using System;
using Treehouse.Runtime.Maths;
using Treehouse.Runtime.Windowing;
using SDL3;

namespace Treehouse.Runtime.Rendering;

/// <summary>
/// 애플리케이션 내 렌더러를 정의합니다.
/// </summary>
public class Renderer : IDisposable
{
    /// <summary>
    /// 해당 렌더러가 그릴 창.
    /// </summary>
    public Window? Window { get; private set; }

    /// <summary>
    /// 해당 렌더러의 클리어 색상.
    /// </summary>
    public ColorRGBA<float> ClearColor { get; set; }

    /// <summary>
    /// 해당 렌더러의 수직 동기화 여부.
    /// </summary>
    public bool ShouldVSync { get; set; }

    /// <summary>
    /// 해당 렌더러가 관리하는 명령 리스트.
    /// </summary>
    internal nint CommandList { get; private set; }

    /// <summary>
    /// 해당 렌더러가 관리하는 스왑 체인.
    /// </summary>
    internal nint Swapchain { get; private set; }

    public void Dispose()
    {
        SDL.ReleaseGPUBuffer();
        SDL.AcquireGPUCommandBuffer()
    }

    public void Begin()
    {
        if (CommandList == null)
        {
            throw new NullReferenceException("해당 렌더러의 명령 리스트가 null입니다!");
        }

        if (Swapchain == null)
        {
            throw new NullReferenceException("해당 렌더러의 스왑 체인이 null입니다!");
        }

        CommandList.Begin();
        CommandList.SetFramebuffer(Swapchain.Framebuffer);
        CommandList.ClearColorTarget(0, new RgbaFloat(ClearColor.R, ClearColor.G, ClearColor.B, ClearColor.A));
    }

    public void End()
    {
        if (CommandList == null)
        {
            throw new NullReferenceException("해당 렌더러의 명령 리스트가 null입니다!");
        }

        CommandList.End();
    }

    internal static Renderer Create(GraphicsDevice device, RendererOptions options)
    {
        Renderer renderer = new Renderer();
        renderer.Window = options.Window;
        renderer.ClearColor = options.ClearColor;
        renderer.ShouldVSync = options.ShouldVSync;
        renderer.CommandList = device.ResourceFactory.CreateCommandList();
        renderer.Swapchain = device.MainSwapchain;

        renderer.Swapchain.Resize((uint)options.Size.X, (uint)options.Size.Y);

        return renderer;
    }

    internal static void Destroy(Renderer renderer)
    {
        renderer.Dispose();
    }
}

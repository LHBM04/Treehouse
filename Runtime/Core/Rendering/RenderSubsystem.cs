using SDL3;
using System;
using System.Collections.Generic;
using System.Linq;
using Treehouse.Runtime.Maths;
using Treehouse.Runtime.Core.Windowing;
using Veldrid;
using Veldrid.OpenGL;
using Vulkan.Xlib;
using Window = Treehouse.Runtime.Core.Windowing.Window;
using Windowing_Window = Treehouse.Runtime.Core.Windowing.Window;

namespace Treehouse.Runtime.Core.Rendering;

public class RenderSubsystem : IEngineSubsystem
{
    public uint Priority
    {
        get { return 10; }
    }

    /// <summary>
    /// 해당 서브시스템이 관리하는 그래픽스 디바이스.
    /// </summary>
    private GraphicsDevice? mDevice;

    /// <summary>
    /// 해당 서브시스템이 관리하는 모든 렌더러.
    /// </summary>
    private readonly List<Renderer> mRenderers;

    public RenderSubsystem()
    {
        mRenderers = new List<Renderer>();
    }

    public void OnInitialize()
    {
        // GL의 경우 해줄 게 없다.
    }

    public void OnTick()
    {
        if (mDevice == null)
        {
            throw new NullReferenceException("디바이스가 null입니다!");
        }

        if (!mRenderers.Any())
        {
            return;
        }

        foreach (Renderer renderer in mRenderers)
        {
            renderer.Begin();

            // 렌더링 로직...

            renderer.End();

            mDevice.SubmitCommands(renderer.CommandList);
            mDevice.SwapBuffers(renderer.Swapchain);
        }
    }

    public void OnRelease()
    {
        if (mRenderers.Any())
        {
            foreach (Renderer renderer in mRenderers)
            {
                renderer.Dispose();
            }

            mRenderers.Clear();
        }

        mDevice?.Dispose();
    }

    public void AddRenderer(Windowing_Window window)
    {
        GraphicsDeviceOptions graphicsDeviceOptions = new GraphicsDeviceOptions
        {
            PreferStandardClipSpaceYDirection = true,
            PreferDepthRangeZeroToOne = true,
            SwapchainDepthFormat = PixelFormat.R32_Float
        };

        OpenGLPlatformInfo platformInfo = new OpenGLPlatformInfo(
            SDL.GLGetCurrentContext(),
            SDL.GLGetProcAddress,
            (ctx) => SDL.GLMakeCurrent(window.Handle, ctx),
            () => SDL.GLGetCurrentContext(),
            () => SDL.GLMakeCurrent(nint.Zero, nint.Zero),
            (ctx) => SDL.GLDestroyContext(ctx),
            () => SDL.GLSwapWindow(window.Handle),
            (vSync) => SDL.GLSetSwapInterval(vSync ? 1 : 0)
        );

        mDevice = GraphicsDevice.CreateOpenGL(
            graphicsDeviceOptions,
            platformInfo,
            (uint)window.Size.X,
            (uint)window.Size.Y
        );

        RendererOptions rendererOptions = new RendererOptions
        {
            Window = window,
            ClearColor = new ColorRGBA<float>(1.0f, 1.0f, 1.0f, 1.0f),
            Position = new Vector2D<float>(0.0f, 0.0f),
            Size = new Vector2D<float>(window.Size.X, window.Size.Y),
            ShouldVSync = false,
        };

        Renderer renderer = Renderer.Create(mDevice, rendererOptions);
        mRenderers.Add(renderer);
    }

    public void DestroyOpenGL(Windowing_Window window)
    {
        mRenderers.RemoveAll((Renderer renderer) => { return renderer.Window == window; });
    }
}

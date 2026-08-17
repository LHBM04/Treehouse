using SDL3;
using System;
using Treehouse.Runtime.System.Windowing;
using Veldrid;
using Veldrid.OpenGL;

namespace Treehouse.Runtime.System.Rendering;

public class RenderSubsystem : EngineSubsystem
{
    /// <summary>
    /// 해당 서브시스템이 관리하는 그래픽스 디바이스.
    /// </summary>
    private GraphicsDevice? mDevice;

    internal override void OnInitialize()
    {
        base.OnInitialize();
    }

    internal override void OnRelease()
    {
        base.OnRelease();
    }

    public void CreateOpenGL(Window window)
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
    }
}

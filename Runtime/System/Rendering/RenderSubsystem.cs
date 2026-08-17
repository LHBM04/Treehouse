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

    /// <summary>
    /// 해당 서브시스템이 관리하는 명령 리스트.
    /// </summary>
    private CommandList? mCommandList;

    internal override void OnInitialize()
    {
        base.OnInitialize();
    }

    internal override void OnTick()
    {
        base.OnTick();

        if (mCommandList == null)
        {
            throw new NullReferenceException("명령 리스트가 null입니다!");
        }

        mCommandList.Begin();
        mCommandList.SetFramebuffer(mDevice.MainSwapchain.Framebuffer);
        mCommandList.ClearColorTarget(0, new RgbaFloat(1, 1, 1, 1));
        mCommandList.End();

        mDevice.SubmitCommands(mCommandList);
        mDevice.SwapBuffers(mDevice.MainSwapchain);
    }

    internal override void OnRelease()
    {
        mCommandList.Dispose();
        mDevice.Dispose();

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

        mCommandList = mDevice.ResourceFactory.CreateCommandList();
    }
}

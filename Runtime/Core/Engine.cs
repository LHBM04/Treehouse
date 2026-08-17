using Treehouse.Runtime.Core.Rendering;
using Treehouse.Runtime.Core.Windowing;

namespace Treehouse.Runtime.Core;

/// <summary>
/// 애플리케이션 내 엔진을 구현합니다.
/// </summary>
public class Engine : System<IEngineSubsystem>
{
    public void Tick()
    {
        GetSubsystem<WindowSubsystem>()?.OnTick();
        GetSubsystem<RenderSubsystem>()?.OnTick();
    }
}

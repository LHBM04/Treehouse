using System.Linq;

namespace Treehouse.Runtime.Core;

/// <summary>
/// 애플리케이션 내 엔진을 구현합니다.
/// </summary>
public class Engine : System<IEngineSubsystem>
{
    public void Initialize()
    {
        Subsystems.OrderBy((IEngineSubsystem subsystem) => { return subsystem.Priority; });

        foreach (IEngineSubsystem subsystem in Subsystems)
        {
            subsystem.OnInitialize();
        }
    }

    public void Tick()
    {
        foreach (IEngineSubsystem subsystem in Subsystems)
        {
            subsystem.OnTick();
        }
    }

    public void Release()
    {
        Subsystems.OrderByDescending((IEngineSubsystem subsystem) => { return subsystem.Priority; });

        foreach (IEngineSubsystem subsystem in Subsystems)
        {
            subsystem.OnRelease();
        }

        Subsystems.Clear();
    }
}

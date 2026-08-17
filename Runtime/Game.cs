using System.Linq;

namespace Treehouse.Runtime;

public class Game : System<IGameSubsystem>
{
    /// <summary>
    /// 해당 게임의 실행 여부.
    /// </summary>
    public bool IsRunning { get; set; }

    public void Initialize()
    {
        Subsystems.OrderBy((subsystem) => { return subsystem.Priority; });

        foreach (IGameSubsystem subsystem in Subsystems)
        {
            subsystem.OnInitialize();
        }
    }

    public void Tick()
    {
        foreach (IGameSubsystem subsystem in Subsystems)
        {
            subsystem.OnTick();
        }
    }

    public void Release()
    {
        Subsystems.OrderByDescending((subsystem) => { return subsystem.Priority; });

        foreach (IGameSubsystem subsystem in Subsystems)
        {
            subsystem.OnRelease();
        }

        Subsystems.Clear();
    }
}

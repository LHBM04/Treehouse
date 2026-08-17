using System;
using System.Collections.Generic;
using System.Linq;

namespace Treehouse.Runtime.System;

/// <summary>
/// 애플리케이션 내 엔진을 구현합니다.
/// </summary>
public class Engine : IDisposable
{
    /// <summary>
    /// 해당 엔진 내의 모든 서브시스템.
    /// </summary>
    private readonly List<EngineSubsystem> mSubsystems;

    public Engine()
    {
        mSubsystems = new List<EngineSubsystem>();
    }

    public void Dispose()
    {
        if (mSubsystems.Any())
        {
            foreach (var subsystem in mSubsystems)
            {
                subsystem.OnRelease();
            }

            mSubsystems.Clear();
        }
    }

    /// <summary>
    /// 매 프레임마다 호출됩니다.
    /// </summary>
    public void Tick()
    {
        if (mSubsystems.Any())
        {
            foreach (var subsystem in mSubsystems)
            {
                subsystem.OnTick();
            }
        }
    }

    /// <summary>
    /// 지정한 타입의 서브시스템을 엔진에 추가합니다.
    /// </summary>
    /// <param name="submoduleType">지정할 타입.</param>
    public void AddSubsystem(Type submoduleType)
    {
        EngineSubsystem? newSubmodule = Activator.CreateInstance(submoduleType) as EngineSubsystem;
        if (newSubmodule == null)
        {
            throw new ArgumentException("올바른 서브시스템 타입이 아닙니다!");
        }

        newSubmodule!.OnInitialize();
        mSubsystems.Add(newSubmodule!);
    }

    /// <summary>
    /// 지정한 타입의 서브시스템을 엔진에 추가합니다.
    /// </summary>
    /// <typeparam name="TSubmodule">지정할 타입.</typeparam>
    public void AddSubsystem<TSubmodule>() where TSubmodule : EngineSubsystem
    {
        AddSubsystem(typeof(TSubmodule));
    }

    /// <summary>
    /// 해당 엔진에 지정한 타입을 가진 서브시스템을 반환합니다.
    /// </summary>
    /// <param name="subsystemType">지정할 타입.</param>
    /// <returns>지정한 타입을 가진 서브시스템. 없으면 null.</returns>
    public EngineSubsystem? GetSubsystem(Type subsystemType)
    {
        return mSubsystems.FirstOrDefault(submodule => submodule.GetType() == subsystemType);
    }

    /// <summary>
    /// 해당 엔진에 지정한 타입을 가진 서브시스템을 반환합니다.
    /// </summary>
    /// <typeparam name="TSubsystem">지정할 타입.</typeparam>
    /// <returns>지정한 타입을 가진 서브시스템. 없으면 null.</returns>
    public TSubsystem? GetSubsystem<TSubsystem>() where TSubsystem : EngineSubsystem
    {
        return mSubsystems.OfType<TSubsystem>().FirstOrDefault();
    }
}

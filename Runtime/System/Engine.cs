using System;
using System.Collections.Generic;
using System.Linq;

namespace Treehouse.Runtime.System;

/// <summary>
/// 애플리케이션 내 엔진을 구현합니다.
/// </summary>
public class Engine : ISystem
{
    /// <summary>
    /// 해당 엔진 내의 모든 서브시스템.
    /// </summary>
    private readonly List<IEngineSubsystem> mSubsystems;

    public Engine()
    {
        mSubsystems = new List<IEngineSubsystem>();
    }

    public void Dispose()
    {
        if (mSubsystems.Any())
        {
            mSubsystems.OrderByDescending((IEngineSubsystem subsystem) => { return subsystem.Priority; });

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
            mSubsystems.OrderBy(subsystem => subsystem.Priority);

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
        IEngineSubsystem? newSubmodule = Activator.CreateInstance(submoduleType) as IEngineSubsystem;
        if (newSubmodule == null)
        {
            throw new ArgumentException("올바른 서브시스템 타입이 아닙니다!");
        }

        newSubmodule?.OnInitialize();
        mSubsystems?.Add(newSubmodule!);
    }

    /// <summary>
    /// 지정한 타입의 서브시스템을 엔진에 추가합니다.
    /// </summary>
    /// <typeparam name="TSubmodule">지정할 타입.</typeparam>
    public void AddSubsystem<TSubmodule>() where TSubmodule : IEngineSubsystem
    {
        AddSubsystem(typeof(TSubmodule));
    }

    /// <summary>
    /// 해당 엔진에 지정한 타입을 가진 서브시스템을 반환합니다.
    /// </summary>
    /// <param name="subsystemType">지정할 타입.</param>
    /// <returns>지정한 타입을 가진 서브시스템. 없으면 null.</returns>
    public IEngineSubsystem? GetSubsystem(Type subsystemType)
    {
        return mSubsystems.FirstOrDefault(submodule => submodule.GetType() == subsystemType);
    }

    /// <summary>
    /// 해당 엔진에 지정한 타입을 가진 서브시스템을 반환합니다.
    /// </summary>
    /// <typeparam name="TSubsystem">지정할 타입.</typeparam>
    /// <returns>지정한 타입을 가진 서브시스템. 없으면 null.</returns>
    public TSubsystem? GetSubsystem<TSubsystem>() where TSubsystem : IEngineSubsystem
    {
        return mSubsystems.OfType<TSubsystem>().FirstOrDefault();
    }
}

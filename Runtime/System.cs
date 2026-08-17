using System;
using System.Collections.Generic;
using System.Linq;

namespace Treehouse.Runtime;

/// <summary>
/// 애플리케이션 내 시스템을 정의합니다.
/// </summary>
public abstract class System<TSubsystem> : IDisposable
{
    /// <summary>
    /// 해당 시스템이 관리하는 모든 서브시스템.
    /// </summary>
    public List<TSubsystem> Subsystems { get; private set; }
        = new List<TSubsystem>();

    public void Dispose()
    {
        Subsystems.Clear();
    }

    /// <summary>
    /// 지정한 타입의 서브시스템을 엔진에 추가합니다.
    /// </summary>
    /// <param name="submoduleType">지정할 타입.</param>
    public void AddSubsystem(Type submoduleType)
    {
        TSubsystem? newSubmodule = (TSubsystem)Activator.CreateInstance(submoduleType);
        if (newSubmodule == null)
        {
            throw new ArgumentException("올바른 서브시스템 타입이 아닙니다!");
        }

        Subsystems.Add(newSubmodule);
    }

    /// <summary>
    /// 지정한 타입의 서브시스템을 엔진에 추가합니다.
    /// </summary>
    /// <typeparam name="TSubsystem">지정할 타입.</typeparam>
    public void AddSubsystem<TSubsystem>()
    {
        AddSubsystem(typeof(TSubsystem));
    }

    /// <summary>
    /// 해당 엔진에 지정한 타입을 가진 서브시스템을 반환합니다.
    /// </summary>
    /// <param name="subsystemType">지정할 타입.</param>
    /// <returns>지정한 타입을 가진 서브시스템. 없으면 null.</returns>
    public TSubsystem? GetSubsystem(Type subsystemType)
    {
        return Subsystems.FirstOrDefault((subsystem) => { return subsystem.GetType() == subsystemType;});
    }

    /// <summary>
    /// 해당 엔진에 지정한 타입을 가진 서브시스템을 반환합니다.
    /// </summary>
    /// <typeparam name="TSubsystem">지정할 타입.</typeparam>
    /// <returns>지정한 타입을 가진 서브시스템. 없으면 null.</returns>
    public TSubsystem2? GetSubsystem<TSubsystem2>() where TSubsystem2 : TSubsystem
    {
        return (TSubsystem2)GetSubsystem(typeof(TSubsystem2));
    }
}

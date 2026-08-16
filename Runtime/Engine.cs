using System;
using System.Collections.Generic;
using System.Linq;

namespace Treehouse.Runtime;

/// <summary>
/// 애플리케이션 내 엔진을 구현합니다.
/// </summary>
public class Engine : IDisposable
{
    /// <summary>
    /// 해당 엔진 내의 모든 서브모듈.
    /// </summary>
    private readonly List<EngineSubmodule> mSubmodules;

    internal Engine()
    {
        mSubmodules = new List<EngineSubmodule>();
    }

    public void Dispose()
    {
        if (mSubmodules.Any())
        {
            foreach (var submodule in mSubmodules)
            {
                submodule.OnRelease();
            }

            mSubmodules.Clear();
        }
    }

    /// <summary>
    /// 매 프레임마다 호출됩니다.
    /// </summary>
    internal void Tick()
    {
        if (mSubmodules.Any())
        {
            foreach (var submodule in mSubmodules)
            {
                submodule.OnTick();
            }
        }
    }

    /// <summary>
    /// 지정한 타입의 서브모듈을 엔진에 추가합니다.
    /// </summary>
    /// <param name="submoduleType">지정할 타입.</param>
    public void AddSubmodule(Type submoduleType)
    {
        EngineSubmodule? newSubmodule = Activator.CreateInstance(submoduleType) as EngineSubmodule;
        if (newSubmodule != null)
        {
            throw new ArgumentException("올바른 Submodule 타입이 아닙니다.");
        }

        newSubmodule!.OnInitialize();
        mSubmodules.Add(newSubmodule!);
    }

    /// <summary>
    /// 지정한 타입의 서브모듈을 엔진에 추가합니다.
    /// </summary>
    /// <typeparam name="TSubmodule">지정할 타입.</typeparam>
    public void AddSubmodule<TSubmodule>() where TSubmodule : EngineSubmodule
    {
        AddSubmodule(typeof(TSubmodule));
    }

    /// <summary>
    /// 해당 엔진에 지정한 타입을 가진 서브모듈을 반환합니다.
    /// </summary>
    /// <param name="submoduleType">지정할 타입.</param>
    /// <returns>지정한 타입을 가진 서브모듈. 없으면 null.</returns>
    public EngineSubmodule? GetSubmodule(Type submoduleType)
    {
        return mSubmodules.FirstOrDefault(submodule => submodule.GetType() == submoduleType);
    }

    /// <summary>
    /// 해당 엔진에 지정한 타입을 가진 서브모듈을 반환합니다.
    /// </summary>
    /// <typeparam name="TSubmodule">지정할 타입.</typeparam>
    /// <returns>지정한 타입을 가진 서브모듈. 없으면 null.</returns>
    public TSubmodule? GetSubmodule<TSubmodule>() where TSubmodule : EngineSubmodule
    {
        return mSubmodules.OfType<TSubmodule>().FirstOrDefault();
    }
}

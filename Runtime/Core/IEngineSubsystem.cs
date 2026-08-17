namespace Treehouse.Runtime.Core;

/// <summary>
/// 애플리케이션 내 엔진 시스템이 관리하는 서브시스템을 정의합니다.
/// </summary>
public interface IEngineSubsystem : ISubsystem
{
    /// <summary>
    /// 해당 서브시스템이 초기화될 때 호출됩니다.
    /// </summary>
    void OnInitialize();

    /// <summary>
    /// 매 프레임마다 호출됩니다.
    /// </summary>
    void OnTick();

    /// <summary>
    /// 해당 서브시스템이 해제될 때 호출됩니다,
    /// </summary>
    void OnRelease();
}

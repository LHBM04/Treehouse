namespace Treehouse.Runtime.System;

/// <summary>
/// 애플리케이션 내 서브시스템을 정의합니다.
/// </summary>
public interface ISubsystem
{
    /// <summary>
    /// 해당 서브시스템의 우선도.
    /// </summary>
    public uint Priority { get; }
}

namespace Treehouse.Runtime.Core.Windowing;

/// <summary>
/// 애플리케이션 내 창이 가지는 플래그를 정의합니다.
/// </summary>
public enum WindowFlags : ushort
{
    /// <summary>
    /// 일반.
    /// </summary>
    None = 0,

    /// <summary>
    /// 전체 화면.
    /// </summary>
    Fullscreen = 1 << 0,

    /// <summary>
    /// 크기 조절 가능.
    /// </summary>
    Resizable = 1 << 1,

    /// <summary>
    /// 테두리 없음.
    /// </summary>
    Borderless = 1 << 2,

    /// <summary>
    /// 항상 최상위.
    /// </summary>
    AlwaysOnTop = 1 << 3,

    /// <summary>
    /// 숨겨짐.
    /// </summary>
    Hidden = 1 << 4,

    /// <summary>
    /// 최소화됨.
    /// </summary>
    Minimized = 1 << 5,

    /// <summary>
    /// 최대화됨.
    /// </summary>
    Maximized = 1 << 6,
}

using Treehouse.Runtime.Maths;

namespace Treehouse.Runtime.System.Windowing;

/// <summary>
/// 애플리케이션 내 창이 가지는 설정을 정의합니다.
/// </summary>
public record class WindowOptions
{
    /// <summary>
    /// 생성할 창의 제목.
    /// </summary>
    public string Title { get; set; } = "Treehouse Window";

    /// <summary>
    /// 생성할 창의 위치.
    /// </summary>
    public Vector2D<int> Position { get; set; } = new Vector2D<int>(100, 100);

    /// <summary>
    /// 생성할 창의 크기.
    /// </summary>
    public Vector2D<int> Size { get; set; } = new Vector2D<int>(800, 600);

    /// <summary>
    /// 생성할 창의 플래그.
    /// </summary>
    public WindowFlags Flags { get; set; } = WindowFlags.Resizable;
}

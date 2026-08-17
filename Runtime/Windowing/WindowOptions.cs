using Treehouse.Runtime.Maths;

namespace Treehouse.Runtime.Windowing;

/// <summary>
/// 애플리케이션 내 창이 가지는 설정을 정의합니다.
/// </summary>
public record class WindowOptions
{
    /// <summary>
    /// 생성할 창의 제목.
    /// </summary>
    public required string Title { get; set; }

    /// <summary>
    /// 생성할 창의 X 위치.
    /// </summary>
    public required int PositionX { get; set; }

    /// <summary>
    /// 생성할 창의 Y 위치.
    /// </summary>
    public required int PositionY { get; set; }

    /// <summary>
    /// 생성할 창의 가로 크기.
    /// </summary>
    public required int SizeX { get; set; }

    /// <summary>
    /// 생성할 창의 세로 크기.
    /// </summary>
    public required int SizeY { get; set; }

    /// <summary>
    /// 생성할 창의 플래그.
    /// </summary>
    public required WindowFlags Flags { get; set; }
}

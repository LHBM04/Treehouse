using System.Numerics;

namespace Treehouse.Runtime.System;

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
    public Vector2 Position { get; set; } = Vector2.Zero;

    /// <summary>
    /// 생성할 창의 크기.
    /// </summary>
    public Vector2 Size { get; set; } = new Vector2(800, 600);

    /// <summary>
    /// 생성할 창의 플래그.
    /// </summary>
    public WindowFlags Flags { get; set; } = WindowFlags.None;
}

using System;
using Treehouse.Runtime.Maths;

namespace Treehouse.Runtime.Core;

/// <summary>
/// 게임 화면이 가지는 플래그를 정의합니다.
/// </summary>
public enum ScreenFlags : byte
{
    /// <summary>
    /// 전체 화면.
    /// </summary>
    Fullscreen,

    /// <summary>
    /// 테두리 없는 창 모드.
    /// </summary>
    Borderless,

    /// <summary>
    /// 테두리 있는 창 모드.
    /// </summary>
    Windowed
}

/// <summary>
/// 게임의 옵션을 정의합니다.
/// </summary>
public record class GameOptions
{
    /// <summary>
    /// 게임의 제목.
    /// </summary>
    public required string Title { get; set; }

    /// <summary>
    /// 게임의 해상도.
    /// </summary>
    public required Vector2D<int> Resolution { get; set; }

    /// <summary>
    /// 해당 게임의 화면 플래그.
    /// </summary>
    public required ScreenFlags Flags { get; set; }
}

using System;
using Treehouse.Runtime.Maths;

namespace Treehouse.Runtime;

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
/// 프로젝트의 옵션을 정의합니다.
/// </summary>
public record class ProjectSettings
{
    /// <summary>
    /// 게임의 제목.
    /// </summary>
    public required string ProductName { get; set; }

    /// <summary>
    /// 
    /// </summary>
    public required string CompanyName { get; set; }

    /// <summary>
    /// 
    /// </summary>
    public required string Version { get; set; }
}

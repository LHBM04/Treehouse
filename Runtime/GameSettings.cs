using System.Text.Json.Serialization;
using Treehouse.Runtime.Maths;

namespace Treehouse.Runtime;

/// <summary>
/// 게임 내 설정을 정의합니다.
/// </summary>
public record class GameSettings
{
    /// <summary>
    /// 게임 제목.
    /// </summary>
    [JsonPropertyName("DisplayedTitle")]
    public required string DisplayedTitle { get; set; }

    /// <summary>
    /// 게임 스크린 가로 크기.
    /// </summary>
    [JsonPropertyName("ScreenSizeX")]
    public required int ScreenSizeX { get; set; }

    /// <summary>
    /// 게임 스크린 세로 크기.
    /// </summary>
    [JsonPropertyName("ScreenSizeY")]
    public required int ScreenSizeY { get; set; }

    /// <summary>
    /// 게임 스크린 플래그.
    /// </summary>
    [JsonPropertyName("ScreenFlags")]
    public required ScreenFlags ScreenFlags { get; set; }

    /// <summary>
    /// 게임 가로 해상도.
    /// </summary>
    [JsonPropertyName("ResolutionSizeX")]
    public required float ResolutionSizeX { get; set; }

    /// <summary>
    /// 게임 세로 해상도.
    /// </summary>
    [JsonPropertyName("ResolutionSizeY")]
    public required float ResolutionSizeY { get; set; }

    /// <summary>
    /// 게임 프레임 레이트 제한 값.
    /// </summary>
    [JsonPropertyName("FrameLateLimit")]
    public required float FrameLateLimit { get; set; }

    /// <summary>
    /// 게임 내 수직 동기화 적용 여부.
    /// </summary>
    [JsonPropertyName("ShouldVSync")]
    public bool ShouldVSync { get; set; }
}

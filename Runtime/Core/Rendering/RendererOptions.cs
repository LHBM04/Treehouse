using Treehouse.Runtime.Core.Windowing;
using Treehouse.Runtime.Maths;

namespace Treehouse.Runtime.Core.Rendering;

/// <summary>
/// 생성할 렌더러의 설정을 정의합니다.
/// </summary>
public record class RendererOptions
{
    /// <summary>
    /// 생성할 렌더러가 그릴 창.
    /// </summary>
    public required Window Window { get; init; }

    /// <summary>
    /// 생성할 렌더러의 백버퍼 위치.
    /// </summary>
    public required Vector2D<float> Position { get; init; } = new Vector2D<float>(0.0f, 0.0f);

    /// <summary>
    /// 생성할 렌더러의 백버퍼 크기.
    /// </summary>
    public required Vector2D<float> Size { get; init; } = new Vector2D<float>(800, 600);

    /// <summary>
    /// 생성할 렌더러의 클리어 색상.
    /// </summary>
    public required ColorRGBA<float> ClearColor { get; init; } = new ColorRGBA<float>(1.0f, 1.0f, 1.0f, 1.0f);

    /// <summary>
    /// 생성할 렌더러의 수직 동기화 여부.
    /// </summary>
    public required bool ShouldVSync { get; init; } = true;
}

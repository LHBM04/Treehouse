using System.Numerics;

namespace Treehouse.Runtime.Maths;

/// <summary>
/// R, G, B, A로 구성된 색상을 정의합니다.
/// </summary>
/// <typeparam name="TValue"></typeparam>
public struct ColorRGBA<TValue> where TValue : INumber<TValue>
{
    /// <summary>
    /// 해당 색상의 빨강.
    /// </summary>
    public TValue R { get; set; }

    /// <summary>
    /// 해당 색상의 초록.
    /// </summary>
    public TValue G { get; set; }

    /// <summary>
    /// 해당 색상의 파랑.
    /// </summary>
    public TValue B { get; set; }

    /// <summary>
    /// 해당 색상의 투명도.
    /// </summary>
    public TValue A { get; set; }
}

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

    /// <summary>
    /// 생성자.
    /// </summary>
    public ColorRGBA()
    {
        R = default(TValue);
        G = default(TValue);
        B = default(TValue);
        A = default(TValue);
    }

    /// <summary>
    /// 생성자.
    /// </summary>
    /// <param name="value">생성할 색상의 공통 값.</param>
    public ColorRGBA(TValue value)
    {
        R = value;
        G = value;
        B = value;
        A = value;
    }

    /// <summary>
    /// 생성자.
    /// </summary>
    /// <param name="r">생성할 색상의 빨강 값.</param>
    /// <param name="g">생성할 색상의 초록 값.</param>
    /// <param name="b">생성할 색상의 파랑 값.</param>
    public ColorRGBA(TValue r, TValue g, TValue b)
    {
        R = r;
        G = g;
        B = b;
        A = TValue.One;
    }

    /// <summary>
    /// 생성자.
    /// </summary>
    /// <param name="r">생성할 색상의 빨강 값.</param>
    /// <param name="g">생성할 색상의 초록 값.</param>
    /// <param name="b">생성할 색상의 파랑 값.</param>
    /// <param name="a">생성할 색상의 투명도.</param>
    public ColorRGBA(TValue r, TValue g, TValue b, TValue a)
    {
        R = r;
        G = g;
        B = b;
        A = a;
    }
}

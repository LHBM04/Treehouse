using System.Numerics;

namespace Treehouse.Runtime.Maths;

/// <summary>
/// 2차원 벡터를 정의합니다.
/// </summary>
/// <typeparam name="TValue">2차원 벡터의 원소 타입.</typeparam>
public struct Vector2D<TValue> where TValue : INumber<TValue>
{
    /// <summary>
    /// 해당 2차원 벡터의 X 좌표.
    /// </summary>
    public TValue X { get; set; }

    /// <summary>
    /// 해당 2차원 벡터의 Y 좌표.
    /// </summary>
    public TValue Y { get; set; }

    /// <summary>
    /// 생성자.
    /// </summary>
    public Vector2D()
    {
        X = default(TValue);
        Y = default(TValue);
    }

    /// <summary>
    /// 생성자.
    /// </summary>
    /// <param name="value">생성할 2차원 벡터의 공통 값.</param>
    public Vector2D(TValue value)
    {
        X = value;
        Y = value;
    }

    /// <summary>
    /// 생성자.
    /// </summary>
    /// <param name="x">생성할 2차원 벡터의 X 값.</param>
    /// <param name="y">생성할 2차원 벡터의 Y 값.</param>
    public Vector2D(TValue x, TValue y)
    {
        X = x;
        Y = y;
    }
}


namespace UnityExtension
{
    /// <summary>
    /// 轴
    /// 因为对众多不同用途、不同名称的定义感到厌烦，所以就定义了一个万能版本
    /// 提示：可以为 Axis 字段添加 AxisUsageAttribute 以实现不同用途
    /// </summary>
    [System.Flags]
    public enum Axis
    {
        None = 0,

        PositiveX = 1,      // 000 001
        PositiveY = 2,      // 000 010
        PositiveZ = 4,      // 000 100
        NegativeX = 8,      // 001 000
        NegativeY = 16,     // 010 000
        NegativeZ = 32,     // 100 000

        X = 9,
        Y = 18,
        Z = 36,

        XY = 27,
        YZ = 54,
        XZ = 45,

        All = 63
    }


    /// <summary>
    /// 轴之间关系
    /// </summary>
    public enum AxisRelation
    {
        Same,
        Vertical,
        Opposite,
    }

} // namespace UnityExtension
/// <summary>
/// UI数据接口
/// 所有需要传递给UI的数据类都应该实现这个接口
/// </summary>
public interface IUIData
{
    /// <summary>
    /// 验证数据是否有效
    /// </summary>
    bool IsValid();
}

public class NoneUIData : IUIData
{
    public static readonly NoneUIData noneUIData = new NoneUIData();
    
    static NoneUIData()
    {
    }

    public bool IsValid()
    {
        return true;
    }
}
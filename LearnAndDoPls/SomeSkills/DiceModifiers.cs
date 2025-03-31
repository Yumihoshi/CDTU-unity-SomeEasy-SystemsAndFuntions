using UnityEngine;


// 骰子修改器接口 - 用于实现各种buff
public interface IDiceModifier
{
    int ModifyDiceValue(int originalValue);
    string GetDescription();
}

public class MinValueIncreaseBuff : IDiceModifier
{
    private int _increase;

    public MinValueIncreaseBuff(int increase)
    {
        _increase = increase;
    }

    public int ModifyDiceValue(int originalValue)
    {
        //返回两个值中较大的一个
        return Mathf.Max(originalValue, _increase);
    }

    public string GetDescription()=> $"最小值提升至{_increase}";
}

public class ValueAdditionBuff : IDiceModifier
{
    private int _addAmount;

    public ValueAdditionBuff(int addAmount)
    {
        _addAmount = addAmount;
    }

    public int ModifyDiceValue(int originalValue)
    {
        return originalValue + _addAmount;
    }

    public string GetDescription()=> $"点数增加{_addAmount}";
    
}
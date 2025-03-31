using System;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(SpriteRenderer), typeof(AudioSource))]
public class Dice : MonoBehaviour
{
    private int _level; // 骰子等级
    private List<IDiceModifier> _modifiers = new List<IDiceModifier>(); // buff列表

    private int _value; // 骰子点数

    public int GetValue() => _value; // 获取骰子点数

    #region 通知UI事件
    public event EventHandler<OnDiceRolledEventArgs> OnDiceRolled; // 掷骰子事件

    public class OnDiceRolledEventArgs : EventArgs
    {
        public int value { get; set; } // 骰子点数
        public OnDiceRolledEventArgs(int _value)
        {
            this.value = _value;
        }
    }
    #endregion
    
    public Dice(int level = 0)
    {
        _level = level;
    }

    private void Start()
    {
        GameWorldManager.Instance.OnGameStart += GameWorldManager_OnGameStart;
    }

    private void GameWorldManager_OnGameStart(object sender, EventArgs e)
    {
        RollDice();
    }


    // 基础骰子生成策略
    private interface IDiceValueGenerator
    {
        int GenerateValue();
    }

    // 不同等级骰子的生成策略
    private class LevelDiceValueGenerator : IDiceValueGenerator
    {
        private int _minValue;
        private int _maxValue;

        public LevelDiceValueGenerator(int level)
        {
            // 根据等级设置不同的范围
            switch (level)
            {
                case 0:
                    _minValue = 1;
                    _maxValue = 4;
                    break;
                case 1:
                    _minValue = 5;
                    _maxValue = 10;
                    break;
                case 2:
                    _minValue = 10;
                    _maxValue = 16;
                    break;
                default:
                    _minValue = 16;
                    _maxValue = 20;
                    break;
            }
        }

        public int GenerateValue()
        {
            // 注意：Random.Range对整数是[min, max)，所以max要+1
            return UnityEngine.Random.Range(_minValue, _maxValue + 1);
        }
    }


    /// <summary>
    /// 添加骰子修改器(buff)
    /// </summary>
    /// <param name="modifier"></param>
    public void AddModifier(IDiceModifier modifier)
    {
        if (modifier != null && !_modifiers.Contains(modifier))
        {
            _modifiers.Add(modifier);
            Debug.Log($"添加了骰子Buff: {modifier.GetDescription()}");
        }
    }

    /// <summary>
    /// 移除骰子修改器(buff)
    /// </summary>
    /// <param name="modifier"></param>
    public void RemoveModifier(IDiceModifier modifier)
    {
        if (_modifiers.Contains(modifier))
        {
            _modifiers.Remove(modifier);
            Debug.Log($"移除了骰子Buff: {modifier.GetDescription()}");
        }
    }

    /// <summary>
    /// 清除所有修改器
    /// </summary>
    public void ClearModifiers()
    {
        _modifiers.Clear();
        Debug.Log("清除了所有骰子Buff");
    }

    public void RollDice()
    {
        // 使用策略模式生成初始骰子值
        IDiceValueGenerator generator = new LevelDiceValueGenerator(_level);
        int originalValue = generator.GenerateValue();

        // 应用所有修改器(buff)
        int modifiedValue = originalValue;
        foreach (var modifier in _modifiers)
        {
            modifiedValue = modifier.ModifyDiceValue(modifiedValue);
        }

        _value = modifiedValue;

        // 触发事件
        OnDiceRolled?.Invoke(this, new OnDiceRolledEventArgs(_value));

        Debug.Log($"骰子原始值: {originalValue}, 修改后值: {_value}");
    }
}





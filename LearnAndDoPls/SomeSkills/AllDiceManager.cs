using System.Collections.Generic;
using UnityEngine;
using CDTU.Utils;

public class AllDiceManager : Singleton<AllDiceManager>
{
    private List<Dice> allDices = new List<Dice>();

    private int NowLevel = 0; // 当前骰子等级

    public int GetNowLevel()=> NowLevel; // 获取当前骰子等级

    public void SetNowLevel(int level) => NowLevel = level; // 设置当前骰子等级    

    public Dice CreateDice(Vector2 location)
    {
        GameObject diceObj = new GameObject("Dice");
        diceObj.transform.position = location;
        
        // 添加必要组件
        Dice dice = diceObj.AddComponent<Dice>();
        dice.Initialize(NowLevel);  // 初始化方法替代构造函数
        
        allDices.Add(dice);
        return dice;
    }


    public void ClearALLDices()
    {
        allDices.Clear(); // 清空所有骰子
    }

}

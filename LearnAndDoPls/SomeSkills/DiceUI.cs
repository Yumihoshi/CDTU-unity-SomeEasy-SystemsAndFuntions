using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

[RequireComponent(typeof(SpriteRenderer), typeof(AudioSource))]
public class DiceUI : MonoBehaviour
{
    private SpriteRenderer _spriteRenderer;

    [Serializable]
    public class Number_Sprites
    {
        public int level;
        public Sprite sprite;
    }
    public List<Number_Sprites> number_ImagesList = new();
    public Dictionary<int, Number_Sprites> Number_ImageDic = new();//Bug

    private void Awake()
    {
        _spriteRenderer = GetComponent<SpriteRenderer>();
        try
        {
            foreach (Number_Sprites number_Sprites in number_ImagesList)
            {
                Number_ImageDic[number_Sprites.level] = number_Sprites;
            }
        }
        catch (Exception e)
        {
            Debug.LogError("加载数字图片失败:" + e.Message + "请检查资源是否存在");
        }
    }

}
using System;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName ="Prize Data" ,menuName ="Data/Prize Data")]
public class PrizeDataSO : ScriptableObject
{
    public List<PrizeData> prizeDataList;
}

public enum Rarity { Normal, Rare, Epic, Legendary }

[Serializable]
public class PrizeData
{
    public Rarity rarity;
    public Sprite sprite;
    public float probability;
    public string name;
    public string brand;
    public int id;
    public int price;
}
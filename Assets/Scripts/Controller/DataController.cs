using System.Collections.Generic;
using UnityEngine;
using Random = UnityEngine.Random;

public class DataController : MonoSingleton<DataController>
{
    [SerializeField] private Rarity nextPrizeRarity;
    [SerializeField] private PrizeDataSO prizeDataSO;
    
    [SerializeField] private DebugUI debugUI;
    
    private List<PrizeData> prizeDataList;
    private PrizeData currentPrizeData;

    public PrizeData GetCurrentPrizeData()
    {
        return currentPrizeData;
    }
    
    /// <summary>
    /// 상품의 데이터들을 불러옵니다.
    /// </summary>
    public void LoadPrizeData()
    {
        prizeDataList = prizeDataSO.prizeDataList;
    }

    /// <summary>
    /// 테스트 / 해당 레어리티 등급의 상품의 정보를 가져옴
    /// </summary>
    public PrizeData GetNextPrize()
    {
        if (debugUI.GetCurrentRandomMode)
        {
            nextPrizeRarity = (Rarity)Random.Range(0, 4);
        }
        else
        {
            nextPrizeRarity = debugUI.GetCurrentLevel;
        }
        
        foreach (var currentPrizeData in prizeDataSO.prizeDataList)
        {
            if (currentPrizeData.rarity == nextPrizeRarity)
            {
                this.currentPrizeData = currentPrizeData;
                return currentPrizeData;
            }
        }

        return null;
    }
}

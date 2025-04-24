using System.Collections.Generic;
using UnityEngine;

public class DataController : MonoSingleton<DataController>
{
    [SerializeField] private Rarity nextPrizeRarity;
    [SerializeField] private PrizeDataSO prizeDataSO;
    
    private List<PrizeData> prizeData;

    public Rarity GetNextPrizeRairity() => nextPrizeRarity;

    /// <summary>
    /// 상품의 데이터들을 불러옵니다.
    /// </summary>
    public void LoadPrizeData()
    {
        prizeData = prizeDataSO.prizeDataList;
    }

    /// <summary>
    /// 테스트 / 해당 레어리티 등급의 상품의 정보를 가져옴
    /// </summary>
    public PrizeData GetNextPrize()
    {
        foreach (var currentPrizeData in prizeDataSO.prizeDataList)
        {
            if (currentPrizeData.rarity == nextPrizeRarity)
            {
                return currentPrizeData;
            }
        }

        return null;
    }
}

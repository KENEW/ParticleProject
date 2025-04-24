using System.Collections;
using UnityEngine;
using UnityEngine.UI;

public class Crane : MonoBehaviour
{
    [SerializeField] private UIController uiController;
    
    private bool isFever = false;
    private bool isBoxOpen = false;
    private bool isRareEffect = false;
    
    private int maxHitCount = 10;
    private int currentHitCount = 0;
    
    private PrizeData prizeData;
 
    public void AddHit() => currentHitCount++;
    
    private void Update()
    {
        // 특정 등급 이상일 경우 일정 이상 히트 후 결과 공개
        if (!isBoxOpen && isFever)
        {   
            // InputController등으로 따로 뺄 수 있음
            if (Input.GetMouseButtonDown(0))
            {
                HitFeverButton();        
            }
            
            if (currentHitCount >= 10)
            {
                PrizeProductionScreen();
            }
        }
        
        uiController.UpdateHitGauge(currentHitCount, maxHitCount);
        
        // 박스가 오픈 중인 상태일 때 카메라 흔들기
        if (isBoxOpen)
        {
            if (isRareEffect)
            {
                CameraController.Instance.OnShake(1.0f, 1.5f);
            }
            else
            {
                CameraController.Instance.OnShake(0.1f, 5f);
            }
        }
    }
    
    /// <summary>
    /// 에픽 등급 이상의 상품 출연 시, 화면을 클릭시 히트 카운터가 증가
    /// </summary>
    public void HitFeverButton()
    {
        AddHit();
        uiController.HitGiftBox();
    }

    /// <summary>
    /// 상품이 나오기 전 연출 화면
    /// </summary>
    public void PrizeProductionScreen()
    {
        SoundManager.Instance.PlaySFX("FeverChanceOpenSFX");
        CameraController.Instance.Scale = 5;
        isBoxOpen = true;
        
        StartCoroutine(GiftBoxResultCo());
    }

    /// <summary>
    /// 상품의 열릴 때 패널을 실행함
    /// </summary>
    private IEnumerator GiftBoxResultCo()
    {
        SoundManager.Instance.PlaySFX("BoxOpenSFX");
        yield return new WaitForSeconds(2f);
        
        isFever = false;
        isBoxOpen = false;

        uiController.RunScreenFade(false);
        
        yield return new WaitForSeconds(0.5f);
        
        uiController.ShowGifBoxResult();
        uiController.RunScreenFade(true);
    }
    
    /// <summary>
    /// 상품을 다시 뽑음
    /// </summary>
    public void ReDraw()
    {
        currentHitCount = 0;
        CameraController.Instance.Scale = 7;
        
        prizeData = DataController.Instance.GetNextPrize();
        
        uiController.ReDraw();
        uiController.SetPrizeResultUI(prizeData);
        
        // 다음 선물 상자가 에픽 등급의 이상이면 효과를 보여준다.
        if (prizeData.rarity == Rarity.Epic || prizeData.rarity == Rarity.Legendary)
        {
            isRareEffect = true;
            isFever = true;
            uiController.ShowRareEffectUI(true);
        }
        else
        {
            isRareEffect = false;
            isFever = false;
            uiController.ShowRareEffectUI(false);
            PrizeProductionScreen();
        }
        
        SoundManager.Instance.PlaySFX("RedrawSFX");
    }
}

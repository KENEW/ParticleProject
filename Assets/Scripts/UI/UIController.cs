using System;
using System.Collections;
using UnityEngine;
using UnityEngine.Serialization;
using UnityEngine.UI;

public class UIController : MonoBehaviour
{
    [Header("PrizeText")]
    public Text prizeBrandText;
    public Text prizeNameText;
    public Text prizeProbabilityText;
    public Text prizePriceText;
    
    [Header("UIPanel")]
    public GameObject flashFadePanel;
    public GameObject giftBoxResultUIPanel;
    
    [Header("InGame")]
    public GameObject prizeImage;
    public Button giftBoxDrawButton;
    public GameObject effectFeverPanel;
    public Image gaugeImage;
    public GameObject giftBoxObject;
    public SpriteRenderer giftBoxSpriteRenderer;
    public Sprite[] giftBoxSprites = new Sprite[2]; // 0 : Close, 1 : Open
    
    [Header("Particle System")]
    public GameObject giftBoxRareEffect;
    public GameObject[] giftBoxEffects;
    public GameObject rareEffect;
    
    [Header("Aniamtor")]
    public Animator giftBoxAnimator;
    public Animator downPanelAnimator;
    public Animator giftThrowAnimator;
    
    private PrizeData prizeData;

    private float feverGaugeImageSizeX;
    
    private const int FEVER_CHANCE_GAUGE_SPEED = 8;
    
    private void Start()
    {
        Screen.SetResolution(720, 1280, false);
        feverGaugeImageSizeX = gaugeImage.rectTransform.sizeDelta.x;
    }
    
    /// <summary>
    /// 1회 히트의 기능을 표현
    /// </summary>
    public void HitGiftBox()
    {
        SoundManager.Instance.PlaySFX("FeverChanceHitSFX");
        giftBoxAnimator.Play("GiftBoxShake", 0, 0);
    }

    /// <summary>
    /// 히트 게이지를 표현
    /// </summary>
    public void UpdateHitGauge(int currentHitCount, int maxHitCount)
    {
        float gaugeX = Mathf.Lerp(gaugeImage.rectTransform.sizeDelta.x, feverGaugeImageSizeX - (feverGaugeImageSizeX / maxHitCount) * currentHitCount, Time.deltaTime * FEVER_CHANCE_GAUGE_SPEED);
        gaugeImage.rectTransform.sizeDelta = new Vector2(gaugeX, gaugeImage.rectTransform.sizeDelta.y);
    }
    
    /// <summary>
    ///  특정 이상의 레어 등급일 경우 UI 효과를 출력
    /// </summary>
    public void ShowRareEffectUI(bool isActive)
    {
        if (isActive)
        {
            effectFeverPanel.SetActive(true);
            giftBoxRareEffect.SetActive(true);
        }
        else
        {
            effectFeverPanel.SetActive(false);
            giftBoxRareEffect.SetActive(false);
        }
    }
    
    /// <summary>
    /// 상품의 이미지를 출력
    /// </summary>
    public void ShowPrizeImage()
    {
        prizeImage.SetActive(true);
        prizeImage.GetComponent<Image>().sprite = DataController.Instance.GetCurrentPrizeData().sprite;
        prizeImage.GetComponent<Image>().SetNativeSize();
    }

    /// <summary>
    /// 선물 상자의 UI 적용
    /// </summary>
    /// <param name="prizeData"></param>
    public void SetPrizeResultUI(PrizeData prizeData)
    {
        giftThrowAnimator.Play("GiftBoxThrow", 0, 0);
        this.prizeData = prizeData;
        
        prizeBrandText.text = prizeData.brand;
        prizeNameText.text = prizeData.name;
        prizeProbabilityText.text = prizeData.probability + "%";
        prizePriceText.text = string.Format("{0:n0}", prizeData.price) + "원";
    }

    /// <summary>
    /// 다시 뽑을 때 UI 초기화
    /// </summary>
    public void ReDraw()
    {
        prizeImage.SetActive(false);
        giftBoxResultUIPanel.SetActive(false);

        giftBoxDrawButton.interactable = false;
        giftBoxSpriteRenderer.sprite = giftBoxSprites[0];

        downPanelAnimator.SetFloat("Speed", 1f);
        downPanelAnimator.Play("DownPanelFade", 0, 0);
        giftThrowAnimator.Play("GiftBoxThrow", 0, 0);
        gaugeImage.rectTransform.sizeDelta = new Vector2(feverGaugeImageSizeX, gaugeImage.rectTransform.sizeDelta.y);
        
        foreach (GameObject g in giftBoxEffects)
        {
            g.SetActive(false);
        }
        
        giftBoxObject.SetActive(true);
    }
    
    /// <summary>
    /// 상품이 출연 시
    /// </summary>
    public void ShowGifBoxResult()
    {
        effectFeverPanel.SetActive(false);
        giftBoxDrawButton.interactable = true;
        downPanelAnimator.SetFloat("Speed", -1f);
        downPanelAnimator.Play("DownPanelFade");
    }
    
    /// <summary>
    /// 선물 상자가 열릴 때
    /// </summary>
    public void ShowPrizeResult()
    {
        giftBoxSpriteRenderer.sprite = giftBoxSprites[1];
        effectFeverPanel.SetActive(false);
    }

    /// <summary>
    /// 페이드 화면을 출력
    /// </summary>
    public void RunScreenFade(Action callBack = null, bool isReverse = false)
    {
        if (!isReverse)
        {
            StartCoroutine(FlashFade(callBack));
        }
        else
        {
            StopCoroutine(FlashFadeReverse());
        }
    }
    
    /// <summary>
    /// 상품이 공개될 때 빛이 번쩍하는 페이드 효과를 적용
    /// </summary>
    private IEnumerator FlashFade(Action callBack)
    {
        Image flashFadePanelImage = flashFadePanel.GetComponent<Image>();
        
        flashFadePanelImage.color = new Color(1f, 1f, 1f, 0f);
        SoundManager.Instance.PlaySFX("FadeSFX");

        for (float i = 0; i <= 1.0f; i = i + 0.05f)
        {
            flashFadePanelImage.color = new Color(flashFadePanelImage.color.r, 
                flashFadePanelImage.color.g, 
                flashFadePanelImage.color.b, 
                i);
            yield return new WaitForSeconds(0.05f);
        }
        
        flashFadePanelImage.color = Color.white;
        callBack?.Invoke();
        
        giftBoxRareEffect.SetActive(false);
        giftBoxObject.SetActive(false);
        giftBoxResultUIPanel.SetActive(true);
        prizeImage.SetActive(true);
        giftBoxEffects[(int)prizeData.rarity].SetActive(true);

        yield return new WaitForSeconds(0.5f);
        
        ShowPrizeImage();

        StartCoroutine(FlashFadeReverse());
    }
    
    /// <summary>
    /// 페이드 된 화면에서 다시 원 화면으로 회귀
    /// </summary>
    private IEnumerator FlashFadeReverse()
    {
        Image flashFadePanelImage = flashFadePanel.GetComponent<Image>();
        
        flashFadePanelImage.color = new Color(1f, 1f, 1f, 0f);

        for (float i = 1; i >= 0.1f; i = i - 0.05f)
        {
            flashFadePanelImage.color = new Color(flashFadePanelImage.color.r, 
                flashFadePanelImage.color.g, 
                flashFadePanelImage.color.b, 
                i);
            yield return new WaitForSeconds(0.05f);
        }
        
        flashFadePanelImage.color = Color.clear;

        if (prizeData.rarity == Rarity.Epic || prizeData.rarity == Rarity.Legendary)
        {
            SoundManager.Instance.PlaySFX("EpicRareSFX");
        }
        else
        {
            SoundManager.Instance.PlaySFX("NormalRareSFX");
        }
        
        yield return new WaitForSeconds(0.5f);        
    }
}

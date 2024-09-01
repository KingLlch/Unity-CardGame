using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class CardView : MonoBehaviour, IPointerClickHandler
{
    private static CardView _instance;

    public static CardView Instance
    {
        get
        {
            if (_instance == null)
            {
                _instance = FindObjectOfType<CardView>();
            }

            return _instance;
        }
    }

    public GameObject CardViewObject;
    public Image CardViewImage;
    public TextMeshProUGUI CardViewName;
    public TextMeshProUGUI CardViewSecondName;
    public TextMeshProUGUI CardViewDescription;

    public GameObject cardArmorGameObject;
    public TextMeshProUGUI CardArmor;

    public GameObject TimerGameObject;
    public TextMeshProUGUI CardTimer;

    public GameObject CardViewPointsGameObject;
    public GameObject CardViewMaxPointsGameObject;

    public TextMeshProUGUI CardViewPoints;
    public TextMeshProUGUI CardViewMaxPoints;

    public GameObject CardViewDescriptionParent;
    public CardViewDescription CardViewDescriptionPrefab;

    public Toggle CurrentPointToggle;

    public GameObject SpawnCardView;
    public Image SpawnCardViewImage;
    public TextMeshProUGUI SpawnCardViewName;
    public TextMeshProUGUI SpawnCardViewDescription;
    public TextMeshProUGUI SpawnCardPoints;

    public List<GameObject> StatusEffectDescriptionList = new List<GameObject>();

    private void Awake()
    {
        if (_instance == null)
        {
            _instance = this;
        }
    }

    private void Start()
    {
        for (int i = 0; i < CardEffectsDescriptionList.effectDescriptionList.Count; i++)
        {
            InstantiateStatusEffectDescription(i);
        }
    }

    public void ShowCard(CardInfoScript card)
    {
        transform.GetComponent<Image>().raycastTarget = true;
        CurrentPointToggle.isOn = true;

        Material imageMaterial = new Material(card.Image.material);
        CardViewImage.material = imageMaterial;
        imageMaterial.SetTexture("_Image", card.SelfCard.BaseCard.ImageTexture);
        imageMaterial.SetColor("_Color", card.SelfCard.BaseCard.ColorTheme);

        CardViewName.text = card.Name.text;
        CardViewSecondName.text = card.SecondName.text;
        CardViewDescription.text = card.Description.text;

        if (card.SelfCard.BaseCard.ArmorPoints > 0)
        {
            cardArmorGameObject.SetActive(true);
            CardArmor.text = card.SelfCard.BaseCard.ArmorPoints.ToString();
        }
        else
            cardArmorGameObject.SetActive(false);

        if (card.SelfCard.EndTurnActions.Timer > 0)
        {
            TimerGameObject.SetActive(true);
            CardTimer.text = card.SelfCard.EndTurnActions.Timer.ToString();
        }

        else 
            TimerGameObject.SetActive(false);

        CardViewName.colorGradient = new VertexGradient(card.SelfCard.BaseCard.ColorTheme, card.SelfCard.BaseCard.ColorTheme, Color.black, Color.black);
        CardViewSecondName.colorGradient = new VertexGradient(card.SelfCard.BaseCard.ColorTheme, card.SelfCard.BaseCard.ColorTheme, Color.black, Color.black);
        CardViewDescription.color = Color.black;

        ShowPoints(true);

        CardViewPoints.text = card.SelfCard.BaseCard.Points.ToString();
        if (card.SelfCard.BaseCard.Points < card.SelfCard.BaseCard.MaxPoints)
            CardViewPoints.colorGradient = new VertexGradient(Color.red, Color.red, Color.white, Color.white);
        else if (card.SelfCard.BaseCard.Points > card.SelfCard.BaseCard.MaxPoints)
            CardViewPoints.colorGradient = new VertexGradient(Color.green, Color.green, Color.white, Color.white);
        else
            CardViewPoints.colorGradient = new VertexGradient(Color.white, Color.white, Color.white, Color.white);

        CardViewMaxPoints.text = card.SelfCard.BaseCard.MaxPoints.ToString();

        CheckStatusEffectGameObject(card);
        SpawnCardView.SetActive(false);

        if (card.SelfCard.Spawns.SpawnCardCount != 0 && card.SelfCard.Spawns.SpawnCardNumber == -1)
            ShowSpawnCardView(card, true, true);
        else if (card.SelfCard.Spawns.SpawnCardCount != 0)
            ShowSpawnCardView(card, true);
        else if(card.SelfCard.UniqueMechanics.TransformationNumber != -1)
            ShowSpawnCardView(card, false);
    }

    private void ShowSpawnCardView(CardInfoScript card, bool isSpawn, bool selfIllusion = false)
    {
        SpawnCardView.SetActive(true);

        Material imageMaterial = new Material(card.Image.material);
        SpawnCardViewImage.material = imageMaterial;
        Card SpawnCard;

        if (isSpawn && selfIllusion)
            SpawnCard = card.SelfCard;
        else if (isSpawn && !selfIllusion)
            SpawnCard = CardManagerList.SummonCards[card.SelfCard.Spawns.SpawnCardNumber];
        else
            SpawnCard = CardManagerList.TransformationCards[card.SelfCard.UniqueMechanics.TransformationNumber];

        imageMaterial.SetTexture("_Image", SpawnCard.BaseCard.ImageTexture);
        imageMaterial.SetColor("_Color", SpawnCard.BaseCard.ColorTheme);

        SpawnCardPoints.text = SpawnCard.BaseCard.Points.ToString();

        SpawnCardViewName.text = SpawnCard.BaseCard.Name;
        SpawnCardViewName.colorGradient = new VertexGradient(SpawnCard.BaseCard.ColorTheme, SpawnCard.BaseCard.ColorTheme, Color.black, Color.black);
        SpawnCardViewDescription.color = Color.black;

        if (!selfIllusion)
            SpawnCardViewDescription.text = SpawnCard.BaseCard.DescriptionUk;
        else
            SpawnCardViewDescription.text = "Illusion.";

    }

    private void InstantiateStatusEffectDescription(int NumberEffects)
    {
        CardViewDescription cardViewDescription = Instantiate(CardViewDescriptionPrefab, Vector3.forward, Quaternion.identity, CardViewDescriptionParent.transform);
        cardViewDescription.Image.sprite = CardEffectsDescriptionList.effectDescriptionList[NumberEffects].EffectImage;


        if(FindObjectOfType<LocalizationManager>() != null)
        switch (LocalizationManager.Instance.Language)
        {
            case "en":
                cardViewDescription.Name.text = CardEffectsDescriptionList.effectDescriptionList[NumberEffects].NameEng;
                cardViewDescription.Description.text = CardEffectsDescriptionList.effectDescriptionList[NumberEffects].DescriptionEng;
                break;

            case "ru":
                cardViewDescription.Name.text = CardEffectsDescriptionList.effectDescriptionList[NumberEffects].NameRu;
                cardViewDescription.Description.text = CardEffectsDescriptionList.effectDescriptionList[NumberEffects].DescriptionRu;
                break;

            case "uk":
                cardViewDescription.Name.text = CardEffectsDescriptionList.effectDescriptionList[NumberEffects].NameUk;
                cardViewDescription.Description.text = CardEffectsDescriptionList.effectDescriptionList[NumberEffects].DescriptionUk;
                break;
        }
        else
        {
            cardViewDescription.Name.text = CardEffectsDescriptionList.effectDescriptionList[NumberEffects].NameEng;
            cardViewDescription.Description.text = CardEffectsDescriptionList.effectDescriptionList[NumberEffects].DescriptionEng;
        }

        StatusEffectDescriptionList.Add(cardViewDescription.gameObject);
    }

    private void CheckStatusEffectGameObject(CardInfoScript card)
    {
        int countStatuseffects = 0;

        if (card.SelfCard.UniqueMechanics.DestroyCardPoints != 0)
        {
            StatusEffectDescriptionList[0].SetActive(true);
            countStatuseffects++;
        }
        else
            StatusEffectDescriptionList[0].SetActive(false);

        if (card.SelfCard.BoostOrDamage.Damage > 0 || card.SelfCard.BoostOrDamage.SelfDamage > 0 || card.SelfCard.EndTurnActions.EndTurnNearDamage > 0 || card.SelfCard.EndTurnActions.EndTurnRandomDamage > 0 || card.SelfCard.EndTurnActions.EndTurnSelfDamage > 0)
        {
            StatusEffectDescriptionList[1].SetActive(true);
            countStatuseffects++;
        }
        else
            StatusEffectDescriptionList[1].SetActive(false);

        if (card.SelfCard.BoostOrDamage.Boost > 0 || card.SelfCard.BoostOrDamage.SelfBoost > 0 || card.SelfCard.EndTurnActions.EndTurnNearBoost > 0 || card.SelfCard.EndTurnActions.EndTurnRandomBoost > 0 || card.SelfCard.EndTurnActions.EndTurnSelfBoost > 0)
        {
            StatusEffectDescriptionList[2].SetActive(true);
            countStatuseffects++;
        }
        else
            StatusEffectDescriptionList[2].SetActive(false);

        if (card.SelfCard.Spawns.SpawnCardCount > 0)
        {
            StatusEffectDescriptionList[3].SetActive(true);
            countStatuseffects++;
        }
        else
            StatusEffectDescriptionList[3].SetActive(false);

        if (card.SelfCard.DrawCard.DrawCardCount > 0)
        {
            StatusEffectDescriptionList[4].SetActive(true);
            countStatuseffects++;
        }
        else
            StatusEffectDescriptionList[4].SetActive(false);

        if (card.SelfCard.BoostOrDamage.NearDamage > 0 || card.SelfCard.BoostOrDamage.NearBoost > 0 || card.SelfCard.EndTurnActions.EndTurnNearDamage > 0 || card.SelfCard.EndTurnActions.EndTurnNearBoost > 0)
        {
            StatusEffectDescriptionList[5].SetActive(true);
            countStatuseffects++;
        }
        else
            StatusEffectDescriptionList[5].SetActive(false);

        if (card.SelfCard.BaseCard.ArmorPoints > 0)
        {
            StatusEffectDescriptionList[6].SetActive(true);
            countStatuseffects++;
        }
        else
            StatusEffectDescriptionList[6].SetActive(false);

        if (card.SelfCard.StatusEffects.IsStunOther || card.SelfCard.StatusEffects.IsSelfStunned)
        {
            StatusEffectDescriptionList[7].SetActive(true);
            countStatuseffects++;
        }
        else
            StatusEffectDescriptionList[7].SetActive(false);

        if (card.SelfCard.StatusEffects.IsSelfShielded || card.SelfCard.StatusEffects.IsShieldOther)
        {
            StatusEffectDescriptionList[8].SetActive(true);
            countStatuseffects++;
        }
        else
            StatusEffectDescriptionList[8].SetActive(false);

        if (card.SelfCard.StatusEffects.IsIllusion || card.SelfCard.Spawns.SpawnCardNumber == -1)
        {
            StatusEffectDescriptionList[9].SetActive(true);
            countStatuseffects++;
        }
        else
            StatusEffectDescriptionList[9].SetActive(false);

        if (card.SelfCard.StatusEffects.IsInvisibility)
        {
            StatusEffectDescriptionList[10].SetActive(true);
            countStatuseffects++;
        }
        else
            StatusEffectDescriptionList[10].SetActive(false);

        if (card.SelfCard.StatusEffects.IsInvulnerability)
        {
            StatusEffectDescriptionList[11].SetActive(true);
            countStatuseffects++;
        }
        else
            StatusEffectDescriptionList[11].SetActive(false);

        if (card.SelfCard.StatusEffects.SelfEnduranceOrBleeding < 0 || card.SelfCard.StatusEffects.EnduranceOrBleedingOther < 0)
        {
            StatusEffectDescriptionList[12].SetActive(true);
            countStatuseffects++;
        }
        else
            StatusEffectDescriptionList[12].SetActive(false);

        if (card.SelfCard.StatusEffects.SelfEnduranceOrBleeding > 0 || card.SelfCard.StatusEffects.EnduranceOrBleedingOther > 0)
        {
            StatusEffectDescriptionList[13].SetActive(true);
            countStatuseffects++;
        }
        else
            StatusEffectDescriptionList[13].SetActive(false);

        if (card.SelfCard.EndTurnActions.Timer > 0 )
        {
            StatusEffectDescriptionList[14].SetActive(true);
            countStatuseffects++;
        }
        else
            StatusEffectDescriptionList[14].SetActive(false);


        int height = (countStatuseffects * 200 + (countStatuseffects - 1) * 20) / 2 + 20;

        if (height > 1000)
            CardViewDescriptionParent.GetComponent<RectTransform>().sizeDelta = new Vector2(CardViewDescriptionParent.GetComponent<RectTransform>().sizeDelta.x, height);
        else
            CardViewDescriptionParent.GetComponent<RectTransform>().sizeDelta = new Vector2(CardViewDescriptionParent.GetComponent<RectTransform>().sizeDelta.x, 0);
    }

    public void ShowPoints(bool IsCurrentPoints)
    {
        if (IsCurrentPoints)
        {
            CardViewMaxPointsGameObject.SetActive(false);
            CardViewPointsGameObject.SetActive(true);
        }
        else
        {
            CardViewPointsGameObject.SetActive(false);
            CardViewMaxPointsGameObject.SetActive(true);
        }
    }

    public void OnPointerClick(PointerEventData eventData)
    {
        transform.GetComponent<Image>().raycastTarget = false;
        CardViewObject.SetActive(false);
    }
}

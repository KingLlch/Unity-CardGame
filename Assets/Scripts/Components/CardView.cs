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

    public GameObject CardViewPointsGameObject;
    public GameObject CardViewMaxPointsGameObject;

    public TextMeshProUGUI CardViewPoints;
    public TextMeshProUGUI CardViewMaxPoints;

    public GameObject[] StatusEffects;

    public Toggle CurrentPointToggle;

    public Sprite shieldImage;
    public Sprite illusionImage;
    public Sprite invisibilityImage;
    public Sprite stunImage;
    public Sprite invulnerabilityImage;
    public Sprite bleedingImage;
    public Sprite enduranceImage;

    public GameObject SpawnCardView;
    public Image SpawnCardViewImage;
    public TextMeshProUGUI SpawnCardViewName;
    public TextMeshProUGUI SpawnCardViewDescription;
    public TextMeshProUGUI SpawnCardPoints;

    private void Awake()
    {
        if (_instance == null)
        {
            _instance = this;
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

        CardViewName.colorGradient = new VertexGradient(card.SelfCard.BaseCard.ColorTheme, card.SelfCard.BaseCard.ColorTheme, Color.black, Color.black);
        CardViewSecondName.colorGradient = new VertexGradient(card.SelfCard.BaseCard.ColorTheme, card.SelfCard.BaseCard.ColorTheme, Color.black, Color.black);
        CardViewDescription.colorGradient = new VertexGradient(card.SelfCard.BaseCard.ColorTheme, card.SelfCard.BaseCard.ColorTheme, Color.black, Color.black);

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

        if (card.SelfCard.Summons.SpawnCardCount != 0 && card.SelfCard.Summons.SpawnCardNumber == -1)
            ShowSpawnCardView(card, true, true);
        else if (card.SelfCard.Summons.SpawnCardCount != 0)
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
            SpawnCard = CardManagerList.SummonCards[card.SelfCard.Summons.SpawnCardNumber];
        else
            SpawnCard = CardManagerList.TransformationCards[card.SelfCard.UniqueMechanics.TransformationNumber];

        imageMaterial.SetTexture("_Image", SpawnCard.BaseCard.ImageTexture);
        imageMaterial.SetColor("_Color", SpawnCard.BaseCard.ColorTheme);

        SpawnCardPoints.text = SpawnCard.BaseCard.Points.ToString();

        SpawnCardViewName.text = SpawnCard.BaseCard.Name;

        if(!selfIllusion)
            SpawnCardViewDescription.text = SpawnCard.BaseCard.Description;
        else
            SpawnCardViewDescription.text = "Illusion.";
    }

    private void CheckStatusEffectGameObject(CardInfoScript card)
    {
        if (card.SelfCard.StatusEffects.IsSelfShielded || card.SelfCard.StatusEffects.IsShieldOther)
            StatusEffects[0].SetActive(true);
        else
            StatusEffects[0].SetActive(false);

        if (card.SelfCard.StatusEffects.IsIllusion)
            StatusEffects[1].SetActive(true);
        else
            StatusEffects[1].SetActive(false);

        if (card.SelfCard.StatusEffects.IsStunOther || card.SelfCard.StatusEffects.IsSelfStunned)
            StatusEffects[2].SetActive(true);
        else
            StatusEffects[2].SetActive(false);

        if (card.SelfCard.StatusEffects.IsInvisibility)
            StatusEffects[3].SetActive(true);
        else
            StatusEffects[3].SetActive(false);

        if (card.SelfCard.StatusEffects.IsInvulnerability)
            StatusEffects[4].SetActive(true);
        else
            StatusEffects[4].SetActive(false);

        if (card.SelfCard.StatusEffects.SelfEnduranceOrBleeding < 0 || card.SelfCard.StatusEffects.EnduranceOrBleedingOther < 0)
            StatusEffects[5].SetActive(true);
        else
            StatusEffects[5].SetActive(false);

        if (card.SelfCard.StatusEffects.SelfEnduranceOrBleeding > 0 || card.SelfCard.StatusEffects.EnduranceOrBleedingOther > 0)
            StatusEffects[6].SetActive(true);
        else
            StatusEffects[6].SetActive(false);
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

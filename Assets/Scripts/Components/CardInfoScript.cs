using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class CardInfoScript : MonoBehaviour
{
    public Card SelfCard;

    public GameObject CardComponents;
    public GameObject DescriptionObject;
    public GameObject CardBack;
    public GameObject PointObject;
    public GameObject ShadowCard;
    public GameObject ShadowPoint;
    public GameObject DestroyGameObject;

    public Image Image;
    public Image ImageEdge1;
    public Image DestroyImage;

    public Image CardStatusEffectImage;

    public TextMeshProUGUI Point;
    public TextMeshProUGUI Name;
    public TextMeshProUGUI SecondName;
    public TextMeshProUGUI Description;

    public int SiblingIndex;
    public bool IsHideCard;
    public bool IsAnimationCard;
    public bool IsOrderCard;
    public bool IsDeckBuildCard;

    public GameObject StatusEffectPrefab;

    public GameObject BleedingPanel;
    public TextMeshProUGUI BleedingPanelText;

    [HideInInspector] public GameObject StatusEffectShield;
    [HideInInspector] public GameObject StatusEffectIllusion;
    [HideInInspector] public GameObject StatusEffectStunned;
    [HideInInspector] public GameObject StatusEffectInvisibility;
    [HideInInspector] public GameObject StatusEffectInvulnerability;

    public int ShowPoints(Card card)
    {
        return card.BaseCard.Points;
    }

    public void ShowCardInfo(Card card)
    {
        SelfCard = card;

        CardBack.SetActive(false);
        PointObject.SetActive(true);
        ShadowCard.SetActive(true);
        ShadowPoint.SetActive(true);
        IsHideCard = false;
        Point.text = card.BaseCard.Points.ToString();
        Name.text = card.BaseCard.Name.ToString();
        SecondName.text = card.BaseCard.AbilityName.ToString();
        Description.text = card.BaseCard.Description.ToString();

        Name.colorGradient = new VertexGradient(card.BaseCard.ColorTheme, card.BaseCard.ColorTheme, Color.black, Color.black);
        SecondName.colorGradient = new VertexGradient(card.BaseCard.ColorTheme, card.BaseCard.ColorTheme, Color.black, Color.black);
        Description.colorGradient = new VertexGradient(card.BaseCard.ColorTheme, card.BaseCard.ColorTheme, Color.black, Color.black);

        Material imageMaterial = new Material(Image.material);

        Image.material = imageMaterial;

        imageMaterial.SetTexture("_Image", card.BaseCard.ImageTexture);
        imageMaterial.SetColor("_Color", card.BaseCard.ColorTheme);
    }

    public void HideCardInfo(Card card)
    {
        SelfCard = card;
        CardBack.SetActive(true);
        PointObject.SetActive(false);
        ShadowCard.SetActive(false);
        ShadowPoint.SetActive(false);
        IsHideCard = true;
    }

    public void ShowDescription()
    {
        if ((IsDeckBuildCard) || ((!IsHideCard) && (!GameManager.Instance.IsDrag) && (!IsAnimationCard) && (!IsOrderCard) && (DescriptionObject != null)))
        {
            DescriptionObject.SetActive(true);
            DescriptionObject.transform.SetParent(transform.parent.parent);
        }
    }

    public void HideDescription()
    {
        if (DescriptionObject != null)
        {
            DescriptionObject.SetActive(false);
            DescriptionObject.transform.SetParent(transform);
        }
    }

    public void CheckSiblingIndex()
    {
        SiblingIndex = transform.GetSiblingIndex();
    }
}

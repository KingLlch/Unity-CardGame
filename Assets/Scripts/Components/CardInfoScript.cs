using TMPro;
using UnityEngine;
using UnityEngine.SceneManagement;
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

    public GameObject StatusEffectPrefab;

    [HideInInspector] public GameObject StatusEffectShield;
    [HideInInspector] public GameObject StatusEffectIllusion;
    [HideInInspector] public GameObject StatusEffectStunned;
    [HideInInspector] public GameObject StatusEffectInvisibility;
    [HideInInspector] public GameObject StatusEffectInvulnerability;

    public int ShowPoints(Card card)
    {
        return card.Points;
    }

    public void ChangePoints(ref Card targetCard, int value, Card startCard)
    {
        if ((targetCard.StatusEffects.IsIllusion) && (value < 0))
        {
            value += value;
        }

        if ((targetCard.StatusEffects.IsShielded) && (value < 0))
        {
            value = 0;
            targetCard.StatusEffects.IsShielded = false;
        }

        targetCard.Points += value;

        Debug.Log(startCard.Name + " изменила силу " + targetCard.Name + " в размере " + value + "\n" + (targetCard.Points - value) + " => " + targetCard.Points);

        ShowPointsUI(targetCard);
    }

    public void ShowCardInfo(Card card)
    {
        SelfCard = card;

        CardBack.SetActive(false);
        PointObject.SetActive(true);
        ShadowCard.SetActive(true);
        ShadowPoint.SetActive(true);
        IsHideCard = false;
        Point.text = card.Points.ToString();
        Name.text = card.Name.ToString();
        SecondName.text = card.SecondName.ToString();
        Description.text = card.Description.ToString();

        Name.colorGradient = new VertexGradient(card.ColorTheme, card.ColorTheme, Color.black, Color.black);
        SecondName.colorGradient = new VertexGradient(card.ColorTheme, card.ColorTheme, Color.black, Color.black);
        Description.colorGradient = new VertexGradient(card.ColorTheme, card.ColorTheme, Color.black, Color.black);

        Material imageMaterial = new Material(Image.material);

        Image.material = imageMaterial;

        imageMaterial.SetTexture("_Image", card.ImageTexture);
        imageMaterial.SetColor("_Color", card.ColorTheme);
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
        if ((SceneManager.sceneCount == 1) || ((!IsHideCard) && (!GameManager.Instance.IsDrag) && (!IsAnimationCard) && (!IsOrderCard) && (DescriptionObject != null)))
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

    public void ShowPointsUI(Card card)
    {
        Point.text = card.Points.ToString();
    }

    public void CheckSiblingIndex()
    {
        SiblingIndex = transform.GetSiblingIndex();
    }
}

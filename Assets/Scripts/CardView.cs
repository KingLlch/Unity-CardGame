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

    public GameObject[] StatusEffects;

    public Sprite shieldImage;
    public Sprite illusionImage;
    public Sprite invisibilityImage;
    public Sprite stunImage;
    public Sprite invulnerabilityImage;

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

        Material imageMaterial = new Material(card.Image.material);
        CardViewImage.material = imageMaterial;
        imageMaterial.SetTexture("_Image", card.SelfCard.Image);
        imageMaterial.SetColor("_Color", card.SelfCard.ColorTheme);

        CardViewName.text = card.Name.text;
        CardViewSecondName.text = card.SecondName.text;
        CardViewDescription.text = card.Description.text;

        CardViewName.colorGradient = new VertexGradient(card.SelfCard.ColorTheme, card.SelfCard.ColorTheme, Color.black, Color.black);
        CardViewSecondName.colorGradient = new VertexGradient(card.SelfCard.ColorTheme, card.SelfCard.ColorTheme, Color.black, Color.black);
        CardViewDescription.colorGradient = new VertexGradient(card.SelfCard.ColorTheme, card.SelfCard.ColorTheme, Color.black, Color.black);


        if (card.SelfCard.StatusEffects.IsShielded)
            StatusEffects[0].SetActive(true);
        else
            StatusEffects[0].SetActive(false);

        if (card.SelfCard.StatusEffects.IsIllusion)
            StatusEffects[1].SetActive(true);
        else
            StatusEffects[1].SetActive(false);

        if (card.SelfCard.StatusEffects.IsStun)
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
    }

    public void OnPointerClick(PointerEventData eventData)
    {
        transform.GetComponent<Image>().raycastTarget = false;
        CardViewObject.SetActive(false);
    }
}

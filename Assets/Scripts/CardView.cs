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
    public TextMeshProUGUI CardViewDescription;

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
        CardViewDescription.text = card.Description.text;
    }

    public void OnPointerClick(PointerEventData eventData)
    {
        transform.GetComponent<Image>().raycastTarget = false;
        CardViewObject.SetActive(false);
    }
}

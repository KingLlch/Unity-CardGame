using UnityEngine;
using UnityEngine.UI;

public class StatusEffect : MonoBehaviour
{
    [SerializeField] private Sprite shieldImage;
    [SerializeField] private Sprite illusionImage;
    [SerializeField] private Sprite invisibilityImage;
    [SerializeField] private Sprite stunImage;

    public void Initialize(StatusEffectsType type)
    {
        if (type == StatusEffectsType.shield)
        transform.GetComponent<Image>().sprite = shieldImage;

        if (type == StatusEffectsType.illusion)
            transform.GetComponent<Image>().sprite = illusionImage;

        if (type == StatusEffectsType.invisibility)
            transform.GetComponent<Image>().sprite = invisibilityImage;

        if (type == StatusEffectsType.stun)
            transform.GetComponent<Image>().sprite = stunImage;
    }
}

public enum StatusEffectsType
{
    shield,
    illusion,
    invisibility,
    stun
}


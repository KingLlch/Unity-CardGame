using UnityEngine;
using UnityEngine.UI;

public class StatusEffect : MonoBehaviour
{
    public Sprite shieldImage;
    public Sprite illusionImage;
    public Sprite invisibilityImage;
    public Sprite stunImage;
    public Sprite invulnerabilityImage;

    public void InitializeStatusEffect(StatusEffectsType type)
    {
        if (type == StatusEffectsType.shield)
            transform.GetComponent<Image>().sprite = shieldImage;

        if (type == StatusEffectsType.illusion)
            transform.GetComponent<Image>().sprite = illusionImage;

        if (type == StatusEffectsType.invisibility)
            transform.GetComponent<Image>().sprite = invisibilityImage;

        if (type == StatusEffectsType.stun)
            transform.GetComponent<Image>().sprite = stunImage;

        if (type == StatusEffectsType.invulnerability)
            transform.GetComponent<Image>().sprite = invulnerabilityImage;
    }
}

public enum StatusEffectsType
{
    shield = 0,
    illusion = 1,
    invisibility = 2,
    stun = 3,
    invulnerability = 4,
    bleeding = 5,
    endurance = 6
}


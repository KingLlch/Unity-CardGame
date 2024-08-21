using UnityEngine;
using UnityEngine.UI;

public class StatusEffect : MonoBehaviour
{
    public void Initialize(StatusEffectsType type)
    {
        if (type == StatusEffectsType.shield)
            transform.GetComponent<Image>().sprite = CardView.Instance.shieldImage;

        if (type == StatusEffectsType.illusion)
            transform.GetComponent<Image>().sprite = CardView.Instance.illusionImage;

        if (type == StatusEffectsType.invisibility)
            transform.GetComponent<Image>().sprite = CardView.Instance.invisibilityImage;

        if (type == StatusEffectsType.stun)
            transform.GetComponent<Image>().sprite = CardView.Instance.stunImage;

        if (type == StatusEffectsType.invulnerability)
            transform.GetComponent<Image>().sprite = CardView.Instance.invulnerabilityImage;

        if (type == StatusEffectsType.bleeding)
            transform.GetComponent<Image>().sprite = CardView.Instance.bleedingImage;

        if (type == StatusEffectsType.endurance)
            transform.GetComponent<Image>().sprite = CardView.Instance.enduranceImage;
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


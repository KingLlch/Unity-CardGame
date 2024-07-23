using UnityEngine;
using UnityEngine.UI;

public class StatusEffect : MonoBehaviour
{
    [SerializeField] private Sprite shieldImage;

    public void Initialize(StatusEffectsType type)
    {
        if (type == StatusEffectsType.shield)
        transform.GetComponent<Image>().sprite = shieldImage;
    }
}

public enum StatusEffectsType
{
    shield,
    stun
}


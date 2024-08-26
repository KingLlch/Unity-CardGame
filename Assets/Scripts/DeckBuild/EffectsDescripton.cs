using System.Collections;
using System.Collections.Generic;
using UnityEngine;
public struct EffectDescription
{
    public Sprite EffectImage;

    public string Name;
    public string Description;


    public EffectDescription(string effectImagePath, string name, string description) 
    {
        EffectImage = Resources.Load<Sprite>(effectImagePath);
        Name = name;
        Description = description;
    }
}

public static class CardEffectsDescriptionList
{ 
    public static List<EffectDescription> effectDescriptionList = new List<EffectDescription>();
}

public class EffectsDescripton : MonoBehaviour
{
    private void Awake()
    {
        CardEffectsDescriptionList.effectDescriptionList.Add(new EffectDescription("Sprites/Effects/Destroy", "Destroy", "Ñhange card points to 0."));

        CardEffectsDescriptionList.effectDescriptionList.Add(new EffectDescription("Sprites/Effects/Damage", "Damage", "Ñhange card points to - value."));

        CardEffectsDescriptionList.effectDescriptionList.Add(new EffectDescription("Sprites/Effects/Boost", "Boost", "Ñhange card points to + value."));

        CardEffectsDescriptionList.effectDescriptionList.Add(new EffectDescription("Sprites/Effects/Spawn", "Spawn", "Ñreate a unit."));

        CardEffectsDescriptionList.effectDescriptionList.Add(new EffectDescription("Sprites/Effects/Draw", "Draw", "Add first card from deck to your hand."));

        CardEffectsDescriptionList.effectDescriptionList.Add(new EffectDescription("Sprites/Effects/Near", "Near", "Ñards to the left and right of the selected one."));

        CardEffectsDescriptionList.effectDescriptionList.Add(new EffectDescription("Sprites/Effects/Armor", "Armor", "Block Damage"));

        CardEffectsDescriptionList.effectDescriptionList.Add(new EffectDescription("Sprites/Effects/Stun", "Stun", "The target's end of turn abilities are disabled for 1 turn."));

        CardEffectsDescriptionList.effectDescriptionList.Add(new EffectDescription("Sprites/Effects/Shield", "Shield", "Blocks 1 tick of damage."));

        CardEffectsDescriptionList.effectDescriptionList.Add(new EffectDescription("Sprites/Effects/Illusion", "Illusion", "Receives 2 times more damage."));

        CardEffectsDescriptionList.effectDescriptionList.Add(new EffectDescription("Sprites/Effects/Invisibility", "Invisibility", "The card must be played onto the enemy field."));

        CardEffectsDescriptionList.effectDescriptionList.Add(new EffectDescription("Sprites/Effects/Invulnerability", "Invulnerability", "Card cannot be targeted."));

        CardEffectsDescriptionList.effectDescriptionList.Add(new EffectDescription("Sprites/Effects/Bleeding", "Bleeding", "At the end of your turn damage card by 1 and change duration -1."));

        CardEffectsDescriptionList.effectDescriptionList.Add(new EffectDescription("Sprites/Effects/Endurance", "Endurance", "At the end of your turn boost card by 1 and change duration -1."));

        CardEffectsDescriptionList.effectDescriptionList.Add(new EffectDescription("Sprites/Effects/Timer", "Timer", "At the end of your turn change value timer by -1, if it becomes 0, apply the effect."));
    }
}



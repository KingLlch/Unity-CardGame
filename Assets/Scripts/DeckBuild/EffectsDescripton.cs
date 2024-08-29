using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.U2D;
public struct EffectDescription
{
    public Sprite EffectImage;

    public string NameEng;
    public string NameRu;
    public string NameUk;

    public string DescriptionEng;
    public string DescriptionRu;
    public string DescriptionUk;


    public EffectDescription(string effectImagePath, string nameEng, string nameRu, string nameUk, string descriptionEng, string descriptionRu, string descriptionUk) 
    {
        EffectImage = Resources.Load<SpriteAtlas>("Sprites/Effects/EffectSpiteAtlas").GetSprite(effectImagePath);
        NameEng = nameEng;
        NameRu = nameRu;
        NameUk = nameUk;

        DescriptionEng = descriptionEng;
        DescriptionRu = descriptionRu;
        DescriptionUk = descriptionUk;
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
        CardEffectsDescriptionList.effectDescriptionList.Add(new EffectDescription(
            "Destroy", 
            "Destroy", 
            "Уничтожьте", 
            "Знищте", 
            "Сhange card points to 0.", 
            "Измените очки карты до 0.", 
            "Змініть очки картки на 0."
        ));

        CardEffectsDescriptionList.effectDescriptionList.Add(new EffectDescription(
            "Damage",
            "Damage",
            "Урон",
            "Шкода",
            "Сhange card points to - value.",
            "Измените очки карты на - значение.",
            "Змініть очки картки на - значення."
        ));

        CardEffectsDescriptionList.effectDescriptionList.Add(new EffectDescription(
            "Boost",
            "Boost",
            "Усиление",
            "Підсилення",
            "Сhange card points to + value.",
            "Измените очки карты на + значение.",
            "Змініть очки картки на + значення."
        ));

        CardEffectsDescriptionList.effectDescriptionList.Add(new EffectDescription(
            "Spawn",
            "Spawn",
            "Создание",
            "Створення",
            "Сreate a unit.",
            "Создайте отряд.",
            "Створіть загiн."
        ));

        CardEffectsDescriptionList.effectDescriptionList.Add(new EffectDescription(
            "Draw",
            "Draw card",
            "Добор карты",
            "Витяг картки",
            "Add first card from deck to your hand.",
            "Добавьте первую карту из колоды в вашу руку.",
            "Додайте першу карту з колоди до вашої руки."
        ));

        CardEffectsDescriptionList.effectDescriptionList.Add(new EffectDescription(
            "Near",
            "Near",
            "Рядом",
            "Поряд",
            "Cards to the left and right of the selected one.",
            "Карты слева и справа от выбранной.",
            "Карти зліва і справа від вибраної."
        ));

        CardEffectsDescriptionList.effectDescriptionList.Add(new EffectDescription(
            "Armor",
            "Armor",
            "Броня",
            "Броня",
            "Block Damage",
            "Блокирует урон.",
            "Блокує шкоду."
        ));

        CardEffectsDescriptionList.effectDescriptionList.Add(new EffectDescription(
            "Stun",
            "Stun",
            "Оглушение",
            "Оглушення",
            "The target's end of turn abilities are disabled for 1 turn.",
            "Способности цели в конце хода отключены на 1 ход.",
            "Здібності цілі в кінці ходу вимкнені на 1 хід."
        ));

        CardEffectsDescriptionList.effectDescriptionList.Add(new EffectDescription(
            "Shield",
            "Shield",
            "Щит",
            "Щит",
            "Blocks 1 tick of damage.",
            "Блокирует 1 получение урона.",
            "Блокує 1 одержання шкоди."
        ));

        CardEffectsDescriptionList.effectDescriptionList.Add(new EffectDescription(
            "Illusion",
            "Illusion",
            "Иллюзия",
            "Ілюзія",
            "Receives 2 times more damage.",
            "Получает в 2 раза больше урона.",
            "Отримує в 2 рази більше шкоди."
        ));

        CardEffectsDescriptionList.effectDescriptionList.Add(new EffectDescription(
            "Invisibility",
            "Invisibility",
            "Невидимость",
            "Невидимість",
            "The card must be played onto the enemy field.",
            "Карта должна быть сыграна на поле врага.",
            "Картку потрібно зіграти на полі ворога."
        ));

        CardEffectsDescriptionList.effectDescriptionList.Add(new EffectDescription(
            "Invulnerability",
            "Invulnerability",
            "Неуязвимость",
            "Невразливість",
            "Card cannot be targeted.",
            "Карта не может быть выбрана целью.",
            "Картка не може бути обрана ціллю."
        ));

        CardEffectsDescriptionList.effectDescriptionList.Add(new EffectDescription(
            "Bleeding",
            "Bleeding",
            "Кровотечение",
            "Кровотеча",
            "At the end of your turn damage card by 1 and change duration -1.",
            "В конце вашего хода наносите карте 1 урон и уменьшите продолжительность на 1.",
            "В кінці вашого ходу завдайте картці 1 шкоду і зменшіть тривалість на 1."
        ));

        CardEffectsDescriptionList.effectDescriptionList.Add(new EffectDescription(
            "Endurance",
            "Endurance",
            "Выносливость",
            "Витривалість",
            "At the end of your turn boost card by 1 and change duration -1.",
            "В конце вашего хода увеличьте очки карты на 1 и уменьшите продолжительность на 1.",
            "В кінці вашого ходу збільшіть очки картки на 1 і зменшіть тривалість на 1."
        ));

        CardEffectsDescriptionList.effectDescriptionList.Add(new EffectDescription(
            "Timer",
            "Timer",
            "Таймер",
            "Таймер",
            "At the end of your turn change value timer by -1, if it becomes 0, apply the effect.",
            "В конце вашего хода уменьшите значение таймера на 1, если оно станет 0, примените эффект.",
            "В кінці вашого ходу зменшіть значення таймера на 1, якщо воно стане 0, застосуйте ефект."
        ));
    }
}



using DG.Tweening;
using System.Collections;
using System.Xml.Serialization;
using UnityEngine;

public class EffectsManager : MonoBehaviour
{
    private static EffectsManager _instance;

    public static EffectsManager Instance
    {
        get
        {
            if (_instance == null)
            {
                _instance = FindObjectOfType<EffectsManager>();
            }

            return _instance;
        }
    }

    public ParticleSystem[] DamageParticle;
    public ParticleSystem[] DamageBurstParticle;

    public ParticleSystem[] BoostParticle;
    public ParticleSystem[] BoostBurstParticle;

    public Material destroyMaterial;
    public Material shieldMaterial;
    public Material illusionMaterial;
    public Material invisibilityMaterial;
    public Material invulnerabilityMaterial;

    public GameObject CardBack;
    public Transform PlayerDeck;
    public Transform EnemyDeck;

    [HideInInspector] public int ParticleZCoordinate = 50;
    [HideInInspector] public float ParticleTimeToMove = 0.4f;
    [HideInInspector] public float ShaderChangePointsTime = 1f;

    private void Awake()
    {
        if (_instance == null)
        {
            _instance = this;
        }
    }

    public void DrawCardEffect(Transform Hand, bool isPlayer)
    {
        CardBack.SetActive(true);
        if (isPlayer)
            CardBack.transform.position = PlayerDeck.transform.position;
        else 
            CardBack.transform.position = EnemyDeck.transform.position;
        CardBack.transform.DOMove(Hand.position, 0.3f);
    }

    public void HideDrawCardEffect()
    {
        CardBack.SetActive(false);
    }

    public void StartParticleEffects(Transform start, Transform end, int value)
    {
        if (value > 0)
        {
            if (start == end)
                ParticleEffects(start, end, true, true);
            else
                ParticleEffects(start, end, true, false, true);
        }
        else
        {
            if (start == end)
                ParticleEffects(start, end, false, true);
            else
                ParticleEffects(start, end, false, false, true);
        }
    }

    private void ParticleEffects(Transform start, Transform end, bool isBoost, bool isSelf, bool isStartDelay = false)
    {
        if (isBoost)
        {
            for (int i = 0; i < 9; i++)
            {
                if (!BoostParticle[i].isPlaying)
                {
                    if (!isSelf)
                    {

                        BoostParticle[i].transform.position = new Vector3(start.position.x, start.position.y, ParticleZCoordinate);
                        BoostParticle[i].Play();
                        BoostParticle[i].transform.DOMove(new Vector3(end.position.x, end.position.y, ParticleZCoordinate), ParticleTimeToMove);

                        if (isStartDelay)
                            BoostBurstParticle[i].startDelay = ParticleTimeToMove;
                        BoostBurstParticle[i].transform.position = new Vector3(end.position.x, end.position.y, ParticleZCoordinate);
                        BoostBurstParticle[i].Play();
                        break;
                    }
                    else
                    {
                        BoostBurstParticle[i].transform.position = new Vector3(start.position.x, start.position.y, ParticleZCoordinate);
                        BoostBurstParticle[i].Play();
                        break;
                    }
                }
            }
        }

        if (!isBoost)
        {
            for (int i = 0; i < 9; i++)
            {
                if (!DamageParticle[i].isPlaying)
                {
                    if (!isSelf)
                    {
                        DamageParticle[i].transform.position = new Vector3(start.position.x, start.position.y, ParticleZCoordinate);
                        DamageParticle[i].Play();
                        DamageParticle[i].transform.DOMove(new Vector3(end.position.x, end.position.y, ParticleZCoordinate), ParticleTimeToMove);

                        if (isStartDelay)
                            DamageBurstParticle[i].startDelay = ParticleTimeToMove;
                        DamageBurstParticle[i].transform.position = new Vector3(end.position.x, end.position.y, ParticleZCoordinate);
                        DamageBurstParticle[i].Play();
                        break;
                    }
                    else
                    {
                        DamageBurstParticle[i].transform.position = new Vector3(start.position.x, start.position.y, ParticleZCoordinate);
                        DamageBurstParticle[i].Play();
                        break;
                    }
                }
            }
        }
    }

    public void StartShaderEffect(CardInfoScript card, Color color)
    {
        if (!card.IsShaderActive)
        {
            StartCoroutine(ShaderEffect(card, color));
            card.IsShaderActive = true;
        }
    }

    private IEnumerator ShaderEffect(CardInfoScript card, Color color)
    {
        yield return new WaitForSeconds(ParticleTimeToMove);

        float damage = ShaderChangePointsTime;
        card.Image.material.SetFloat("_Damage", damage);
        card.Image.material.SetColor("_Color", color);

        while (damage > 0)
        {
            damage -= 0.05f;
            card.Image.material.SetFloat("_Damage", damage);
            yield return new WaitForSeconds(0.05f);
        }

        card.IsShaderActive = false;

        yield break;
    }

    public void StartDestroyCoroutine(CardInfoScript card)
    {
        card.PointObject.SetActive(false);
        card.CardComponents.SetActive(false);
        card.DestroyGameObject.SetActive(true);

        Material DestroyMaterial = new Material(destroyMaterial);
        card.DestroyImage.material = DestroyMaterial;
        DestroyMaterial.SetTexture("_Image", card.SelfCard.BaseCard.ImageTexture);
        DestroyMaterial.SetFloat("_Trashold", 0);

        StartCoroutine(DestroyEffectsCoroutine(card));
    }

    private IEnumerator DestroyEffectsCoroutine(CardInfoScript card)
    {
        yield return new WaitForSeconds(ParticleTimeToMove);

        float trashold = 0;

        while (trashold <= 1)
        {
            trashold += 0.05f;
            card.DestroyImage.material.SetFloat("_Trashold", trashold);
            yield return new WaitForSeconds(0.05f);
        }

        yield break;
    }
}

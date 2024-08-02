using DG.Tweening;
using System.Collections;
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

    private Coroutine DestroyCoroutine;

    public Material destroyMaterial;
    public Material shieldMaterial;
    public Material illusionMaterial;
    public Material invisibilityMaterial;
    public Material invulnerabilityMaterial;

    public GameObject CardBack;
    public Transform PlayerDeck;
    public Transform EnemyDeck;

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

    public void ParticleEffects(Transform start, Transform end, bool isBoost, bool isSelf = false)
    {
        if (isBoost)
        {
            for (int i = 0; i < 9; i++)
            {
                if (!BoostParticle[i].isPlaying)
                {
                    if (!isSelf)
                    {
                        BoostParticle[i].transform.position = new Vector3(start.position.x, start.position.y, 5);
                        BoostParticle[i].Play();
                        BoostParticle[i].transform.DOMove(new Vector3(end.position.x, end.position.y, 5), 0.2f);

                        BoostBurstParticle[i].transform.position = new Vector3(end.position.x, end.position.y, 5);
                        BoostBurstParticle[i].Play();
                        break;
                    }
                    else
                    {
                        BoostBurstParticle[i].transform.position = new Vector3(start.position.x, start.position.y, 5);
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
                        DamageParticle[i].transform.position = new Vector3(start.position.x, start.position.y, 5);
                        DamageParticle[i].Play();
                        DamageParticle[i].transform.DOMove(new Vector3(end.position.x, end.position.y, 5), 0.2f);

                        DamageBurstParticle[i].transform.position = new Vector3(end.position.x, end.position.y, 5);
                        DamageBurstParticle[i].Play();
                        break;
                    }
                    else
                    {
                        DamageBurstParticle[i].transform.position = new Vector3(start.position.x, start.position.y, 5);
                        DamageBurstParticle[i].Play();
                        break;
                    }
                }
            }
        }
    }

    public void StartDestroyCoroutine(CardInfoScript card)
    {
        card.PointObject.SetActive(false);
        card.CardComponents.SetActive(false);
        card.DestroyGameObject.SetActive(true);

        Material DestroyMaterial = new Material(destroyMaterial);
        card.DestroyImage.material = DestroyMaterial;
        DestroyMaterial.SetFloat("_Trashold", 0);
        DestroyCoroutine = StartCoroutine(DestroyEffectsCoroutine(card));
    }

    private IEnumerator DestroyEffectsCoroutine(CardInfoScript card)
    {
        float trashold = 0;

        while (trashold <= 1)
        {
            trashold += 0.05f;
            card.DestroyImage.material.SetFloat("_Trashold", trashold);
            yield return new WaitForSeconds(0.05f);
        }

        StopCoroutine(DestroyCoroutine);
    }
}

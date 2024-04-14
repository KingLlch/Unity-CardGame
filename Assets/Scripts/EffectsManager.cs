using DG.Tweening;
using UnityEngine;

public class EffectsManager : MonoBehaviour
{
    public ParticleSystem[] DamageParticle;
    public ParticleSystem[] DamageBurstParticle;

    public ParticleSystem[] BoostParticle;
    public ParticleSystem[] BoostBurstParticle;


    public void EndTurnBoost(Transform start, Transform end)
    {
        for (int i = 0; i < 9; i++)
        {
            if (!BoostParticle[i].isPlaying)
            {
                BoostParticle[i].transform.position = new Vector3(start.position.x, start.position.y, -50);
                BoostParticle[i].Play();
                BoostParticle[i].transform.DOMove(new Vector3(end.position.x, end.position.y, -50), 0.2f);

                BoostBurstParticle[i].transform.position = new Vector3(end.position.x, end.position.y, -50);
                BoostBurstParticle[i].Play();
                break;
            }
        }
    }

    public void EndTurnDamage(Transform start, Transform end)
    {
        for (int i = 0; i < 9; i++)
        {
            if (!DamageParticle[i].isPlaying)
            {
                DamageParticle[i].transform.position = new Vector3(start.position.x, start.position.y, -50);
                DamageParticle[i].Play();
                DamageParticle[i].transform.DOMove(new Vector3(end.position.x, end.position.y, -50), 0.2f);

                DamageBurstParticle[i].transform.position = new Vector3(end.position.x, end.position.y, -50);
                DamageBurstParticle[i].Play();
                break;
            }
        }
    }

    public void Boost(Transform start, Transform end)
    {
        for (int i = 0; i < 9; i++)
        {
            if (!BoostParticle[i].isPlaying)
            {
                BoostParticle[i].transform.position = new Vector3(start.position.x, start.position.y, -50);
                BoostParticle[i].Play();
                BoostParticle[i].transform.DOMove(new Vector3(end.position.x, end.position.y, -50), 0.2f);

                BoostBurstParticle[i].transform.position = new Vector3(end.position.x, end.position.y,-50);
                BoostBurstParticle[i].Play();
                break;
            }
        }
    }

    public void SelfBoost(Transform start)
    {
        for (int i = 0; i < 9; i++)
        {
            if (!BoostParticle[i].isPlaying)
            {
                BoostBurstParticle[i].transform.position = new Vector3(start.position.x, start.position.y, -50);
                BoostBurstParticle[i].Play();
                break;
            }
        }
    }

    public void Damage(Transform start, Transform end)
    {
        for (int i = 0; i < 9; i++)
        {
            if (DamageParticle[i].isPlaying == false)
            {
                DamageParticle[i].transform.position = new Vector3(start.position.x, start.position.y, -50);
                DamageParticle[i].Play();
                DamageParticle[i].transform.DOMove(new Vector3(end.position.x, end.position.y, -50), 0.2f);

                DamageBurstParticle[i].transform.position = new Vector3(end.position.x, end.position.y, -50);
                DamageBurstParticle[i].Play();
                break;
            }
        }
    }

    public void SelfDamage(Transform start)
    {
        for (int i = 0; i < 9; i++)
        {
            if (!DamageParticle[i].isPlaying)
            {
                DamageBurstParticle[i].transform.position = new Vector3(start.position.x, start.position.y, -50);
                DamageBurstParticle[i].Play();
                break;
            }
        }
    }
}

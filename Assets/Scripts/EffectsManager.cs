using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;
using Unity.VisualScripting;

public class EffectsManager : MonoBehaviour
{
    public ParticleSystem DamageParticle;
    public ParticleSystem DamageBurstParticle;
    public ParticleSystem BoostParticle;
    public ParticleSystem BoostBurstParticle;


    public void EndTurnBoost(Transform start, Transform end)
    {
        BoostParticle.transform.position = new Vector3(start.position.x, start.position.y, -50);
        BoostParticle.transform.DOMove(new Vector3(end.position.x, end.position.y, -50), 0.2f);
        BoostBurstParticle.transform.position = new Vector3(end.position.x, end.position.y, -50);
        BoostBurstParticle.Play();
    }

    public void EndTurnDamage(Transform start, Transform end)
    {
        DamageParticle.transform.position = new Vector3(start.position.x, start.position.y, -50);
        DamageParticle.transform.DOMove(new Vector3(end.position.x, end.position.y, -50), 0.2f);
        DamageBurstParticle.transform.position = new Vector3(end.position.x, end.position.y, -50);
        DamageBurstParticle.Play();
    }

    public void Boost(Transform start, Transform end)
    {
        BoostParticle.transform.position = new Vector3(start.position.x, start.position.y, -50);
        BoostParticle.transform.DOMove(new Vector3(end.position.x, end.position.y, -50), 0.2f);
        BoostBurstParticle.transform.position = new Vector3(end.position.x, end.position.y,-50);
        BoostBurstParticle.Play();
    }

    public void SelfBoost(Transform start)
    {
        BoostBurstParticle.transform.position = new Vector3(start.position.x, start.position.y, -50);
        BoostBurstParticle.Play();
    }

    public void Damage(Transform start, Transform end)
    {
        DamageParticle.transform.position = new Vector3(start.position.x, start.position.y, -50);
        DamageParticle.transform.DOMove(new Vector3(end.position.x, end.position.y, -50), 0.2f);
        DamageBurstParticle.transform.position = new Vector3(end.position.x, end.position.y, -50);
        DamageBurstParticle.Play();
    }

    public void SelfDamage(Transform start)
    {
        DamageBurstParticle.transform.position = new Vector3(start.position.x, start.position.y, -50);
        DamageBurstParticle.Play();
    }
}

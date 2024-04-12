using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;
using Unity.VisualScripting;

public class EffectsManager : MonoBehaviour
{
    public ParticleSystem DamageParticle;
    public ParticleSystem BoostParticle;


    public void EndTurnBoost(Transform start, Transform end)
    {
        BoostParticle.transform.position = new Vector3 (start.position.x, start.position.y,500);
        BoostParticle.transform.DOMove(new Vector3(end.position.x, end.position.y,500), 0.5f);
    }

    public void EndTurnDamage(Transform start, Transform end)
    {
        DamageParticle.transform.position = new Vector3(start.position.x, start.position.y, 500);
        DamageParticle.transform.DOMove(new Vector3(end.position.x, end.position.y, 500), 0.5f);
    }

    public void Boost(Transform start, Transform end)
    {
        BoostParticle.transform.position = new Vector3(start.position.x, start.position.y, 500);
        BoostParticle.transform.DOMove(new Vector3(end.position.x, end.position.y, 500), 0.5f);
    }

    public void SelfBoost(Transform start)
    {
        BoostParticle.transform.position = new Vector3(start.position.x, start.position.y, 500);
        BoostParticle.transform.DOMove(new Vector3(start.position.x, start.position.y, 500), 0.5f);
    }

    public void Damage(Transform start, Transform end)
    {
        DamageParticle.transform.position = new Vector3(start.position.x, start.position.y, 500);
        DamageParticle.transform.DOMove(new Vector3(end.position.x, end.position.y, 500), 0.5f);
    }

    public void SelfDamage(Transform start)
    {
        BoostParticle.transform.position = new Vector3(start.position.x, start.position.y, 500);
        DamageParticle.transform.DOMove(new Vector3(start.position.x, start.position.y, 500), 0.5f);
    }
}

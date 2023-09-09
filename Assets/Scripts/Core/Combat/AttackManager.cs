using System;
using System.Collections.Generic;
using UnityEngine;

public class AttackManager : MonoBehaviour
{
    [SerializeField] public List<AttackConfiguration> attackConfigurations = new List<AttackConfiguration>();
    [SerializeField] protected PolygonCollider2D hitbox;

    protected AttackConfiguration currentAttack;
    protected FactionId factionId;

    public virtual event Action<AnimationVariableId> SetAnimationTrigger; 
    public void Initialize(FactionId factionId)
    {
        this.factionId = factionId;
    }
    public virtual void Attack()
    {
        hitbox.points = attackConfigurations[0].GetHitbox().points;
        currentAttack = attackConfigurations[0];
        SetAnimationTrigger?.Invoke(AnimationVariableId.Attack1);
    }
    public virtual void StopAttack()
    {
        hitbox.enabled = false;
        //currentAttack = new AttackConfiguration();
    }
    public virtual void SetHitBox(bool enabled)
    {
        hitbox.enabled = enabled;
    }
    protected virtual void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.TryGetComponent<IDamageable>(out IDamageable damageable))
        {
            Debug.Log("Damage " + currentAttack.GetDamage());
            if (damageable.GetFactionId() != factionId) damageable.GetDamaged(currentAttack.GetDamage(),transform.position);
        }
    }
}

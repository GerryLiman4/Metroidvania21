using System;
using UnityEngine;

public class HealthManager : MonoBehaviour,IDamageable
{
    [SerializeField] protected int maxHealth = 1;
    [SerializeField] protected int currentHealth;
    [SerializeField] protected FactionId factionId;

    public virtual event Action<StateId> ChangeState;
    public virtual event Action<float, Vector3> knockbackTime;
    public virtual event Action<int> SetHealth;
    public virtual event Action<int> Damage;
    public virtual event Action<int> Heal;

    [SerializeField] protected float hurtStateDuration = 1.5f;
    [SerializeField] protected float hurtStateTimer = 0f;
    protected bool isHurtState;
    protected Vector3 damageDirection = new Vector3();
    public virtual void Initialize()
    {
        currentHealth = maxHealth;
        SetHealth?.Invoke(currentHealth);
    }
    public virtual void GetDamaged(int damage, Vector3 direction)
    {
        if (isHurtState) return;
        damageDirection = direction - transform.position;
        currentHealth -= damage;
        Damage?.Invoke(damage);
        if (currentHealth <= 0)
        {
            ChangeState?.Invoke(StateId.Dead);
            return;
        }
        isHurtState = true;
        hurtStateTimer = 0;
        ChangeState?.Invoke(StateId.Hurt);
    }

    public FactionId GetFactionId()
    {
        return factionId;
    }

    private void OnDestroy()
    {
        ChangeState = null;
        Heal = null;
        Damage = null;
        SetHealth = null;
        knockbackTime = null;
    }
}

using System;
using UnityEngine;

public class PlayerHealth : HealthManager
{
    public override event Action<StateId> ChangeState;
    public override event Action<int> SetHealth;
    public override event Action<int> Damage;
    public override event Action<int> Heal;

    public override event Action<float,Vector3> knockbackTime;
    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.B))
        {
            GetDamaged(1,new Vector3(0f,0f,0f));
        }
        if (isHurtState)
        {
            hurtStateTimer += Time.deltaTime;
            if(hurtStateTimer >= hurtStateDuration)
            {
                hurtStateTimer = 0f;
                isHurtState = false;
                ChangeState?.Invoke(StateId.Idle);
                return;
            }
            knockbackTime?.Invoke(hurtStateTimer,damageDirection);
        }
    }
    public override void Initialize()
    {
        currentHealth = maxHealth;
        SetHealth?.Invoke(currentHealth);
    }
    public override void GetDamaged(int damage,Vector3 direction)
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

    private void OnDestroy()
    {
        ChangeState = null;
        Heal = null;
        Damage = null;
        SetHealth = null;
        knockbackTime = null;
    }
}

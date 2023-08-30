using UnityEngine;

public interface IDamageable 
{
    public FactionId GetFactionId();
    public void GetDamaged(int damage,Vector3 direction);

}

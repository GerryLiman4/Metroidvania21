using System.Collections.Generic;
using UnityEngine;

public class AttackManager : MonoBehaviour
{
    [SerializeField] public List<AttackConfiguration> attackConfigurations = new List<AttackConfiguration>();
    [SerializeField] protected PolygonCollider2D hitbox;
    public void Attack()
    {
        hitbox.points = attackConfigurations[0].GetHitbox().points;
    }
}

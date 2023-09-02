using UnityEngine;

[CreateAssetMenu(fileName = "AttackConfiguration", menuName = "Attack Configuration/Create Configuration")]
public class AttackConfiguration : ScriptableObject
{
    [SerializeField] private int damage = 1;
    [SerializeField] private PolygonCollider2D hitbox;
    [SerializeField] private Vector2 hitboxOffset;
    [SerializeField] private AnimationVariableId animationVariableName;

    public PolygonCollider2D GetHitbox()
    {
        return hitbox;
    }
    public int GetDamage()
    {
        return damage;
    }
}

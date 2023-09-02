using UnityEngine;

public class PlayerJump : Jump
{
    protected override void Awake()
    {
        base.Awake();
    }
    public void SetAirDashCondition()
    {
        _body.gravityScale = 0f;
        _velocity = new Vector2(_body.velocity.x, 0f);
        _body.velocity = _velocity;
    }
}

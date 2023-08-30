using System;
using UnityEngine;

[RequireComponent(typeof(Controller), typeof(CollisionDataRetriever), typeof(Rigidbody2D))]
public class Move : MonoBehaviour
{
    [SerializeField, Range(0f, 100f)] protected float _maxSpeed = 5.5f;
    [SerializeField, Range(0f, 100f)] protected float _maxAcceleration = 100f;
    [SerializeField, Range(0f, 100f)] protected float _maxAirAcceleration = 100f;

    [SerializeField] protected bool hasKnockback = false;
    [SerializeField] protected float knockbackForce = 2f;

    protected Vector2 _direction, _desiredVelocity, _velocity;
    protected Rigidbody2D _body;

    protected float _maxSpeedChange, _acceleration;
    protected bool _onGround;

    public virtual event Action<AnimationVariableId, float> SetAnimationFloat;
    public virtual event Action<AnimationVariableId, bool> SetAnimationBool;
    private void Awake()
    {
        _body = GetComponent<Rigidbody2D>();
    }
    public virtual void HorizontalInput(float inputDirection)
    {
        _direction.x = inputDirection;
    }
    public virtual void LimitMaxSpeed(float collisionFriction)
    {
        _desiredVelocity = new Vector2(_direction.x, 0f) * Mathf.Max(_maxSpeed - collisionFriction, 0f);

        float moveSpeed = Mathf.Abs(_desiredVelocity.x);
        SetAnimationFloat?.Invoke(AnimationVariableId.MoveSpeed, moveSpeed);
    }
    public virtual void Flip(Transform model)
    {
        if (_direction.x == 0) return;
        Vector3 originalScale = model.localScale;

        if (_direction.x > 0)
        {
            originalScale.x = Mathf.Abs(originalScale.x);
        }
        else if (_direction.x < 0)
        {
            originalScale.x = Mathf.Abs(originalScale.x) * -1f;
        }

        model.localScale = originalScale;
    }
    public virtual void Flip(Transform model, int direction)
    {
        Vector3 originalScale = model.localScale;

        originalScale.x = Mathf.Abs(originalScale.x) * direction;

        model.localScale = originalScale;
    }

    public virtual void MoveHorizontal(bool onGround, float inputDirection)
    {
        _onGround = onGround;
        _velocity = _body.velocity;

        _acceleration = _onGround ? _maxAcceleration : _maxAirAcceleration;
        _maxSpeedChange = _acceleration * Time.deltaTime;
        _velocity.x = Mathf.MoveTowards(_velocity.x, _desiredVelocity.x, _maxSpeedChange);

        _body.velocity = _velocity;

        SetAnimationBool?.Invoke(AnimationVariableId.Fall, _body.velocity.y > 0 ? false : true);
        SetAnimationBool?.Invoke(AnimationVariableId.Grounded, _onGround);
    }
    public Vector2 GetMoveDirection()
    {
        return _direction;
    }
    public virtual void SetKnockback(float time, Vector3 direction)
    {
        if (!hasKnockback) return;
        _velocity = _body.velocity;
        float knockbackDirection = direction.x >= 0 ? -1f : 1f;
        _velocity.x = knockbackForce * knockbackDirection;

        _body.velocity = _velocity;

    }
    private void OnDestroy()
    {
        SetAnimationFloat = null;
        SetAnimationBool = null;
    }
}
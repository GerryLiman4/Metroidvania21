using System;
using UnityEngine;


[RequireComponent(typeof(Controller), typeof(CollisionDataRetriever), typeof(Rigidbody2D))]
public class Jump : MonoBehaviour
{
    [SerializeField, Range(0f, 10f)] protected float _jumpHeight = 4.5f;
    [SerializeField, Range(0, 5)] protected int _maxAirJumps = 1;
    [SerializeField, Range(0f, 5f)] protected float _downwardMovementMultiplier = 5f;
    [SerializeField, Range(0f, 5f)] protected float _upwardMovementMultiplier = 4f;
    [SerializeField, Range(0f, 0.3f)] protected float _coyoteTime = 0.2f;
    [SerializeField, Range(0f, 0.3f)] protected float _jumpBufferTime = 0.2f;

    [SerializeField] protected bool hasKnockback = false;
    [SerializeField] protected float knockbackForce = 1.5f;

    [SerializeField] AnimationCurve knockbackCurve;

    protected Rigidbody2D _body;
    protected Vector2 _velocity;

    protected int _jumpPhase;
    protected float _defaultGravityScale, _jumpSpeed, _coyoteCounter, _jumpBufferCounter;

    protected bool _desiredJump, _onGround, _isJumping, _isJumpReset;

    public virtual event Action<AnimationVariableId, bool> SetAnimationBool;
    // Start is called before the first frame update
    protected virtual void Awake()
    {
        _body = GetComponent<Rigidbody2D>();

        _isJumpReset = true;
        _defaultGravityScale = 1f;
    }
    public void JumpInput(bool isJump)
    {
        _desiredJump = isJump;
    }

    public void JumpMovement(bool jumpInput, bool isOnGround)
    {
        _onGround = isOnGround;
        _velocity = _body.velocity;

        if (_onGround && _body.velocity.y == 0)
        {
            _jumpPhase = 0;
            _coyoteCounter = _coyoteTime;
            _isJumping = false;
        }
        else
        {
            _coyoteCounter -= Time.deltaTime;
        }

        if (_desiredJump && _isJumpReset)
        {
            _isJumpReset = false;
            _desiredJump = false;
            _jumpBufferCounter = _jumpBufferTime;
        }
        else if (_jumpBufferCounter > 0)
        {
            _jumpBufferCounter -= Time.deltaTime;
        }
        else if (!_desiredJump)
        {
            _isJumpReset = true;
        }
        if (_jumpBufferCounter > 0)
        {
            JumpAction();
        }

        DetermineGravity(jumpInput);

        _body.velocity = _velocity;
        if (_body.velocity.y <= 0) SetAnimationBool?.Invoke(AnimationVariableId.Jump, false);
    }
    public void DetermineGravity(bool jumpInput)
    {
        if (jumpInput && _body.velocity.y > 0)
        {
            _body.gravityScale = _upwardMovementMultiplier;
        }
        else if (!jumpInput || _body.velocity.y < 0)
        {
            _body.gravityScale = _downwardMovementMultiplier;
        }
        else if (_body.velocity.y == 0)
        {
            _body.gravityScale = _defaultGravityScale;
        }
    }
    private void JumpAction()
    {
        if (_coyoteCounter > 0f || (_jumpPhase < _maxAirJumps && _isJumping))
        {
            if (_isJumping)
            {
                _jumpPhase += 1;
            }

            _jumpBufferCounter = 0;
            _coyoteCounter = 0;
            _jumpSpeed = Mathf.Sqrt(-2f * Physics2D.gravity.y * _jumpHeight * _upwardMovementMultiplier);
            _isJumping = true;

            SetAnimationBool?.Invoke(AnimationVariableId.Jump, _isJumping);

            if (_velocity.y > 0f)
            {
                _jumpSpeed = Mathf.Max(_jumpSpeed - _velocity.y, 0f);
            }
            else if (_velocity.y < 0f)
            {
                _jumpSpeed += Mathf.Abs(_body.velocity.y);
            }
            _velocity.y += _jumpSpeed;
        }
    }
    public virtual void SetKnockback(float time,Vector3 direction)
    {
        if (!hasKnockback) return;
        _velocity = _body.velocity;
        _velocity.y = knockbackForce * knockbackCurve.Evaluate(time);

        _body.velocity = _velocity;
    }

    protected virtual void OnDestroy()
    {
        SetAnimationBool = null;
    }
}

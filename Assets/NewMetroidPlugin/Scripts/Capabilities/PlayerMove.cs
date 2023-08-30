using System;
using UnityEngine;

[RequireComponent(typeof(Controller), typeof(CollisionDataRetriever), typeof(Rigidbody2D))]
public class PlayerMove : Move
{
    [SerializeField, Range(0f, 100f)] private float maxDashSpeed = 20f;

    public bool inputDash = false;
    public float dashTime;
    public float distanceBetweenImages;
    public int dashMaxCount = 1;
    private float dashTimeLeft;
    private float lastImageXPosition;
    private int dashCount = 0;
    private bool isDashing = false;
    private bool dashCancelFlag = false;
    private float dashDirection = 1;

    private bool onGroundDashFlag = false;

    public override event Action<AnimationVariableId, float> SetAnimationFloat;
    public override event Action<AnimationVariableId, bool> SetAnimationBool;

    public event Action SpawnAfterImage;
    public event Action<StateId> ChangeState;
    private void Awake()
    {
        _body = GetComponent<Rigidbody2D>();
    }

    public override void HorizontalInput(float inputDirection)
    {
        if (inputDirection == 0 && isDashing)
        {
            _direction.x = transform.localScale.x < 0 ? -1f : 1f;
        }
        else
        {
            _direction.x = inputDirection;
        }
    }
    public override void LimitMaxSpeed(float collisionFriction)
    {
        if (isDashing)
        {
            _desiredVelocity = new Vector2(_direction.x, 0f) * Mathf.Max(maxDashSpeed - collisionFriction, 0f);
        }
        else
        {
            _desiredVelocity = new Vector2(_direction.x, 0f) * Mathf.Max(_maxSpeed - collisionFriction, 0f);
        }
        float moveSpeed = Mathf.Abs(_desiredVelocity.x);
        SetAnimationFloat?.Invoke(AnimationVariableId.MoveSpeed, moveSpeed);
    }
    public override void Flip(Transform model)
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

    public override void MoveHorizontal(bool onGround, float inputDirection)
    {
        bool tempOnGround = _onGround;
        _onGround = onGround;
        _velocity = _body.velocity;

        _acceleration = _onGround ? _maxAcceleration : _maxAirAcceleration;
        _maxSpeedChange = _acceleration * Time.deltaTime;

        CheckDashInterruption();
        // dash logic
        if (isDashing)
        {
            if (dashTimeLeft > 0)
            {
                _velocity.x = _desiredVelocity.x;

                if (Mathf.Abs(transform.position.x - lastImageXPosition) > distanceBetweenImages)
                {
                    SpawnAfterImage?.Invoke();
                    lastImageXPosition = transform.position.x;
                }
                if (!onGroundDashFlag || (onGroundDashFlag == onGround))
                {
                    if (tempOnGround == false && onGround)
                    {
                        dashTimeLeft = 0;
                        StopMove();
                    }
                    else
                    {
                        dashTimeLeft -= Time.deltaTime;
                    }
                }
                if (onGroundDashFlag && !onGround && inputDirection == 0)
                {
                    StopMove();
                }
            }

            if (dashTimeLeft <= 0)
            {
                isDashing = false;
                if (onGround) dashCount = 0;
                ChangeState?.Invoke(StateId.Idle);
            }
        }
        else
        {
            if (onGround) dashCount = 0;
            _velocity.x = Mathf.MoveTowards(_velocity.x, _desiredVelocity.x, _maxSpeedChange);
        }

        _body.velocity = _velocity;
        SetAnimationBool?.Invoke(AnimationVariableId.Fall, _body.velocity.y > 0 ? false : true);
        SetAnimationBool?.Invoke(AnimationVariableId.Grounded, _onGround);
    }

    private void CheckDashInterruption()
    {
        if (!isDashing) return;

        if (dashDirection != _direction.x && _onGround)
        {
            StopMove();
            isDashing = false;
            dashTimeLeft = 0f;
            dashCount = 0;

            ChangeState?.Invoke(StateId.Idle);
        }

    }

    public void DashInput(bool inputDash)
    {
        dashCancelFlag = this.inputDash == inputDash ? false : true;
        this.inputDash = inputDash;

        // triggered once 
        if (((inputDash && !isDashing) && (inputDash && dashCount < dashMaxCount)) && dashCancelFlag)
        {
            onGroundDashFlag = _onGround;
            dashDirection = transform.localScale.x < 0f ? -1f : 1f;
            _direction.x = dashDirection;

            SpawnAfterImage?.Invoke();
            lastImageXPosition = transform.position.x;

            isDashing = true;
            dashTimeLeft = dashTime;
            dashCount++;

            ChangeState?.Invoke(onGroundDashFlag ? StateId.Dash : StateId.AirDash);
        }

    }
    public void StopMove()
    {
        _velocity = new Vector2(0f, _body.velocity.y);
        _body.velocity = _velocity;
    }

    private void OnDestroy()
    {
        SetAnimationFloat = null;
        SetAnimationBool = null;
        SpawnAfterImage = null;
        ChangeState = null;
    }
}


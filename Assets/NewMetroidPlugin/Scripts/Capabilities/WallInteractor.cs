using System;
using UnityEngine;

[RequireComponent(typeof(Controller), typeof(CollisionDataRetriever), typeof(Rigidbody2D))]
public class WallInteractor : MonoBehaviour
{
    public bool WallJumping { get; private set; }

    [Header("Wall Slide")]
    [SerializeField] [Range(0.1f, 5f)] private float _wallSlideMaxSpeed = 5f;
    [Header("Wall Jump")]
    [SerializeField] private Vector2 _wallJumpClimb = new Vector2(12f, 15f);
    [SerializeField] private Vector2 _wallJumpBounce = new Vector2(10.7f, 15f);
    [SerializeField] private Vector2 _wallJumpLeap = new Vector2(14f, 15f);
    [Header("Wall Stick")]
    [SerializeField, Range(0.05f, 0.5f)] private float _wallStickTime = 0.25f;

    private CollisionDataRetriever _collisionDataRetriever;
    private Rigidbody2D _body;

    private Vector2 _velocity;
    private bool _onWall, _onGround, _desiredJump, _isJumpReset;
    private float _wallDirectionX, _wallStickCounter;

    public virtual event Action<AnimationVariableId, bool> SetAnimationBool;
    // Start is called before the first frame update
    void Start()
    {
        _collisionDataRetriever = GetComponent<CollisionDataRetriever>();
        _body = GetComponent<Rigidbody2D>();

        _isJumpReset = true;
    }
    public void JumpInput(bool jumpInput)
    {
        _desiredJump = jumpInput;
    }

    public void WallMovement(float moveInput)
    {
        _velocity = _body.velocity;
        _onWall = _collisionDataRetriever.OnWall;
        _onGround = _collisionDataRetriever.OnGround;
        _wallDirectionX = _collisionDataRetriever.ContactNormal.x;

        #region Wall Slide
        if (_onWall)
        {
            if (_velocity.y < -_wallSlideMaxSpeed)
            {
                _velocity.y = -_wallSlideMaxSpeed;
            }
        }
        #endregion

        #region Wall Stick
        if (_collisionDataRetriever.OnWall && !_collisionDataRetriever.OnGround && !WallJumping)
        {
            if (_wallStickCounter > 0)
            {
                _velocity.x = 0;

                if (moveInput != 0 &&
                    Mathf.Sign(moveInput) == Mathf.Sign(_collisionDataRetriever.ContactNormal.x))
                {
                    _wallStickCounter -= Time.deltaTime;
                }
                else
                {
                    _wallStickCounter = _wallStickTime;
                }
            }
            else
            {
                _wallStickCounter = _wallStickTime;
            }
        }
        #endregion

        SetAnimationBool?.Invoke(AnimationVariableId.OnWall, _onWall);

        #region Wall Jump

        if ((_onWall && _velocity.x == 0) || _onGround)
        {
            WallJumping = false;
        }


        if (_onWall && !_onGround)
        {
            if (_desiredJump && _isJumpReset)
            {
                if (moveInput == 0)
                {
                    _velocity = new Vector2(_wallJumpBounce.x * _wallDirectionX, _wallJumpBounce.y);
                    WallJumping = true;
                    _desiredJump = false;
                    _isJumpReset = false;
                }
                else if (Mathf.Sign(-_wallDirectionX) == Mathf.Sign(moveInput))
                {
                    _velocity = new Vector2(_wallJumpClimb.x * _wallDirectionX, _wallJumpClimb.y);
                    WallJumping = true;
                    _desiredJump = false;
                    _isJumpReset = false;
                }
                else
                {
                    _velocity = new Vector2(_wallJumpLeap.x * _wallDirectionX, _wallJumpLeap.y);
                    WallJumping = true;
                    _desiredJump = false;
                    _isJumpReset = false;
                }
            }
            else if (!_desiredJump)
            {
                _isJumpReset = true;
            }
        }
        SetAnimationBool?.Invoke(AnimationVariableId.WallJump, WallJumping);
        #endregion

        _body.velocity = _velocity;
    }

    private void OnCollisionEnter2D(Collision2D collision)
    {
        _collisionDataRetriever.EvaluateCollision(collision);
        _isJumpReset = false;

        if (_collisionDataRetriever.OnWall && !_collisionDataRetriever.OnGround && WallJumping)
        {
            _body.velocity = Vector2.zero;
        }
    }

}

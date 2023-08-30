using System;
using UnityEngine;

public class PlayerController : Controller
{
    [SerializeField] public Animator animator;
    [SerializeField] public SpriteRenderer spriteRenderer;
    [SerializeField] public PlayerMove horizontalMovement;
    [SerializeField] public Jump jumpMovement;
    [SerializeField] public CollisionDataRetriever collisionRetriever;
    [SerializeField] public WallInteractor wallMovement;
    [SerializeField] public AfterImageEffectPool afterImageEffect;
    [SerializeField] public PlayerHealth health;
    protected void Awake()
    {
        horizontalMovement.SetAnimationFloat += OnSetAnimation;
        horizontalMovement.SetAnimationBool += OnSetAnimation;
        horizontalMovement.SpawnAfterImage += SpawnAfterImage;
        horizontalMovement.ChangeState += OnChangeState;

        jumpMovement.SetAnimationBool += OnSetAnimation;

        health.ChangeState += OnChangeState;
        health.knockbackTime += OnKnockbackState;
    }

    private void OnKnockbackState(float totalTime,Vector3 direction)
    {
        horizontalMovement.Flip(transform, direction.x > 0 ? 1 : -1);
        horizontalMovement.SetKnockback(totalTime, direction);
        jumpMovement.SetKnockback(totalTime,direction);
    }

    private void OnChangeState(StateId stateId)
    {
        previousState = currentState;
        currentState = stateId;
    }

    private void Start()
    {
        afterImageEffect.Initialize(spriteRenderer, transform);
        health.Initialize();
    }
    private void SpawnAfterImage()
    {
        afterImageEffect.GetFromPool();
    }

    protected void OnSetAnimation(AnimationVariableId animationVariable, float amount)
    {
        animator.SetFloat(animationVariable.ToString(), amount);
    }
    protected void OnSetAnimation(AnimationVariableId animationVariable, bool isTrue)
    {
        animator.SetBool(animationVariable.ToString(), isTrue);
    }

    protected void Update()
    {
        // horizontal movement
        horizontalMovement.HorizontalInput(input.RetrieveMoveInput(this.gameObject));
        horizontalMovement.DashInput(input.RetrieveDashInput(this.gameObject));
        horizontalMovement.LimitMaxSpeed(collisionRetriever.Friction);
        horizontalMovement.Flip(transform);

        // jump movement
        jumpMovement.JumpInput(input.RetrieveJumpInput(this.gameObject));

        // wall movement
        wallMovement.JumpInput(input.RetrieveJumpInput(this.gameObject));
    }
    protected void FixedUpdate()
    {
        switch (currentState)
        {
            case (StateId.AirDash):
                // horizontal movement
                horizontalMovement.MoveHorizontal(collisionRetriever.OnGround, input.RetrieveMoveInput(this.gameObject));

                // jump Movement
                jumpMovement.SetAirDashCondition();
                break;
            case (StateId.Hurt):
                break;
            default:
                // horizontal movement
                horizontalMovement.MoveHorizontal(collisionRetriever.OnGround, input.RetrieveMoveInput(this.gameObject));

                // jump Movement
                jumpMovement.JumpMovement(input.RetrieveJumpInput(this.gameObject), collisionRetriever.OnGround);
                break;
        }
        // wall movement
        wallMovement.WallMovement(input.RetrieveMoveInput(this.gameObject));
    }
    protected void OnDestroy()
    {
        horizontalMovement.SetAnimationFloat -= OnSetAnimation;
        horizontalMovement.SetAnimationBool -= OnSetAnimation;
        horizontalMovement.SpawnAfterImage -= SpawnAfterImage;
        horizontalMovement.ChangeState -= OnChangeState;

        jumpMovement.SetAnimationBool -= OnSetAnimation;

        health.ChangeState -= OnChangeState;
        health.knockbackTime -= OnKnockbackState;
    }
}


using System;
using UnityEngine;

public class PlayerController : Controller
{
    [SerializeField] protected Animator animator;
    [SerializeField] protected SpriteRenderer spriteRenderer;
    [SerializeField] protected PlayerMove horizontalMovement;
    [SerializeField] protected PlayerJump jumpMovement;
    [SerializeField] protected CollisionDataRetriever collisionRetriever;
    [SerializeField] protected WallInteractor wallMovement;
    [SerializeField] protected AfterImageEffectPool afterImageEffect;
    [SerializeField] protected PlayerHealth health;
    [SerializeField] protected AttackManager attackMovement;
    protected void Awake()
    {
        horizontalMovement.SetAnimationFloat += OnSetAnimation;
        horizontalMovement.SetAnimationBool += OnSetAnimation;
        horizontalMovement.SpawnAfterImage += SpawnAfterImage;
        horizontalMovement.ChangeState += OnChangeState;

        jumpMovement.SetAnimationBool += OnSetAnimation;

        health.ChangeState += OnChangeState;
        health.knockbackTime += OnKnockbackState;

        input.Attack += OnAttack;
    }

    private void OnAttack()
    {
        attackMovement.Attack();
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

        input.Attack -= OnAttack;
    }
}


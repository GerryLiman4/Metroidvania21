using UnityEngine;

public class EnemyController : Controller
{
    [SerializeField] public Animator animator;
    [SerializeField] public SpriteRenderer spriteRenderer;
    [SerializeField] public Move horizontalMovement;
    [SerializeField] public Jump jumpMovement;
    [SerializeField] public CollisionDataRetriever collisionRetriever;
    [SerializeField] public WallInteractor wallMovement;
    [SerializeField] public HealthManager health;

    protected void Awake()
    {
        horizontalMovement.SetAnimationFloat += OnSetAnimation;
        horizontalMovement.SetAnimationBool += OnSetAnimation;

        jumpMovement.SetAnimationBool += OnSetAnimation;

        health.ChangeState += OnChangeState;
        health.knockbackTime += OnKnockbackState;
    }
    private void OnKnockbackState(float totalTime, Vector3 direction)
    {
        horizontalMovement.Flip(transform, direction.x > 0 ? 1 : -1);
        horizontalMovement.SetKnockback(totalTime, direction);
        jumpMovement.SetKnockback(totalTime, direction);
    }

    private void OnChangeState(StateId stateId)
    {
        previousState = currentState;
        currentState = stateId;
    }
    private void Start()
    {
        health.Initialize();
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

        jumpMovement.SetAnimationBool -= OnSetAnimation;

        health.ChangeState -= OnChangeState;
        health.knockbackTime -= OnKnockbackState;
    }
}

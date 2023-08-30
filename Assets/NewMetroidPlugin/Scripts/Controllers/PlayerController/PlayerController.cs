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
    protected void Awake()
    {
        horizontalMovement.SetAnimationFloat += OnSetAnimation;
        horizontalMovement.SetAnimationBool += OnSetAnimation;
        horizontalMovement.SpawnAfterImage += SpawnAfterImage;
        horizontalMovement.ChangeState += OnChangeState;

        jumpMovement.SetAnimationBool += OnSetAnimation;
    }
    private void OnChangeState(StateId stateId)
    {
        previousState = currentState;
        currentState = stateId;
    }

    private void Start()
    {
        afterImageEffect.Initialize(spriteRenderer, transform);
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
        // horizontal movement
        horizontalMovement.MoveHorizontal(collisionRetriever.OnGround, input.RetrieveMoveInput(this.gameObject));

        // jump Movement
        switch (currentState)
        {
            case (StateId.AirDash):
                jumpMovement.SetAirDashCondition();
                break;
            default:
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

    }
}


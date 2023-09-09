using UnityEngine;

public class Controller : MonoBehaviour
{
    [SerializeField] public InputController input = null;
    [SerializeField] protected StateId currentState;
    [SerializeField] protected StateId previousState;

    public void ChangeState(StateId nextState)
    {
        previousState = currentState;
        currentState = nextState;
    }
}


using UnityEngine;

public class Controller : MonoBehaviour
{
    [SerializeField] public InputController input = null;
    [SerializeField] protected StateId currentState;
    [SerializeField] protected StateId previousState;
}


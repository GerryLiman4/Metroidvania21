using System;
using UnityEngine;

public abstract class InputController : ScriptableObject
{
    public abstract float RetrieveMoveInput(GameObject gameObject);
    public abstract bool RetrieveJumpInput(GameObject gameObject);
    public abstract bool RetrieveDashInput(GameObject gameObject);
    public abstract bool RetrieveAttackInput(GameObject gameObject);

    public abstract event Action Attack;
}


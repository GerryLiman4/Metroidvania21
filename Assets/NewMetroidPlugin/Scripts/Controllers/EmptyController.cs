using System;
using UnityEngine;

[CreateAssetMenu(fileName = "EmptyController", menuName = "InputController/EmptyController")]
public class EmptyController : InputController
{
    public override event Action Attack;

    public override bool RetrieveAttackInput(GameObject gameObject)
    {
        throw new NotImplementedException();
    }

    public override bool RetrieveDashInput(GameObject gameObject)
    {
        return false;
    }

    public override bool RetrieveJumpInput(GameObject gameObject)
    {
        return false;
    }

    public override float RetrieveMoveInput(GameObject gameObject)
    {
        return 0;
    }
    private void OnDestroy()
    {
        Attack = null;
    }
}


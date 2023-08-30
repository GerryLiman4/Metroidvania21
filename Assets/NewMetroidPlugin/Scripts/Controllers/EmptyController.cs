using UnityEngine;

[CreateAssetMenu(fileName = "EmptyController", menuName = "InputController/EmptyController")]
public class EmptyController : InputController
{
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
}


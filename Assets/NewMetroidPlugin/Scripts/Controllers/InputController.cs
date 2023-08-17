using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace J98214
{
    public abstract class InputController : ScriptableObject
    {
        public abstract float RetrieveMoveInput(GameObject gameObject);
        public abstract bool RetrieveJumpInput(GameObject gameObject);
    }
}

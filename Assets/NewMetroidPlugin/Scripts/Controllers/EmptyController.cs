using J98214;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace J98214
{
    [CreateAssetMenu(fileName = "EmptyController", menuName = "InputController/EmptyController")]
    public class EmptyController : InputController
    {

        public override bool RetrieveJumpInput(GameObject gameObject)
        {
            return false;
        }

        public override float RetrieveMoveInput(GameObject gameObject)
        {
            return 0;
        }
    }
}

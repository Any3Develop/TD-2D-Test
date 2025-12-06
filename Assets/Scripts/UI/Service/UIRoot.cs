using UnityEngine;

namespace UI.Service
{
    public class UIRoot : MonoBehaviour
    {
        [field: SerializeField] public Canvas StaticCanvas {get; private set;}
        [field: SerializeField] public Canvas DynamicCanvas {get; private set;}
    }
}
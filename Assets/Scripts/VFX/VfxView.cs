using Common.Stats;
using UnityEngine;
using Zenject;

namespace VFX
{
    public class VfxView : MonoBehaviour
    {
        [Inject] public StatsCollection Stats { get; private set; }
    }
}
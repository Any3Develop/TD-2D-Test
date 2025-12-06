using DG.Tweening;
using UnityEngine;

namespace VFX
{
    public class AOEBombVfx : VfxView
    {
        [SerializeField] private SpriteRenderer graphic;
        [SerializeField] private float duration = 1f;
        [SerializeField] private Ease easeAnim = Ease.InFlash;
        
        private float Radius = 1f;

        private void Start()
        {
            if (Stats.TryGet(x => x.Name == "Radius", out var radiusStat))
                Radius = radiusStat.TotalValue;
            
            DOTween.Sequence()
                .Insert(0, graphic.DOFade(0f, duration).From(1f).SetEase(easeAnim))
                .Insert(0, graphic.transform.DOScale(Vector3.one * (Radius * 2), duration).SetEase(easeAnim).From(Vector3.zero))
                .SetAutoKill(true)
                .OnKill(() => Destroy(gameObject))
                .Play();
        }
    }
}
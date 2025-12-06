using Common.Pools;
using DG.Tweening;
using UnityEngine;

namespace Enemies
{
    public class EnemyAnimator : MonoBehaviour, ISpwanPoolable
    {
        [SerializeField] private SpriteRenderer graphic;
        [SerializeField] public float dyingTime = 1f;
        [SerializeField] public float rotationSpeed = 50f;
        [SerializeField] private Color dieColor;
        [SerializeField] private Color hitColor;
        
        private Transform _graphicTransform;
        private Sequence _currentAnimation;
        private Tween _moveTween;
        private Color _originalColor;
        private Vector3 _originalRotation;
        private Vector3 _originalScale;

        private void Awake()
        {
            _originalColor = graphic.color;
            _graphicTransform = graphic.transform;
            _originalRotation = _graphicTransform.eulerAngles;
            _originalScale = _graphicTransform.localScale;
        }

        public void Release()
        {
            graphic.color = _originalColor;
            _currentAnimation?.Kill(true);
            _graphicTransform.rotation = Quaternion.Euler(_originalRotation);
        }

        public void Spawn()
        {
            _currentAnimation?.Kill(true);
            _currentAnimation = DOTween.Sequence();
            _currentAnimation.Insert(0, graphic.transform.DOScale(_originalScale, 0.15f).From(Vector3.zero));
            _currentAnimation.Insert(0, graphic.DOColor(_originalColor, 0.15f));
            _currentAnimation.SetAutoKill(true);
            _currentAnimation.OnKill(() => 
            { 
                graphic.color = _originalColor; 
                graphic.transform.localScale = _originalScale; 
            });
            _currentAnimation.Play();
        }
        
        public void Die()
        {
            _currentAnimation?.Kill();
            _currentAnimation = DOTween.Sequence();
            _currentAnimation.Append(graphic.DOColor(dieColor, dyingTime));
            _currentAnimation.SetAutoKill(true);
            _currentAnimation.OnKill(() => graphic.color = dieColor);
            _currentAnimation.Play();
        }

        public void HitReaction()
        {
            _currentAnimation?.Kill();
            _currentAnimation = DOTween.Sequence();
            _currentAnimation.Insert(0, graphic.transform.DOShakePosition(0.1f, 0.15f));
            _currentAnimation.Insert(0, graphic.DOColor(hitColor, 0.05f));
            _currentAnimation.Append(graphic.DOColor(_originalColor, 0.05f));
            _currentAnimation.SetAutoKill(true);
            _currentAnimation.OnKill(() => graphic.transform.localPosition = Vector3.zero);
            _currentAnimation.Play();
        }

        public void Move(float speed)
        {
            var current = _graphicTransform.eulerAngles;
            _graphicTransform.rotation = Quaternion.Euler(current.x, current.y, current.z + (speed * rotationSpeed) * Time.deltaTime);
        }
    }
}
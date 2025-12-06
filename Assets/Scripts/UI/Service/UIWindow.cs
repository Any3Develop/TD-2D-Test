using UnityEngine;

namespace UI.Service
{
    public abstract class UIWindow : MonoBehaviour
    {
        [field: SerializeField] public bool Dynamic { get; private set; }
        public bool IsOpened { get; private set; }

        public virtual void Open()
        {
            IsOpened = true;
            gameObject.SetActive(true);
        }

        public virtual void Close()
        {
            IsOpened = false;
            gameObject.SetActive(false);
        }
    }
}
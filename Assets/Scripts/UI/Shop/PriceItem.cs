using TMPro;
using UnityEngine;
using UnityEngine.UI;

namespace UI.Shop
{
    public class PriceItem : MonoBehaviour
    {
        [field: SerializeField] public TMP_Text AmountText { get; private set; }
        [field: SerializeField] public Image IconImage { get; private set; }
    }
}
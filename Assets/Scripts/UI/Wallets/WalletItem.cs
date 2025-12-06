using TMPro;
using UnityEngine;
using UnityEngine.UI;

namespace UI.Wallets
{
    public class WalletItem : MonoBehaviour
    {
        [field:SerializeField] public TMP_Text AmountText { get; private set; }
        [field:SerializeField] public Image IconImage { get; private set; }
    }
}
using Shop;
using UnityEngine;
using UnityEngine.UI;

namespace UI.Shop
{
    public class ShopItem : MonoBehaviour
    {
        [field: SerializeField] public Button BuyButton { get; private set; }

        [field: SerializeField] public Image IconImage { get; private set; }
        public ProductItem Product { get; set; }
        
        [SerializeField] private PriceItem pricePrototype;
        [SerializeField] private RectTransform priceContainer;

        public void AddPrice(string amount, Sprite icon)
        {
            var priceItem = Instantiate(pricePrototype, priceContainer);
            priceItem.IconImage.sprite = icon;
            priceItem.AmountText.SetText(amount);
        }
    }
}
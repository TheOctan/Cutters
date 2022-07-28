using System;
using System.Threading.Tasks;
using DG.Tweening;
using Project.Scripts.Shop;
using UnityEngine;
using UnityEngine.Serialization;

public class Shop : MonoBehaviour
{
    [SerializeField] private Transform _shopTransform;
    [SerializeField] private PricePolicy _pricePolicy;
    [SerializeField] private float _delayShopping = 0.3f;

    private void OnTriggerEnter(Collider other)
    {
        var inventory = other.GetComponentInChildren<IInventory>();
        if (inventory != null)
        {
            if (!inventory.IsEmpty)
            {
                int totalPrice = inventory.CountItems * +_pricePolicy.PricePerItem;

                AnimateItemsAsync(inventory);
            }
        }
    }

    private async void AnimateItemsAsync(IInventory inventory)
    {
        while (!inventory.IsEmpty)
        {
            Transform item = inventory.GetNextItem();
            item.DOJump(_shopTransform.position, 3, 1, 0.7f)
                .SetEase(Ease.InOutSine);
            await Task.Delay((int)(1000f * _delayShopping));
        }
    }

    private Transform CreateCoin()
    {
        throw new NotImplementedException();
    }
}

using System;
using System.Threading.Tasks;
using DG.Tweening;
using Project.Scripts.Shop;
using UnityEngine;

public class Shop : MonoBehaviour
{
    [SerializeField] private PricePolicy _pricePolicy;
    [Header("Properties")]
    [SerializeField] private Transform _shopTransform;
    [SerializeField] private float _delayShopping = 0.3f;

    private ObjectPool<Coin> _coins;

    private void Awake()
    {
        _coins = new ObjectPool<Coin>(_pricePolicy.MoneyPrefab, 15);
    }

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
            item.DOJump(_shopTransform.position, 1.5f, 1, 0.7f)
                .SetEase(Ease.InOutSine)
                .OnComplete(() => Destroy(item.gameObject));
            await Task.Delay((int)(1000f * _delayShopping));
        }
    }

    private Transform CreateCoin()
    {
        throw new NotImplementedException();
    }
}

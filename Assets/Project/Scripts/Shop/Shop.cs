using System.Threading.Tasks;
using DG.Tweening;
using Project.Scripts.Shop;
using UnityEngine;
using UnityEngine.InputSystem;

public class Shop : MonoBehaviour
{
    [SerializeField] private PricePolicy _pricePolicy;
    [Header("Properties")]
    [SerializeField] private Transform _shopTransform;
    [SerializeField] private RectTransform _uiCoinPosition;

    [Header("Coin")]
    [SerializeField, Min(0.1f)] private float _duration = 0.5f;
    [SerializeField] private float _distance = 5f;

    private ObjectPool<Coin> _coins;
    private Camera _camera;

    private void Awake()
    {
        _coins = new ObjectPool<Coin>(_pricePolicy.MoneyPrefab, 20);
        _camera = Camera.main;
    }

    private void Update()
    {
        if (Keyboard.current.spaceKey.wasPressedThisFrame)
        {
            DropCoin();
        }
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
                .OnComplete(() =>
                {
                    Destroy(item.gameObject);
                    DropCoin();
                });
            await Task.Delay((int)(1000f * _pricePolicy.DelayShopping));
        }
    }

    private async void DropCoin()
    {
        await Task.Delay((int)(1000 * _pricePolicy.DelayCoinDropping));

        Quaternion rotation = _camera.transform.rotation * Quaternion.Euler(90, 0,0);
        Coin coin = _coins.Pull(_shopTransform.position, rotation);
        Vector3 corrected = _uiCoinPosition.position;
        corrected.z = _distance;
        Vector3 endPosition = _camera.ScreenToWorldPoint(corrected);

        coin.transform.DOMove(endPosition, _duration)
            .SetEase(Ease.InSine)
            .OnComplete(() =>
            {
                coin.ReturnToPool();
            });
    }
}

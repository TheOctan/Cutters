using UnityEngine;

namespace Project.Scripts.Shop
{
    [CreateAssetMenu()]
    public class PricePolicy : ScriptableObject
    {
        public float DelayShopping => _delayShopping;
        public float DelayCoinDropping => _delayCoinDropping;
        public int PricePerItem => _pricePerItem;
        public Coin MoneyPrefab => _moneyPrefab;

        [SerializeField] private float _delayShopping = 0.2f;
        [SerializeField] private float _delayCoinDropping = 0.1f;
        [SerializeField] private int _pricePerItem = 15;
        [SerializeField] private Coin _moneyPrefab;
    }
}
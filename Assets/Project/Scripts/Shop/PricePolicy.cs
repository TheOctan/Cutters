using UnityEngine;

namespace Project.Scripts.Shop
{
    [CreateAssetMenu()]
    public class PricePolicy : ScriptableObject
    {
        public int PricePerItem => _pricePerItem;
        public Coin MoneyPrefab => _moneyPrefab;

        [SerializeField] private int _pricePerItem = 15;
        [SerializeField] private Coin _moneyPrefab;
    }
}
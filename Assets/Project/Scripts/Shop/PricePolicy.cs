using UnityEngine;

namespace Project.Scripts.Shop
{
    [CreateAssetMenu()]
    public class PricePolicy : ScriptableObject
    {
        public int PricePerItem => _pricePerItem;
        
        [SerializeField] private int _pricePerItem = 15;
    }
}
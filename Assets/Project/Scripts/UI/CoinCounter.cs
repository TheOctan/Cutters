using System;
using DG.Tweening;
using TMPro;
using UnityEngine;
using UnityEngine.InputSystem;

public class CoinCounter : MonoBehaviour
{
    [SerializeField] private TextMeshProUGUI _coinCountText;
    [SerializeField] private Transform _coinHolder;

    [SerializeField] private float _shakeDuration = 0.1f;
    [SerializeField] private float _shakeStrength = 0.2f;

    private int _currentCoinsCount;

    public void AddCoins(int count)
    {
        Sequence sequence = DOTween.Sequence();

        for (var i = 0; i < count; i++)
        {
            sequence.Append(_coinCountText.transform
                .DOShakeScale(_shakeDuration, _shakeStrength, 1, 0 )
                .OnStart(() =>
                {
                    _currentCoinsCount++;
                    _coinCountText.text = $"x{_currentCoinsCount}";
                }))
                .OnComplete(() => 
                    _coinCountText.transform.localScale = Vector3.one);
        }
    }
}
using DG.Tweening;
using TMPro;
using UnityEngine;

public class CoinCounter : MonoBehaviour
{
    [SerializeField] private TextMeshProUGUI _coinCountText;
    [SerializeField] private Transform _coinHolder;
    [SerializeField] private Transform _coin;
    [SerializeField] private Camera _uiCamera;

    [Header("Properties")]
    [SerializeField] private float _shakeDuration = 0.1f;
    [SerializeField] private float _shakeStrength = 0.2f;

    private int _currentCoinsCount;

    private void Awake()
    {
        // if (!ReferenceEquals(_uiCamera, null))
        // {
        //     Vector3 local = _uiCamera.ScreenToWorldPoint(_coinHolder.position);
        //     _coin.position = new Vector3(local.x, local.y, _coin.transform.position.z);
        // }
    }

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
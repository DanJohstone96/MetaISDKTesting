using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using UnityEngine.UI;
using UnityEngine.SocialPlatforms.Impl;

public class ScoreBoardController : MonoBehaviour
{
    [SerializeField]
    private TextMeshProUGUI _ScoreText;

    [SerializeField]
    private GameObject _Popup;

    private float _Score;

    // Start is called before the first frame update
    void Start()
    {
        Reset();
        SetPopupVisibility(false);
    }

    public void AddScore(float newScore)
    {
        _Score += newScore;
        _ScoreText.text = _Score.ToString();
    }

    public void SetPopupVisibility(bool visible) 
    {
        _Popup.SetActive(visible);
    }

    public void Reset()
    {
        _Score = 0;
        _ScoreText.text = _Score.ToString();
    }
}

﻿using UnityEngine;
using UnityEngine.UI;

[RequireComponent(typeof(Button))]
public class ButtonHome : MonoBehaviour
{
    Button _button;
    void Awake()
    {
        _button = GetComponent<Button>();
        _button.onClick.AddListener(() => GameManager.OnGameRestart?.Invoke());
    }
}

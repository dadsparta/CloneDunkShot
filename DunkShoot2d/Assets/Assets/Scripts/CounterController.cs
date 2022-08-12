using System;
using System.Collections;
using System.Collections.Generic;
using Assets.Scripts;
using TMPro;
using UnityEngine;

public class CounterController : MonoBehaviour
{
    private TMP_Text _text;

    private void Start()
    {
        _text = GetComponent<TMP_Text>();
    }

    public void ChangeCounter()
    {
        _text.text = ScoreDataBase.Score.ToString();
    }
}

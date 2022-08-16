using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class StarController : MonoBehaviour
{
    [SerializeField] private TMP_Text _starCounterController;

    private void OnTriggerEnter2D(Collider2D col)
    {
        if (col.gameObject.CompareTag("Ball"))
        {
            gameObject.SetActive(false);
            ScoreStateDatabase.StarCount++;
            UpdateTextOfCounter();
        }
    }

    private void UpdateTextOfCounter()
    {
        _starCounterController.text = ScoreStateDatabase.StarCount.ToString();
    }
}

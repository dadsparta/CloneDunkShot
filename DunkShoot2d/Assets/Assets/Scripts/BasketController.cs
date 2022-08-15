using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BasketController : MonoBehaviour
{

    [SerializeField] private Transform basketTransform;

    private void OnTriggerEnter2D(Collider2D collision)
    {
        GameManager.instance.UpdateBasket();
    }

    public void UpdateBasket(Vector2 pos)
    {
        basketTransform.gameObject.SetActive(true);
        basketTransform.position = pos;
    }

    public void DisableBasket() {

        basketTransform.gameObject.SetActive(false);
    }

}

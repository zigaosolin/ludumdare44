using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ShopTrigger : MonoBehaviour
{
    bool hasBeedTrigerred = false;

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (hasBeedTrigerred)
            return;

        hasBeedTrigerred = true;
        FindObjectOfType<Game>().TriggerShop();
    }
}

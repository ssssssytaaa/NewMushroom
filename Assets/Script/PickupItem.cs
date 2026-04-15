using UnityEngine;

public class PickupItem : MonoBehaviour
{
    // ¶Ô×¼¸ßÁÁ
    void OnMouseOver()
    {
        GetComponent<Renderer>().material.color = Color.yellow;
    }

    void OnMouseExit()
    {
        GetComponent<Renderer>().material.color = Color.white;
    }
}
using UnityEngine;

public class ResourcePoint : MonoBehaviour
{
    [Header("资源设置")]
    public int maxHp = 3;
    public GameObject dropItemPrefab; // 拖入你之前的可拾取道具预制体
    public float dropForce = 2f;

    private int currentHp;

    void Start()
    {
        currentHp = maxHp;
    }

    // 被攻击扣血
    public void TakeDamage()
    {
        currentHp--;

        if (currentHp <= 0)
        {
            DropResource();
            Destroy(gameObject); // 资源点消失
        }
    }

    // 掉落道具
    void DropResource()
    {
        // 1. 往上抬 0.8 米生成，绝对不会卡地里
        Vector3 spawnPos = transform.position + Vector3.up * 0.5f;
        GameObject item = Instantiate(dropItemPrefab, spawnPos, Quaternion.identity);

        Rigidbody rb = item.GetComponent<Rigidbody>();
        if (rb != null)
        {
            // 2. 重置速度，避免继承奇怪速度
            rb.velocity = Vector3.zero;
            rb.angularVelocity = Vector3.zero;
            // 3. 轻微向上弹，更自然
            rb.AddForce(Vector3.up * 3f, ForceMode.Impulse);
        }
    }
}
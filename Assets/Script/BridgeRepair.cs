using System.Collections.Generic;
using UnityEngine;

public class BridgeRepair : MonoBehaviour
{
    [Header("桥梁设置")]
    public int requiredMaterials = 5;
    public Transform[] bridgeSlots; // 填入5个桥板位置

    [Header("桥板预制体")]
    public GameObject woodPlatePrefab;
    public GameObject cactusPlatePrefab;

    private int currentCount = 0;
    private bool isCompleted = false;

    // 检测丢进来的物品
    private void OnTriggerEnter(Collider other)
    {
        if (isCompleted) return;

        ItemType item = other.GetComponent<ItemType>();
        if (item == null) return;

        // 丢进来的是材料
        AddMaterial(item.type);

        // 销毁投进来的物品
        Destroy(other.gameObject);
    }

    void AddMaterial(MaterialType type)
    {
        if (currentCount >= requiredMaterials) return;

        // 生成对应桥板
        SpawnBridgePlate(type, currentCount);

        currentCount++;

        // 满5个完成修复
        if (currentCount >= requiredMaterials)
        {
            CompleteBridge();
        }
    }

    void SpawnBridgePlate(MaterialType type, int slotIndex)
    {
        GameObject prefab = type == MaterialType.WoodPlank ? woodPlatePrefab : cactusPlatePrefab;

        Transform slot = bridgeSlots[slotIndex];
        GameObject plate = Instantiate(prefab, slot.position, slot.rotation);

        // 让桥板作为桥的子物体
        plate.transform.SetParent(transform);
    }

    void CompleteBridge()
    {
        isCompleted = true;
        GetComponent<Collider>().enabled = false; // 关闭交互区域
        Debug.Log("桥梁已修复！");
    }
}
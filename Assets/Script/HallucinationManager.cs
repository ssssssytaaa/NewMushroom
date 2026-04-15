using UnityEngine;
using System.Collections.Generic;

public class HallucinationManager : MonoBehaviour
{
    [Header("拖入两个世界的根节点")]
    public GameObject realWorldRoot;    // 真实世界父物体
    public GameObject halluWorldRoot;   // 幻觉世界父物体

    [Header("冲击波效果")]
    public float expandSpeed = 30f;
    public float maxRadius = 50f;
    public Transform waveEffect;

    private float currentRadius = 0f;
    private bool isTriggered = false;

    // 所有物体（全层级）
    private List<GameObject> allRealObjects = new List<GameObject>();
    private List<GameObject> allHalluObjects = new List<GameObject>();

    void Start()
    {
        // 递归收集所有子物体（无论嵌套多深）
        CollectAllChildren(realWorldRoot, allRealObjects);
        CollectAllChildren(halluWorldRoot, allHalluObjects);

        // 初始化状态
        foreach (var obj in allRealObjects) obj.SetActive(true);
        foreach (var obj in allHalluObjects) obj.SetActive(false);

        Debug.Log("找到真实物体数量：" + allRealObjects.Count);
        Debug.Log("找到幻觉物体数量：" + allHalluObjects.Count);
    }

    void Update()
    {
        if (Input.GetKeyDown(KeyCode.R))
        {
            isTriggered = true;
        }

        if (isTriggered && currentRadius < maxRadius)
        {
            currentRadius += expandSpeed * Time.deltaTime;

            // 冲击波光圈
            if (waveEffect != null)
                waveEffect.localScale = Vector3.one * currentRadius * 2;

            // 切换显示隐藏
            SwitchObjectsByDistance(allRealObjects, false);
            SwitchObjectsByDistance(allHalluObjects, true);
        }
    }

    // 核心：递归收集所有层级的子物体（真正解决漏物体问题）
    void CollectAllChildren(GameObject parent, List<GameObject> list)
    {
        foreach (Transform child in parent.transform)
        {
            list.Add(child.gameObject);
            CollectAllChildren(child.gameObject, list); // 继续找子物体的子物体
        }
    }

    // 根据距离设置显隐
    void SwitchObjectsByDistance(List<GameObject> objects, bool enable)
    {
        foreach (var obj in objects)
        {
            if (obj == null) continue;

            float dist = Vector3.Distance(transform.position, obj.transform.position);
            if (dist <= currentRadius)
            {
                obj.SetActive(enable);
            }
        }
    }
}
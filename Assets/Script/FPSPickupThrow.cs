using UnityEngine;

public class FPSPickupThrow : MonoBehaviour
{
    [Header("基础设置")]
    public Camera playerCamera;
    public Transform holdPoint;
    public float pickUpRange = 3f;
    public float throwForce = 18f;

    [Header("瞄准设置")]
    public float aimFOV = 45f;
    private float defaultFOV;
    private bool isAiming = false;

    private GameObject currentItem;
    private bool hasItem;

    void Start()
    {
        defaultFOV = playerCamera.fieldOfView;
    }

    void Update()
    {
        // 拾取 E
        if (Input.GetKeyDown(KeyCode.E) && !hasItem)
            TryPickup();

        // 右键瞄准
        if (Input.GetMouseButtonDown(1)) Aim();
        if (Input.GetMouseButtonUp(1)) CancelAim();

        // 瞄准状态下左键投掷
        if (Input.GetMouseButtonDown(0) && hasItem && isAiming)
            ThrowItem();

        // 新增：空手时左键攻击资源点
        if (Input.GetMouseButtonDown(0) && !hasItem)
            TryAttackResource();
    }

    // 攻击资源点
    public float attackRange = 3f;
    void TryAttackResource()
    {
        Ray ray = playerCamera.ScreenPointToRay(Input.mousePosition);
        if (Physics.Raycast(ray, out RaycastHit hit, attackRange))
        {
            // 尝试攻击资源点
            ResourcePoint rp = hit.collider.GetComponent<ResourcePoint>();
            if (rp != null)
            {
                rp.TakeDamage();
            }
        }
    }

    void TryPickup()
    {
        Ray ray = playerCamera.ScreenPointToRay(Input.mousePosition);
        if (Physics.Raycast(ray, out RaycastHit hit, pickUpRange))
        {
            if (hit.collider.CompareTag("Pickup"))
            {
                currentItem = hit.collider.gameObject;
                hasItem = true;

                Rigidbody rb = currentItem.GetComponent<Rigidbody>();
                rb.isKinematic = true;

                // 关键：拿起时关闭碰撞，避免推挤玩家
                currentItem.GetComponent<Collider>().enabled = false;

                currentItem.transform.SetParent(holdPoint);
                currentItem.transform.localPosition = Vector3.zero;
                currentItem.transform.localRotation = Quaternion.identity;
            }
        }
    }

    void Aim()
    {
        isAiming = true;
        playerCamera.fieldOfView = aimFOV;
    }

    void CancelAim()
    {
        isAiming = false;
        playerCamera.fieldOfView = defaultFOV;
    }

    void ThrowItem()
    {
        currentItem.transform.SetParent(null);

        Rigidbody rb = currentItem.GetComponent<Rigidbody>();
        rb.isKinematic = false;
        rb.velocity = Vector3.zero;
        rb.useGravity = true;

        // 关键：扔出去后重新开碰撞
        currentItem.GetComponent<Collider>().enabled = true;

        // 可选：加一点向上角度，更真实
        Vector3 throwDir = playerCamera.transform.forward + playerCamera.transform.up * 0.15f;
        rb.AddForce(throwDir.normalized * throwForce, ForceMode.Impulse);

        hasItem = false;
        currentItem = null;
    }
}
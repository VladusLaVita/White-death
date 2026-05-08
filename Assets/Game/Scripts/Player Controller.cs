using TMPro;
using UnityEngine;

[RequireComponent(typeof(CharacterController), typeof(Inventory))]
public class PlayerController : MonoBehaviour
{
    [Header("Movement")]
    public float maxSpeed = 5f;
    public float midSpeed = 2f;
    public float jumpHeight = 2f;
    public float gravityValue = -9.81f;

    [Header("Interaction")]
    public bool deadFlag = false;
    public float interactDistance = 3f;
    public LayerMask interactableLayer;
    public GameObject hint;
    public TextMeshProUGUI itemLabel;

    [Header("Weapon Visuals")]
    public Transform weaponHolder; // Объект-рука игрока
    public float bulletForce = 10f;
    private Transform _activeMuzzle;
    private ItemData _currentModelItem;

    [Header("References")]
    public Animator animator;
    public Transform cameraTransform;
    public AudioSource audioSource;
    public AudioClip inventorySound;

    private CharacterController _cc;
    private Inventory _inventory;
    private Vector3 _velocity;
    private Interactable _currentInteractable;
    private ItemData lastInventoryItem;

    private static readonly int SpeedHash = Animator.StringToHash("Speed");
    private static readonly int SprintHash = Animator.StringToHash("Sprint");
    private static readonly int JumpHash = Animator.StringToHash("Jump");

    private float _lastInteractTime;
    private const float InteractCooldown = 0.5f;

    void Start()
    {

        _cc = GetComponent<CharacterController>();
        _inventory = GetComponent<Inventory>();
        if (cameraTransform == null) cameraTransform = Camera.main.transform;

        // Подписываемся на смену предмета, чтобы обновлять визуальную модель оружия
        _inventory.onInventoryChanged += UpdateWeaponModel;

        ToggleHint(false);

        UpdateWeaponModel();
    }

    void Update()
    {
        HandleGravity();
        HandleMovement();
        HandleJump();
        CheckInteract();
        CheckUse();
        HandleScroll();
    }

    private void UpdateWeaponModel()
    {
        ItemData active = _inventory.GetActiveItem();

        // 2. ГЛАВНОЕ: Если этот предмет уже отображается — ничего не делаем
        if (active == _currentModelItem) return;

        _currentModelItem = active;

        // Очищаем руку
        foreach (Transform child in weaponHolder) Destroy(child.gameObject);
        _activeMuzzle = null;

        if (active != null && active.weaponModelPrefab != null)
        {
            // Создаем модель в руке
            GameObject model = Instantiate(active.weaponModelPrefab, weaponHolder);

            // 1. Сбрасываем позицию и вращение в локальные 0 (чтобы легло точно в руку)
            model.transform.localPosition = Vector3.zero;
            model.transform.localRotation = Quaternion.identity;

            // 2. ОТКЛЮЧАЕМ ГРАВИТАЦИЮ И ФИЗИКУ (чтобы не упало из рук)

            SetLayerRecursively(model, LayerMask.NameToLayer("Default"));
            if (model.TryGetComponent(out Rigidbody rb))
            {
                rb.isKinematic = true; // Выключает влияние гравитации и сил
                rb.useGravity = false;
            }

            // 3. Отключаем коллайдеры (чтобы оружие не толкало игрока)
            if (model.TryGetComponent(out Collider col))
            {
                col.enabled = false;
            }

            // Ищем точку вылета пули
            _activeMuzzle = model.transform.Find("Muzzle");
        }
    }

    private void HandleScroll()
    {
        if (_inventory == null || _inventory.items.Count == 0) return;

        float scroll = Input.GetAxisRaw("Mouse ScrollWheel");
        if (scroll != 0)
        {
            int direction = (int)Mathf.Sign(scroll);
            _inventory.selectedSlotIndex -= direction;

            if (_inventory.selectedSlotIndex < 0)
                _inventory.selectedSlotIndex = _inventory.items.Count - 1;
            else if (_inventory.selectedSlotIndex >= _inventory.items.Count)
                _inventory.selectedSlotIndex = 0;

            _inventory.onInventoryChanged?.Invoke();
            audioSource.PlayOneShot(inventorySound);

            lastInventoryItem = _inventory.items[_inventory.selectedSlotIndex].data;
            animator.SetBool("Armed", lastInventoryItem.isRanged);
        }
    }

    private void CheckUse()
    {
        if (!Input.GetKeyDown(KeyCode.Mouse0)) return;

        if (_currentInteractable != null)
        {
            _currentInteractable.Use();
            return;
        }

        ItemData activeItem = _inventory.GetActiveItem();
        if (activeItem != null) UseItemFromInventory(activeItem);
    }

    private void UseItemFromInventory(ItemData itemData)
    {
        if (itemData.isEdible) EatItem(itemData);
        else if (itemData.isWeapon)
        {
            if (itemData.isRanged) Shoot(itemData);
            else MeleeAttack(itemData);
        }
    }

    private void Shoot(ItemData data)
    {
        // Поиск патронов в инвентаре
        InventoryItem ammoSlot = _inventory.items.Find(x => x.data == data.ammo);

        if (ammoSlot != null && ammoSlot.count > 0)
        {
            _inventory.RemoveItem(data.ammo, 1);

            animator.SetTrigger("Shoot");
            audioSource.PlayOneShot(data.useSound);

            SpawnPhysicalBullet(data);
        }
        else
        {
            Debug.Log("Нет патронов!");
        }
    }

    private void SpawnPhysicalBullet(ItemData itemData)
    {
        if (itemData.ammoPrefab == null || _activeMuzzle == null) return;

        Ray ray = new Ray(cameraTransform.position, cameraTransform.forward);
        Vector3 targetPoint = Physics.Raycast(ray, out RaycastHit hit, 200f) ? hit.point : ray.GetPoint(200f);
        Vector3 direction = (targetPoint - _activeMuzzle.position).normalized;

        // 1. Создаем объект
        GameObject bullet = Instantiate(itemData.ammoPrefab, _activeMuzzle.position, Quaternion.LookRotation(direction));

        // 2. КОРРЕКЦИЯ ПОВОРОТА
        // Если стрела в префабе стоит вертикально (наконечник по Y), 
        // поворачиваем её на 90 градусов по X, чтобы она легла вдоль вектора полёта.
        bullet.transform.Rotate(90, 0, 0);

        // 3. ПРИЛОЖЕНИЕ СИЛЫ
        if (bullet.TryGetComponent(out Rigidbody rb))
        {
            // Обязательно сбрасываем скорость, если префаб странно себя ведет
            rb.velocity = Vector3.zero;
            rb.angularVelocity = Vector3.zero;

            // Толкаем в сторону прицела (direction)
            rb.AddForce(direction * bulletForce, ForceMode.Impulse);
        }
    }


    private void EatItem(ItemData itemData)
    {
        animator.SetTrigger("Eat");
        audioSource.PlayOneShot(itemData.useSound);
        _inventory.RemoveItem(itemData, 1);
    }

    private void MeleeAttack(ItemData itemData)
    {
        animator.SetTrigger("Strike");
        audioSource.PlayOneShot(itemData.useSound);
    }

    // --- Остальные стандартные методы (Movement, Gravity, Interact) остаются без изменений ---
    private void HandleMovement()
    {
        float inputX = Input.GetAxis("Horizontal");
        float inputZ = Input.GetAxis("Vertical");
        bool isGrounded = _cc.isGrounded;
        bool isSprinting = Input.GetKey(KeyCode.LeftShift) && isGrounded;
        float currentSpeed = isSprinting ? maxSpeed : midSpeed;
        Vector3 move = transform.right * inputX + transform.forward * inputZ;
        if (move.sqrMagnitude > 1f) move.Normalize();
        _cc.Move(move * (currentSpeed * Time.deltaTime) + _velocity * Time.deltaTime);
        animator.SetBool(SprintHash, isSprinting);
        animator.SetFloat(SpeedHash, move.magnitude * (currentSpeed / maxSpeed));
    }

    private void HandleJump()
    {
        if (_cc.isGrounded && Input.GetButtonDown("Jump"))
        {
            animator.SetTrigger(JumpHash);
            _velocity.y = Mathf.Sqrt(jumpHeight * -2f * gravityValue);
        }
    }

    private void HandleGravity()
    {
        if (_cc.isGrounded && _velocity.y < 0) _velocity.y = -2f;
        _velocity.y += gravityValue * Time.deltaTime;
    }

    private void CheckInteract()
    {
        Ray ray = new Ray(cameraTransform.position, cameraTransform.forward);
        if (Physics.Raycast(ray, out RaycastHit hit, interactDistance, interactableLayer))
        {
            if (hit.collider.TryGetComponent(out Interactable interactable))
            {
                if (_currentInteractable != interactable)
                {
                    _currentInteractable = interactable;
                    UpdateUI(interactable.interactionName);
                }
                if (Input.GetKeyDown(KeyCode.E))
                {
                    _currentInteractable.Interact();
                    if (_currentInteractable.TryGetComponent(out Item item))
                        audioSource.PlayOneShot(item.data.pickUpSound);
                }
                return;
            }
        }
        if (Time.time - _lastInteractTime > InteractCooldown) ClearCurrentItem();
    }

    private void UpdateUI(string itemName)
    {
        _lastInteractTime = Time.time;
        if (itemLabel != null) itemLabel.text = itemName;
        ToggleHint(true);
    }
    private void SetLayerRecursively(GameObject obj, int newLayer)
    {
        obj.layer = newLayer;
        foreach (Transform child in obj.transform)
        {
            SetLayerRecursively(child.gameObject, newLayer);
        }
    }
    private void ClearCurrentItem() { _currentInteractable = null; ToggleHint(false); }
    private void ToggleHint(bool state) { if (hint != null && hint.activeSelf != state) hint.SetActive(state); }
}

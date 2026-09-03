using UnityEngine;

public class WeaponShoot : MonoBehaviour
{
    [Header("References")]
    public Camera playerCamera;
    public Animator animator;
    public GameObject crosshair;

    [Header("Shooting")]
    public float fireRate = 0.5f;
    public float range = 20f;

    [Header("Ammo")]
    public int magazineSize = 10;
    public float reloadTime = 2f;

    [Header("Weapon Stats")]
    public int damage = 25;

    private int currentAmmo;
    private float nextFireTime;
    private bool isReloading;

    private UIManager uiManager;

    void Start()
    {
        currentAmmo = magazineSize;
        uiManager = FindFirstObjectByType<UIManager>();
        uiManager.UpdateAmmo(currentAmmo, magazineSize);
        crosshair.SetActive(false);

    }

    void Update()
    {
        HandleAim();
        HandleShoot();
        HandleReload();
    }

    public void IncreaseDamage(int amount)
    {
        damage += amount;
    }

    public void IncreaseAttackRange(float amount)
    {
        range += amount;
    }

    public void IncreaseReloadSpeed(float amount)
    {
        reloadTime -= amount;

        if (reloadTime < 1.2f)
            reloadTime = 1.2f;
    }

    public void IncreaseFireRate(float amount)
    {
        fireRate -= amount;

        if (fireRate < 0.1f)
            fireRate = 0.1f;
    }

    public void IncreaseMagazineSize(int amount)
    {
        magazineSize += amount;
        currentAmmo += amount;

        uiManager.UpdateAmmo(currentAmmo, magazineSize);
    }

    void HandleAim()
    {
        bool aiming = Input.GetMouseButton(1);

        animator.SetBool("IsAiming", aiming);

        crosshair.SetActive(aiming);
    }

    void HandleShoot()
    {
        if (isReloading)
            return;

        if (!Input.GetMouseButton(1))
            return;

        if (currentAmmo <= 0)
        {
            return;
        }

        if (Input.GetMouseButtonDown(0) && Time.time >= nextFireTime)
        {
            nextFireTime = Time.time + fireRate;

            Shoot();
        }
    }

    void Shoot()
    {
        currentAmmo--;

        GameMetricsManager.Instance.RegisterShot();

        Debug.Log("Bang! Ammo: " + currentAmmo);

        animator.SetTrigger("Shoot");

        uiManager.UpdateAmmo(currentAmmo, magazineSize);

        Ray ray = playerCamera.ViewportPointToRay(new Vector3(0.5f, 0.5f, 0));

        RaycastHit hit;

        if (Physics.Raycast(ray, out hit, range))
        {
            Debug.Log("Hit: " + hit.collider.name);

            EnemyHealth enemy = hit.collider.GetComponent<EnemyHealth>();

            if(enemy != null)
            {
                enemy.TakeDamage(damage);

                GameMetricsManager.Instance.RegisterHit();
            }
        }
    }

    void HandleReload()
    {
        if (isReloading)
            return;

        if (Input.GetKeyDown(KeyCode.R))
        {
            StartCoroutine(Reload());
        }
    }

    System.Collections.IEnumerator Reload()
    {
        isReloading = true;

        animator.SetTrigger("Reload");

        Debug.Log("Reloading...");

        yield return new WaitForSeconds(reloadTime);

        currentAmmo = magazineSize;

        uiManager.UpdateAmmo(currentAmmo, magazineSize);

        isReloading = false;

        GameMetricsManager.Instance.RegisterReload();

        Debug.Log("Reload Complete");
    }
}

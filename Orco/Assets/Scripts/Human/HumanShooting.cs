using UnityEngine;

public class HumanShooting : MonoBehaviour
{
    [SerializeField] private LayerMask playerLayer;
    [SerializeField] private GameObject[] bulletPrefab;

    [SerializeField] private float timeBtwShoots = 1f;

    private float shootingTime = 0;
    private bool isPlayerNear = false;
    private int whichWeapon = 0;

    private HumanMovement humanMovement;
    
    void Start()
    {
        humanMovement = GetComponent<HumanMovement>();

        shootingTime = 0;
        isPlayerNear = false;

        whichWeapon = Random.Range(0, bulletPrefab.Length);
    }
    
    void Update()
    {       
        if (!isPlayerNear || humanMovement.isGrabbed)
        {
            return;
        }


        if (shootingTime >= timeBtwShoots)
        {            
            Instantiate(bulletPrefab[whichWeapon], transform.position, Quaternion.identity);
            shootingTime = 0;
        } else
        {
            shootingTime += Time.deltaTime;
        }
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (TriggerUtils.CheckLayerMask(playerLayer, collision.gameObject))
        {
            isPlayerNear = true;
        }
    }

    private void OnTriggerExit2D(Collider2D collision)
    {
        if (TriggerUtils.CheckLayerMask(playerLayer, collision.gameObject))
        {
            isPlayerNear = false;
        }
    }
}

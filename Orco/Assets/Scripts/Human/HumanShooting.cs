using UnityEngine;

public class HumanShooting : MonoBehaviour
{
    [SerializeField] private LayerMask playerLayer;
    [SerializeField] private GameObject[] bulletPrefab;

    [SerializeField] private float timeBtwShoots = 2f;

    private float shootingTime = 0;
    private int whichWeapon = 0;

    private HumanMovement humanMovement;
    
    void Start()
    {
        humanMovement = GetComponent<HumanMovement>();

        shootingTime = 0;

        whichWeapon = Random.Range(0, bulletPrefab.Length);
    }
    
    void Update()
    {
        bool isPlayerNear = humanMovement.isPlayerNear;
        bool isGrabbed = humanMovement.isGrabbed;

        if (!isPlayerNear || isGrabbed)
        {
            return;
        }

        shootingTime -= Time.deltaTime;        

        if (shootingTime <= 0f)
        {
            Debug.Log("shooting " + whichWeapon + " to player");
            Instantiate(bulletPrefab[whichWeapon], transform.position, Quaternion.identity);
            shootingTime = timeBtwShoots;
        }
    }    
}

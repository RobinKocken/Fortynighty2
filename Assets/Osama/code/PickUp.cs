using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PickUp : MonoBehaviour
{
    public Transform cam;
    public RaycastHit hit;
    public PlayerController script;
    public Shooting code;

    public GameObject gun;
    public GameObject gunPlayer;
    public GameObject axe;
    public GameObject axePlayer;

    public GameObject treeParticle;
    
    public int treeHp;
    public int treeTrunkHp;
    public int metalScrapTotal;
    public int plankTotal;
    public float timeUntilOver = 30;
    public float timeUntilOver2 = 30;
    public float timeUntilOver3 = 30;
    public float cooldown;

    public bool gunIsOpgepakt;
    public bool axeIsOpgepakt;
    public bool gunIsActief;
    public bool axeIsActief;
    public bool damageBoostIsActief;
    public bool speedBoostIsActief;
    public bool jumpBoostIsActief;
    public bool cooldownActive;
    public bool kanHakken;

    public Sprite damage, speed, jump;


    // Start is called before the first frame update
    void Start()
    {

    }

    // Update is called once per frame
    void Update()
    {
        if (cooldownActive == true)
        {
            cooldown -= 1f * Time.deltaTime;
            kanHakken = false;
            if (cooldown < 0 || cooldown == 0)
            {
                kanHakken = true;
                cooldownActive = false;
                cooldown = 1f;
            }
        }

        if (Physics.Raycast(cam.position, cam.forward, out hit, 3))
        {
            Tree tree = hit.transform.GetComponentInParent<Tree>();

            print("a");
            if (tree != null)
            {
                print("b");
                if (Input.GetKeyDown(KeyCode.Mouse0))
                {
                    print("c");
                    if (axeIsActief == true)
                    {
                        print("d");
                        if (kanHakken == true)
                        {
                            print("e");
                            cooldownActive = true;
                            if (tree.canDamageTrunk == false)
                            {
                                print("f");
                                tree.ChopTree(1);
                                PlaceTreeParticle(hit.point, hit.normal, hit.transform);
                            }
                            else if (hit.transform.gameObject.CompareTag("TreeTrunk"))
                            {
                                print("g");
                                tree.ChopTrunk(1);
                                PlaceTreeParticle(hit.point, hit.normal, hit.transform);
                            }
                        }
                    } 
                }
            }
        }

        Debug.DrawRay(cam.position, cam.forward * 5, Color.red);
        if (Physics.Raycast(cam.position, cam.forward, out hit, 5))
        {
            if (hit.transform.gameObject.tag == "Gun")
            {
                if (Input.GetKeyDown(KeyCode.E))
                {
                    gun.SetActive(false);
                    gunIsOpgepakt = true;
                }
            }
        }
        if (Physics.Raycast(cam.position, cam.forward, out hit, 5))
        {
            if (hit.transform.gameObject.tag == "Axe")
            {
                if (Input.GetKeyDown(KeyCode.E))
                {
                    axe.SetActive(false);
                    axeIsOpgepakt = true;
                }
            }
        }

        if (gunIsOpgepakt == true)
        {
            if (Input.GetKeyDown(KeyCode.Alpha1))
            {
                axePlayer.SetActive(false);
                gunPlayer.SetActive(true);
                axeIsActief = false;
                gunIsActief = true;
                code.kanSchieten = true;
                kanHakken = false;
            }
        }
        if (axeIsOpgepakt == true)
        {
            if (Input.GetKeyDown(KeyCode.Alpha2))
            {
                axePlayer.SetActive(true);
                gunPlayer.SetActive(false);
                axeIsActief = true;
                gunIsActief = false;
                code.kanSchieten = false;
                kanHakken = true;
            }
        }
        if (Input.GetKeyDown(KeyCode.Alpha0))
        {
            axePlayer.SetActive(false);
            gunPlayer.SetActive(false);
            gunIsActief = false;
            axeIsActief = false;
            code.kanSchieten = false;
            kanHakken = false;
        }
        if (damageBoostIsActief)
        {
            timeUntilOver -= 1 * Time.deltaTime;
            print(timeUntilOver);
        }
        if (speedBoostIsActief)
        {
            timeUntilOver2 -= 1 * Time.deltaTime;
            print(timeUntilOver2);
        }
    }

    public void PlaceTreeParticle(Vector3 hitpoint, Vector3 normal,Transform parent)
    {
        var particle = Instantiate(treeParticle, hitpoint, Quaternion.LookRotation(normal), parent);
    }

    private void OnCollisionEnter(Collision collision)
    {
        if (collision.gameObject.tag == "MetalScrap")
        {
            Destroy(collision.gameObject);
            metalScrapTotal += 1;
        }

        if (collision.gameObject.tag == "Plank")
        {
            Destroy(collision.gameObject);
            plankTotal += 1;
        }
        if (collision.gameObject.tag == "DamageUp")
        {
            Destroy(collision.gameObject);
            damageBoostIsActief = true;
            Invoke("SetBoolBack", 30);

            PowerUpNotificationManager.instance.PickUp(Color.red,damage,30);
        }
        if (collision.gameObject.tag == "SpeedUp")
        {
            Destroy(collision.gameObject);
            speedBoostIsActief = true;
            Invoke("SetBoolBack", 30);

            PowerUpNotificationManager.instance.PickUp(Color.blue, speed, 30);
        }
        if (collision.gameObject.tag == "JumpUp")
        {
            Destroy(collision.gameObject);
            jumpBoostIsActief = true;
            Invoke("SetBoolBack", 30);

            PowerUpNotificationManager.instance.PickUp(Color.yellow, jump, 30);
        }
    }
    private void SetBoolBack()
    {
        damageBoostIsActief = false;
        speedBoostIsActief = false;
        jumpBoostIsActief = false;
        script.speed = script.walkSpeed;
    }
}

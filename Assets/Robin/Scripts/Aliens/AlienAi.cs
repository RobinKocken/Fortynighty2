using System.Collections;
using System.Collections.Generic;
using System.Diagnostics.Contracts;
using UnityEngine;
using UnityEngine.AI;

public class AlienAi : MonoBehaviour
{
    public InventoryManager inventory;
    public NavMeshAgent alienAgent;
    public GameObject alien;
    public GameObject shootPoint;
    public int health;

    public GameObject house;
    public GameObject housePoint;
    public GameObject aimPoint;
    public GameObject target;

    public GameObject explosionParticle;

    public float distance;

    public bool aggro;
    public bool pathing;

    [Header("Bullet")]
    public GameObject bullet;
    public float bulletSpeed;
    public int damage;
    float startTime;
    public float waitForSeconds;

    [Header("Rotation")]
    public float yRotSpeed;
    public float speed;
    public float xRot;
    public float zRot;

    [Header("Drop")]
    public Item item;
    public int min, max;

    void Start()
    {
        alienAgent = GetComponent<NavMeshAgent>();
        alienAgent.enabled = false;

        inventory = GameObject.FindGameObjectWithTag("Manager").GetComponent<InventoryManager>();
        house = GameObject.FindGameObjectWithTag("House");
        housePoint = GameObject.FindGameObjectWithTag("House").GetComponent<HouseScript>().hitPoint;

        aimPoint = housePoint;
        target = house;
    }

    void Update()
    {
        distance = Vector3.Distance(transform.position, target.transform.position);
        Aiming();
        if(alienAgent.enabled == true)
        {
            Path();
            Attack();
        }

        Animation();
        Death();
    }

    void Path()
    {
        if(distance > 30)
        {
            alienAgent.SetDestination(target.transform.position);
        }
        else if(distance < 29)
        {
            alienAgent.SetDestination(transform.position);
        }
    }

    void Attack()
    {
        if(target != null && distance < 30)
        {
            Shooting();
        }
        else if(target == null)
        {
            target = house;
            aimPoint = housePoint;
        }
    }

    void Shooting()
    {
        if(Time.time - startTime > waitForSeconds)
        {
            GameObject b = Instantiate(bullet, shootPoint.transform.position, Quaternion.identity);

            b.transform.SetParent(shootPoint.transform);
            b.transform.eulerAngles = shootPoint.transform.eulerAngles;
            b.transform.parent = null;

            b.GetComponent<AlienBulletScript>().target = target;
            b.GetComponent<AlienBulletScript>().speed = bulletSpeed;
            b.GetComponent<AlienBulletScript>().damage = damage;

            startTime = Time.time;
        }
    }

    void Aiming()
    {
        Quaternion quatShoot = Quaternion.Slerp(shootPoint.transform.localRotation, Quaternion.LookRotation(aimPoint.transform.position - shootPoint.transform.position), 100);
        shootPoint.transform.eulerAngles = new Vector3(quatShoot.eulerAngles.x, quatShoot.eulerAngles.y, quatShoot.eulerAngles.z);
    }

    void Animation()
    {
        alien.transform.Rotate(Vector3.up, yRotSpeed * Time.deltaTime);

        alien.transform.rotation = Quaternion.Euler(xRot * Mathf.Sin(Time.time * speed), alien.transform.eulerAngles.y, zRot * Mathf.Sin(Time.time * speed));
    }

    public void IsHit(GameObject turretHit)
    {
        target = turretHit;
        aimPoint = turretHit;
    }

    public void TakeDamage(int damage)
    {
        health -= damage;
    }

    void Death()
    {
        if(health <= 0)
        {
            Destroy(gameObject);
        }
    }

    public IEnumerator Lerping(Vector3 goTo)
    {
        while(true)
        {
            float distance = Vector3.Distance(transform.position, goTo);

            if(distance > 0.1f)
            {
                transform.position = Vector3.Lerp(transform.position, goTo, 2 * Time.deltaTime);
                transform.rotation = Quaternion.Slerp(transform.rotation, Quaternion.LookRotation(goTo - transform.position), 5 * Time.deltaTime);
            }
            else
            {
                alienAgent.enabled = true;
                yield break;
            }

            yield return null;
        }
    }

    private void OnDestroy()
    {
        inventory.AddItem(item, Random.Range(min, max));

        if (!Application.isPlaying) return;

        if(CameraShake.camShake != null)
            CameraShake.camShake.Explode(transform.position);

        Destroy(Instantiate(explosionParticle, transform.position, Quaternion.identity),10f);

        GameStats.ufosShotDown++;

        Destroy(gameObject);
    }
}

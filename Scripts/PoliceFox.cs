using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PoliceFox : MonoBehaviour
{
    private float hSpeed, vSpeed, scrollSpeed;
    private float xPos, zPos;
    private int spawnLocationDeterminer;
    private bool isStopped;
    public bool caughtPlayer;
    private Animator policeAnim;
    private SpawnManager spawnManPoliceFox;
    private GameObject player;
    public bool homingActive;
    // Start is called before the first frame update
    void Start()
    {
        scrollSpeed = -3.5f;
        spawnLocationDeterminer = Random.Range(1, 4);
        isStopped = false;
        caughtPlayer = false;
        homingActive = false;
        policeAnim = transform.Find("Animal_Fox_01").GetComponent<Animator>();
        spawnManPoliceFox = GameObject.Find("SpawnManager").GetComponent<SpawnManager>();
        player = GameObject.Find("Player");
        //Debug.Log(spawnLocationDeterminer);

        switch (spawnLocationDeterminer)
        {
            case 1:
                hSpeed = 6.0f;
                zPos = Random.Range(4.0f, 8.0f);
                xPos = -12.0f;
                transform.eulerAngles = new Vector3(transform.rotation.x, transform.rotation.y, transform.rotation.z);
                break;
            case 2:
                hSpeed = -6.0f;
                zPos = Random.Range(4.0f, 8.0f);
                xPos = 12.0f;
                transform.eulerAngles = new Vector3(transform.rotation.x, transform.rotation.y + 180, transform.rotation.z);
                break;
            case 3:
                vSpeed = -6.0f;
                xPos = Random.Range(-10.0f, 10.0f);
                zPos = Random.Range(7.0f, 8.0f);
                transform.eulerAngles = new Vector3(transform.rotation.x, transform.rotation.y + 90, transform.rotation.z);
                break;
        }

        transform.position = new Vector3(xPos, 0.3f, zPos);
    }

    // Update is called once per frame
    void Update()
    {
        if (!isStopped)
        {
            transform.position = new Vector3(transform.position.x + hSpeed * Time.deltaTime, transform.position.y, transform.position.z + (vSpeed + scrollSpeed) * Time.deltaTime);
        }
        if (homingActive && !isStopped)
        {
            homingMode();
        }
        outOfBoundsCheck();
    }

    private void OnTriggerEnter(Collider other)
    {
        //Debug.Log("Collided");
        if (other.CompareTag("Player") && other.GetType() != typeof(SphereCollider))
        {
            other.GetComponent<PlayerController>().isArrested = true;
            other.GetComponent<PlayerController>().gameOverTriggered = true;
            caughtPlayer = true;
            policeAnim.SetFloat("Speed_f", 0);
            policeAnim.enabled = false;
            isStopped = true;
        }
    }

    private void homingMode()
    {
        if (homingActive)
        {
            Vector2 policePosition = new Vector2(transform.position.x, transform.position.z);
            Vector2 playerPosition = new Vector2(player.transform.position.x, player.transform.position.z);

            Vector2 directionToPlayer = playerPosition - policePosition;
            directionToPlayer = -directionToPlayer.normalized;
            transform.rotation = Quaternion.LookRotation(transform.forward, directionToPlayer);
            transform.position = new Vector3(transform.position.x + directionToPlayer.x * 2.0f * Time.deltaTime, transform.position.y, transform.position.z + directionToPlayer.y * 2.0f * Time.deltaTime);
        }
    }

    private void outOfBoundsCheck()
    {
        if (transform.position.x > 20 || transform.position.x < -20 || transform.position.z < -10)
        {
            spawnManPoliceFox.deletePoliceFox(gameObject);
        }
    }
}

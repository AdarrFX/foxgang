using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BabyFox : MonoBehaviour
{
    private GameObject player;
    private PlayerController playerScript;
    private float moveSpeed = 3.0f;
    private Vector3 playerDirection;
    private float distToPlayer;
    private float scrollSpeed = -1.5f;
    private float wildFoxScrollSpeed = -5.5f;
    private Rigidbody babyRb;
    private Animator babyAnim;
    private Vector3 originalOrientation;
    private bool isFollowingPlayer = false;
    public AudioClip foxPickupSound;
    private AudioSource babyAudio; 
    // Start is called before the first frame update
    void Start()
    {
        player = GameObject.Find("Player");
        playerScript = player.GetComponent<PlayerController>();
        babyRb = GetComponent<Rigidbody>();
        babyAnim = GetComponent<Animator>();
        babyAnim.SetFloat("Speed_f", 0.5f);
        originalOrientation = transform.rotation.eulerAngles;
        babyAudio = GetComponent<AudioSource>();
    }

    // Update is called once per frame
    void Update()
    {
        babyRb.velocity = Vector3.zero;
        playerDirection = player.transform.position - transform.position;
        distToPlayer = playerDirection.magnitude;
        playerDirection = playerDirection.normalized;

        if (distToPlayer > 2.5 && isFollowingPlayer)
        {
            transform.position = new Vector3(transform.position.x + playerDirection.x * Time.deltaTime * moveSpeed, transform.position.y, transform.position.z + playerDirection.z * Time.deltaTime * moveSpeed);
        }
        transform.position = new Vector3(transform.position.x, transform.position.y, transform.position.z + (scrollSpeed + wildFoxScrollSpeed) * Time.deltaTime);

        checkHorizontalDistanceToPlayer();
        outOfBoundsCheck();
    }

    private void OnCollisionEnter(Collision collision)
    {
        if (!collision.gameObject.CompareTag("Player"))
        {
            collision.gameObject.GetComponent<Rigidbody>().velocity = Vector3.zero;
            Debug.Log(collision.collider.GetType());
        }

        if (collision.gameObject.CompareTag("Player") && !isFollowingPlayer && !playerScript.isArrested)
        {
            if (collision.collider.GetType() == typeof(SphereCollider))
            {
                //Debug.Log("Yep, it's a sphere-o");
                Physics.IgnoreCollision(collision.collider, GetComponent<BoxCollider>());
            }
            isFollowingPlayer = true;
            wildFoxScrollSpeed = 0;
            Vector3 newRotation = new Vector3(originalOrientation.x, originalOrientation.y + 180, originalOrientation.z);
            transform.eulerAngles = newRotation;
            originalOrientation = newRotation;
            babyAnim.SetFloat("Speed_f", 0.95f);

            // Trigger the Fox Up popup
            player.transform.GetChild(1).gameObject.SetActive(true);
            babyAudio.PlayOneShot(foxPickupSound, 1.0f);
            playerScript.addScore();

            // Start timer to remove Fox Up popup
            StartCoroutine(foxUpPopup());
        }
        babyRb.velocity = Vector3.zero;
    }

    private void checkHorizontalDistanceToPlayer()
    {
        float horizontalDistanceToPlayer = transform.position.x - player.transform.position.x;
        //Debug.Log(horizontalDistanceToPlayer);
        if (horizontalDistanceToPlayer > 5 && isFollowingPlayer)
        {
            transform.eulerAngles = new Vector3(originalOrientation.x, originalOrientation.y - 15, originalOrientation.z);
        } else if (horizontalDistanceToPlayer < -5 && isFollowingPlayer)
        {
            transform.eulerAngles = new Vector3(originalOrientation.x, originalOrientation.y + 15, originalOrientation.z);
        } else
        {
            transform.eulerAngles = new Vector3(originalOrientation.x, originalOrientation.y, originalOrientation.z);
        }
    }

    private void outOfBoundsCheck()
    {
        if (transform.position.z < -10)
        {
            Destroy(gameObject);
        }
    }

    IEnumerator foxUpPopup()
    {
        yield return new WaitForSeconds(1);
        player.transform.GetChild(1).gameObject.SetActive(false);
    }
}

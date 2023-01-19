using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CageBehaviour : MonoBehaviour
{
    private GameObject player;
    private Vector3 playerPosition;
    private AudioSource cageSound;
    private CameraControl gameCamera;
    private SpawnManager spawnMan;
    private GameObject ground;
    // Start is called before the first frame update
    void Start()
    {
        player = GameObject.Find("Player");
        playerPosition = player.transform.position;
        cageSound = GetComponent<AudioSource>();
        gameCamera = GameObject.Find("Main Camera").GetComponent<CameraControl>();
        spawnMan = GameObject.Find("SpawnManager").GetComponent<SpawnManager>();
        ground = GameObject.Find("Plane");
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    IEnumerator lowerCageOntoPlayer(float timeToMove)
    {
        playerPosition = new Vector3(player.transform.position.x, player.transform.position.y + 2.12f, player.transform.position.z);
        transform.position = new Vector3(playerPosition.x, playerPosition.y + 10.0f, playerPosition.z);
        gameObject.SetActive(true);

        yield return new WaitForSeconds(0.5f);
        float elapsedTime = 0;

        cageSound.Play();
        gameCamera.cameraShakeOn = true;
        spawnMan.deleteAllPolice();
        spawnMan.deleteAllRocks();
        ground.SetActive(false);

        while (elapsedTime < timeToMove)
        {
            transform.position = Vector3.Lerp(transform.position, playerPosition, (elapsedTime / timeToMove));
            elapsedTime += Time.deltaTime;
            yield return null;
        }
    }

    public void cagePlayer()
    {
        StartCoroutine(lowerCageOntoPlayer(0.8f));
    }
}

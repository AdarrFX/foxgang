using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraControl : MonoBehaviour
{
    // Start is called before the first frame update
    public GameObject player;
    public Vector3 playerPositionAndOffset;
    private Vector3 originalCameraPosition;
    private bool resetCameraPosition;
    private float gameOverCameraOffsetX, gameOverCameraOffsetY, gameOverCameraOffsetZ;
    private float cameraShakeTimer;
    public bool cameraShakeOn;
    private float cameraShakeOffsetX, cameraShakeOffsetY;
    public AudioClip camWooshSound;
    public AudioClip capturedSound;
    private AudioSource gameOverAudio;
    private SpawnManager spawnMan;
    private CageBehaviour cagey;
    private GameHandler gameHandler;
    //private bool triggerGameOverCamera;
    void Start()
    {
        cameraShakeTimer = 0;
        cameraShakeOn = false;
        gameOverCameraOffsetX = 1.5f;
        gameOverCameraOffsetY = 0.5f;
        gameOverCameraOffsetZ = 5.5f;
        playerPositionAndOffset = new Vector3(player.transform.position.x + gameOverCameraOffsetX, player.transform.position.y + gameOverCameraOffsetY, player.transform.position.z + gameOverCameraOffsetZ);
        originalCameraPosition = transform.position;
        spawnMan = GameObject.Find("SpawnManager").GetComponent<SpawnManager>();
        gameOverAudio = GameObject.Find("SoundEffectManager").GetComponent<AudioSource>();
        cagey = GameObject.Find("ArrestedCage").GetComponent<CageBehaviour>();
        gameHandler = GameObject.Find("GameHandler").GetComponent<GameHandler>();
    }

    // Update is called once per frame
    void Update()
    {
        playerPositionAndOffset = new Vector3(player.transform.position.x + gameOverCameraOffsetX, player.transform.position.y + gameOverCameraOffsetY, player.transform.position.z + gameOverCameraOffsetZ);
        
        if (cameraShakeTimer > 0)
        {
            transform.position = originalCameraPosition;
        }

        transform.position = new Vector3(transform.position.x + cameraShakeOffsetX, transform.position.y + cameraShakeOffsetY, transform.position.z);

        cameraShake();
    }

    public void cameraShake()
    {
        if (cameraShakeTimer > 0)
        {
            cameraShakeTimer -= Time.deltaTime;
            cameraShakeOffsetX = Random.Range(-0.3f, 0.3f);
            cameraShakeOffsetY = Random.Range(-0.3f, 0.3f);
            cameraShakeOn = false;
        } 
        else if (cameraShakeTimer <= 0)
        {
            cameraShakeOffsetY = 0;
            cameraShakeOffsetX = 0;
        }
        
        if (cameraShakeOn)
        {
            cameraShakeTimer = 0.8f;
            cameraShakeOn = false;
            originalCameraPosition = transform.position;
            cameraShakeOffsetX = 0;
            cameraShakeOffsetY = 0;
        }
    }

    IEnumerator gameOverCamera(float timeToMove)
    {
        yield return new WaitForSeconds(1.0f);
        float elapsedTime = 0;
        gameOverAudio.PlayOneShot(camWooshSound, 1.0f);
        spawnMan.deleteAllBabies();

        while (elapsedTime < timeToMove)
        {
            transform.position = Vector3.Lerp(transform.position, playerPositionAndOffset, (elapsedTime / timeToMove));
            transform.LookAt(player.transform);
            elapsedTime += Time.deltaTime;
            yield return null;
        }
        originalCameraPosition = transform.position;
        cagey.cagePlayer();
        gameHandler.slideInArrestedText();
        gameHandler.spawnBabiesOnGameOver();
    }

    public void triggerGameOverCamera()
    {
        StartCoroutine(gameOverCamera(0.5f));
    }
}

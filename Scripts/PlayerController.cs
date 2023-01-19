using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using TMPro;

public class PlayerController : MonoBehaviour
{
    private float speed = 8.0f;
    private float horizontalSpeedBoost = 0.5f;
    private float horizontalBound, verticalBound;
    public bool isArrested = false;
    private Vector3 originalOrientation;
    private Transform foxModel;
    public TextMeshProUGUI uiScoreText;
    public GameObject wantedImage;
    private CameraControl gameCamera;
    private int gangSize;
    private int wantedLevel;
    private bool wantedCheck;
    public bool gameOverTriggered;
    private Animator playerAnim;
    private SpawnManager spawnManager;
    private GameHandler gameHandler;
    public AudioClip hitSound;
    private AudioSource hitAudio;
    public Transform babyFoxPrefab;
    private SphereCollider playerFoxCollector;
    // Start is called before the first frame update
    void Start()
    {
        horizontalBound = 10.0f;
        verticalBound = 4.5f;
        originalOrientation = transform.rotation.eulerAngles;
        foxModel = transform.Find("Animal_Fox_02");
        playerAnim = foxModel.GetComponent<Animator>();
        gangSize = 0;
        wantedLevel = 6;
        wantedCheck = true;
        gameOverTriggered = false;
        spawnManager = GameObject.Find("SpawnManager").GetComponent<SpawnManager>();
        gameHandler = GameObject.Find("GameHandler").GetComponent<GameHandler>();
        gameCamera = GameObject.Find("Main Camera").GetComponent<CameraControl>();
        hitAudio = GetComponent<AudioSource>();
        playerFoxCollector = GetComponent<SphereCollider>();
    }

    // Update is called once per frame
    void Update()
    {
        if (gameHandler.gameIsRunning)
        {
            foxModel.transform.eulerAngles = originalOrientation;
            handlePlayerInput();
            if (isArrested && gameOverTriggered)
            {
                gameOverSequence();
            }

            // Handles displaying a new wanted star every 5 foxxos
            if (gangSize % 5 == 0 && gangSize != 0 && gangSize <= 30 && wantedCheck)
            {
                wantedImage.transform.GetChild(wantedLevel).gameObject.SetActive(true);
                wantedLevel -= 1;
                wantedCheck = false;
                spawnManager.increaseWantedLevel();
            }

            if (Input.GetKeyDown(KeyCode.Space))
            {
                addScore();
            }
            uiScoreText.text = gangSize.ToString();
        }
        if (gameHandler.canRestart)
        {
            if (Input.GetKeyDown(KeyCode.R) || Input.GetButtonDown("Submit"))
            {
                SceneManager.LoadScene("My Game");
            }
            else if (Input.GetKeyDown(KeyCode.Q) || Input.GetButtonDown("Cancel"))
            {
                SceneManager.LoadScene("MainMenu");
            }
        }

    }

    void handlePlayerInput()
    {
        if (!spawnManager.isPaused && !isArrested)
        {
            if ((Input.GetKey(KeyCode.UpArrow) || Input.GetAxis("Vertical") >= 0.09) && transform.position.z <= verticalBound)
            {
                transform.position = transform.position + new Vector3(0, 0, speed * Time.deltaTime);
            }
            if ((Input.GetKey(KeyCode.DownArrow) || Input.GetAxis("Vertical") <= -0.09) && transform.position.z >= -verticalBound)
            {
                transform.position = transform.position + new Vector3(0, 0, -speed * Time.deltaTime);
            }
            if ((Input.GetKey(KeyCode.RightArrow) || Input.GetAxis("Horizontal") >= 0.1) && transform.position.x <= horizontalBound)
            {
                transform.position = transform.position + new Vector3((speed + horizontalSpeedBoost) * Time.deltaTime, 0, 0);
                foxModel.transform.eulerAngles = new Vector3(originalOrientation.x, originalOrientation.y + 35, originalOrientation.z);

            }
            if ((Input.GetKey(KeyCode.LeftArrow) || Input.GetAxis("Horizontal") <= -0.1) && transform.position.x >= -horizontalBound)
            {
                transform.position = transform.position + new Vector3(-(speed + horizontalSpeedBoost) * Time.deltaTime, 0, 0);
                foxModel.transform.eulerAngles = new Vector3(originalOrientation.x, originalOrientation.y - 35, originalOrientation.z);

            }
        }
    }

    void gameOverSequence()
    {
        transform.GetChild(1).gameObject.SetActive(false);
        gameHandler.gameOver();
        gameCamera.cameraShakeOn = true;
        playerAnim.SetFloat("Speed_f", 0.0f);
        hitAudio.PlayOneShot(hitSound, 1.0f);
        gameCamera.triggerGameOverCamera();
        gameOverTriggered = false;
    }

    public void addScore()
    {
        gangSize += 1;
        wantedCheck = true;
    }

    public int getGangSize()
    {
        return gangSize;
    }
}

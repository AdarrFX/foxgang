using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class GameHandler : MonoBehaviour
{
    // Start is called before the first frame update
    public Material skyBlack;
    public Camera mainCamera;
    public GameObject player;
    public GameObject UICanvas;
    public GameObject babyModel;
    public GameObject arrestedText;
    public GameObject restartText;
    public GameObject gameOverGangUI;
    private PlayerController playerScript;
    private MusicManager musicMan;
    private SpawnManager spawnMan;
    private GameObject uiLeft, uiRight;
    private TextMeshProUGUI scoreTextGameOver;
    private AudioSource gameOverAudio;
    public AudioClip foxSpawnSoundGameOverScreen;
    public bool gameIsRunning = true;
    public bool canRestart = false;

    void Start()
    {
        musicMan = mainCamera.GetComponent<MusicManager>();
        spawnMan = GameObject.Find("SpawnManager").GetComponent<SpawnManager>();
        playerScript = player.GetComponent<PlayerController>();
        uiLeft = UICanvas.transform.Find("TopLeft").gameObject;
        uiRight = UICanvas.transform.Find("TopRight").gameObject;
        gameOverAudio = GetComponent<AudioSource>();
        scoreTextGameOver = gameOverGangUI.transform.Find("GangSizeGameOver").gameObject.GetComponent<TextMeshProUGUI>();
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void gameOver()
    {
        RenderSettings.skybox = skyBlack;
        musicMan.toggleMusic();
        gameIsRunning = false;
        spawnMan.deleteAllPoliceFoxesExceptCaughtOne();
    }

    IEnumerator slideRight(GameObject canvasObject, float timeToMove, float xCoord)
    {
        float elapsedTime = 0;
        yield return new WaitForSeconds(1.0f);

        uiLeft.SetActive(false);
        uiRight.SetActive(false);
        Vector3 positionTarget = new Vector3(xCoord, canvasObject.GetComponent<RectTransform>().anchoredPosition.y, 0);

        while (elapsedTime < timeToMove)
        {
            canvasObject.GetComponent<RectTransform>().anchoredPosition = Vector3.Lerp(canvasObject.GetComponent<RectTransform>().anchoredPosition, positionTarget, (elapsedTime / timeToMove));
            elapsedTime += Time.deltaTime;
            yield return null;
        }

    }

    IEnumerator spawnGameOverFoxes(GameObject babyFoxModel, int finalGangSize)
    {
        player.transform.GetChild(1).gameObject.SetActive(false);
        yield return new WaitForSeconds(2.0f);
        gameOverGangUI.SetActive(true);
        float xOffset, yOffset, zOffset;
        xOffset = player.transform.position.x - 4.0f;
        yOffset = player.transform.position.y - 1.0f;
        zOffset = player.transform.position.z - 3.0f;
        Vector3 babyModelPosition = new Vector3(xOffset, yOffset, zOffset);

        for (int i=1; i<=finalGangSize; i++)
        {
            yield return new WaitForSeconds(0.1f);
            gameOverAudio.PlayOneShot(foxSpawnSoundGameOverScreen, 1.0f);
            Debug.Log(babyModelPosition);
            Debug.Log(i);
            Instantiate(babyFoxModel, babyModelPosition, babyFoxModel.transform.rotation);
            scoreTextGameOver.text = i.ToString();
            xOffset -= 0.2f;
            zOffset += 0.3f;
            babyModelPosition = new Vector3(xOffset, yOffset, zOffset);

            if (i % 10 == 0 && i != 0)
            {
                xOffset += 3.1f;
                zOffset += -2.8f;
                yOffset -= 0.15f;
            }
        }
        restartText.gameObject.SetActive(true);
        canRestart = true;
    }

    public void slideInArrestedText()
    {
        StartCoroutine(slideRight(arrestedText, 0.8f, 90.0f));
    }

    public void spawnBabiesOnGameOver()
    {
        StartCoroutine(spawnGameOverFoxes(babyModel, playerScript.getGangSize()));
    }
}

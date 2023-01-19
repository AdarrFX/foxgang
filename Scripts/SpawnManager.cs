using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine.SceneManagement;
using UnityEngine;

public class SpawnManager : MonoBehaviour
{
    public GameObject babyFox;
    public GameObject policeFox;
    public GameObject rockWall;
    private float babySpawnInterval, babySpawnTimer;
    private float policeSpawnInterval, policeSpawnTimer;
    private GameHandler gameHandler;
    public bool isPaused = false;
    private MusicManager musicHandler;
    public TextMeshProUGUI pauseTextPaused, pauseTextPressQ;
    private List<GameObject> policeFoxList = new List<GameObject>();
    private List<GameObject> babyFoxList = new List<GameObject>();
    private List<GameObject> RockWallList = new List<GameObject>();
    private int spawnChance;

    // Start is called before the first frame update
    void Start()
    {
        InvokeRepeating("spawnCliffs", 2.0f, 0.25f);
        babySpawnInterval = 1.7f;
        policeSpawnInterval = 1.8f;
        spawnChance = 15;
        babySpawnTimer = babySpawnInterval;
        policeSpawnTimer = policeSpawnInterval;
        gameHandler = GameObject.Find("GameHandler").GetComponent<GameHandler>();
        musicHandler = GameObject.Find("Main Camera").GetComponent<MusicManager>();
        gameHandler.gameIsRunning = true;
        InvokeRepeating("spawnExtraPolice", 3.0f, 0.25f);
    }

    // Update is called once per frame
    void Update()
    {
        if (gameHandler.gameIsRunning)
        {
            // Used for testing
            //if (Input.GetKeyDown(KeyCode.Space))
            //{
            //    GameObject newBabyFox = Instantiate(babyFox, randomSpawnPosition(), babyFox.transform.rotation);
            //    babyFoxList.Add(newBabyFox);
            //    GameObject newPolice = Instantiate(policeFox, policeFox.transform.position, policeFox.transform.rotation);
            //    policeFoxList.Add(newPolice);
            //}
            if (Input.GetKeyDown(KeyCode.Escape))
            {
                pauseGame();
            }
            spawnBabyTimer();
            spawnPoliceTimer();

            // Quit to menu is user presses q while game is paused
            if (isPaused && Input.GetKeyDown(KeyCode.Q))
            {
                isPaused = false;
                Time.timeScale = 1;
                SceneManager.LoadScene("MainMenu");
            }

        } else
        {
            CancelInvoke();
        }
    }

    Vector3 randomSpawnPosition()
    {
        return new Vector3(Random.Range(-9.0f, 9.0f), 0, Random.Range(7.0f, 9.0f));
    }

    void spawnRock(int rockType)
    {
        Vector3 randomRotations = new Vector3(Random.Range(0, 180), Random.Range(0, 180), Random.Range(0, 180));
        float rockPositionX = -20.0f;

        if (rockType == 1)
        {
            rockPositionX = Random.Range(-12.5f, -13.5f);
        }
        if (rockType == 2)
        {
            rockPositionX = Random.Range(12.5f, 13.5f);
        }

        GameObject newRockWall = Instantiate(rockWall, new Vector3(rockPositionX, 0, 10), Quaternion.Euler(randomRotations));
        RockWallList.Add(newRockWall);
    }

    void spawnExtraPolice()
    {
        int spawnRoll = Random.Range(1, 101);
        if (spawnRoll < spawnChance)
        {
            policeFoxList.Add(Instantiate(policeFox, policeFox.transform.position, policeFox.transform.rotation));
        }
    }

    void spawnCliffs()
    {
        spawnRock(1);
        spawnRock(2);
    }

    void spawnBabyTimer()
    {
        babySpawnTimer -= Time.deltaTime;
        if (babySpawnTimer <= 0)
        {
            GameObject newBabyFox = Instantiate(babyFox, randomSpawnPosition(), babyFox.transform.rotation);
            babyFoxList.Add(newBabyFox);
            babySpawnTimer = babySpawnInterval;
        }
    }

    void spawnPoliceTimer()
    {
        policeSpawnTimer -= Time.deltaTime;
        if (policeSpawnTimer <= 0)
        {
            GameObject newPolice = Instantiate(policeFox, policeFox.transform.position, policeFox.transform.rotation);
            policeFoxList.Add(newPolice);
            policeSpawnTimer = policeSpawnInterval;
        }
    }

    public void deleteAllPoliceFoxesExceptCaughtOne()
    {
        foreach (GameObject policeFoxxo in policeFoxList)
        {
            if (!policeFoxxo.gameObject.GetComponent<PoliceFox>().caughtPlayer)
            {
                Destroy(policeFoxxo);
            }
        }
    }

    public void deleteAllBabies()
    {
        foreach (GameObject babyFoxxo in babyFoxList)
        {
            Destroy(babyFoxxo);
        }
        babyFoxList.Clear();
    }

    public void deleteAllPolice()
    {
        foreach (GameObject policeBoi in policeFoxList)
        {
            Destroy(policeBoi);
        }
        policeFoxList.Clear();
    }

    public void deletePoliceFox(GameObject policeFoxToBeDeleted)
    {
        policeFoxList.Remove(policeFoxToBeDeleted);
        Destroy(policeFoxToBeDeleted);
    }

    public void deleteAllRocks()
    {
        foreach (GameObject rock in RockWallList)
        {
            Destroy(rock);
        }
        RockWallList.Clear();
    }

    public void increaseWantedLevel()
    {
        //We increase the "wanted" level by just making the police foxes and babies spawn faster
        policeSpawnInterval -= 0.2f;
        babySpawnInterval -= 0.14f;
        spawnChance += 10;
    }

    void pauseGame()
    {
        if (!isPaused)
        {
            Time.timeScale = 0;
            isPaused = true;
            pauseTextPaused.gameObject.SetActive(true);
            pauseTextPressQ.gameObject.SetActive(true);
            musicHandler.toggleMusic();
        }
        else
        {
            Time.timeScale = 1;
            isPaused = false;
            pauseTextPaused.gameObject.SetActive(false);
            pauseTextPressQ.gameObject.SetActive(false);
            musicHandler.toggleMusic();
        }
    }

}

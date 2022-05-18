using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class GameManager : MonoBehaviour
{
    public static GameManager instance;
    void Awake()
    {
        if (instance != null)
        {
            return;
        }

        instance = this;
    }


    /*
     * 0 - BackGround
     * 1 - Regular platforms
     */
    public float[] multiplier;
    public float[] timers;
    public float score, scoreMultiplier = 1;
    public KeyCode pauseMenuKey, up, down, enter;
    public AudioSource audioSource;
    public GameObject pauseMenu, arrowL, arrowR, camPos, drone;
    public int option;
    public bool isMainMenu, startGame, enemy1Active, enemy2Active, fightActive, paused;

    private PlayerController player;
    private float[] multiplierHolder;

    // Start is called before the first frame update
    void Start()
    {
        Cursor.visible = false;

        StartCoroutine(SpawnDrone());
        StartCoroutine(IncreaseSpeed());
        StartCoroutine(StartFight());
    }


    // Update is called once per frame
    void Update()
    {
        if (player == null)
        {
            player = PlayerController.instance;
        }

        fightActive = true;

        CamAdjuster();
        CheckPlayer();
        MenuController();
    }

    IEnumerator StartFight()
    {
        while (startGame == false)
        {
            yield return new WaitForSeconds(.15f);
        }

        yield return new WaitForSeconds(timers[4]);
        while (enemy1Active || enemy2Active)
        {
            yield return new WaitForSeconds(1f);
        }
        fightActive = true;
        multiplierHolder = new float[multiplier.Length];
        for (int i = 0; i < multiplierHolder.Length; i++)
        {
            multiplierHolder[i] = multiplier[i];
        }
        multiplier[0] = 40;
        multiplier[1] = 80;
        multiplier[2] = 120;
        multiplier[3] = 40;
        yield return new WaitForSeconds(timers[0]);
        multiplier = multiplierHolder;
        fightActive = false;
    }

    IEnumerator IncreaseSpeed()
    {
        while (startGame == false)
        {
            yield return new WaitForSeconds(.15f);
        }

        yield return new WaitForSeconds(timers[0]);
        while (fightActive || enemy1Active || enemy2Active)
        {
            yield return new WaitForSeconds(.15f);
        }
        scoreMultiplier += .1f;
        for (int i = 0; i < multiplier.Length; i++)
        {
            if (i != 5 && i != 6 && i != 7)
            {
                multiplier[i] += 40f;
            }
        }
    }

    IEnumerator SpawnDrone()
    {
        while (startGame == false)
        {
            yield return new WaitForSeconds(.15f);
        }

        yield return new WaitForSeconds(timers[3]);
        while (fightActive || enemy1Active || enemy2Active)
        {
            yield return new WaitForSeconds(1f);
        }
        drone.SetActive(true);
    }

    void MenuController()
    {
        if (Input.GetKeyDown(pauseMenuKey))
        {
            if (isMainMenu)
            {
                Application.Quit();
            }
            else
            {
                if (paused && player != null)
                {
                    paused = false;
                }
                else
                {
                    paused = true;
                    option = 0;
                }
            }
        }

        if (isMainMenu)
        {
            MenuInput();
            startGame = true;
        }
        else
        {
            PauseGame();
        }
    }

    void CamAdjuster()
    {
        double goldenRatio = Camera.main.aspect * 8.88686 / (1.777372);
        Camera.main.transform.position = new Vector3(camPos.transform.position.x + (float)goldenRatio, 0, -10f);
    }

    void CheckPlayer()
    {
        if (startGame && player == null)
        {
            paused = true;
        }
    }

    void MenuInput()
    {
        if (Input.GetKeyDown(up))
        {
            if (option != 0)
            {
                option--;
            }
        }

        if (Input.GetKeyDown(down))
        {
            if (isMainMenu)
            {
                if (option != 3)
                {
                    option++;
                }
            }
            else
            {
                if (option != 4)
                {
                    option++;
                }
            }
        }

        if (Input.GetKeyDown(enter))
        {
            if (isMainMenu)
            {
                if (option == 0)
                {
                    option = 0;
                    isMainMenu = false;
                    SceneManager.LoadScene("Episode0");
                }
                else if (option == 1)
                {
                    //options
                }
                else if (option == 2)
                {
                    //credits
                }
                else
                {
                    Application.Quit();
                }
            }
            else
            {
                if (option == 0)
                {
                    if (player != null)
                    {
                        paused = false;
                    }
                }
                else if (option == 1)
                {
                    SceneManager.LoadScene(SceneManager.GetActiveScene().name);
                }
                else if (option == 2)
                {
                    //options menu
                }
                else if (option == 3)
                {
                    option = 0;
                    isMainMenu = true;
                    SceneManager.LoadScene("MainMenu");
                }
                else
                {
                    Application.Quit();
                }
            }
        }
    }

    void PauseGame()
    {
        if (paused)
        {
            MenuInput();

            Time.timeScale = 0;
            audioSource.volume = .05f;
            pauseMenu.SetActive(true);
            arrowL.SetActive(true);
            arrowR.SetActive(true);
        }
        else
        {
            if (player != null && player.isShooting)
            {
                Time.timeScale = .25f;
            }
            else
            {
                Time.timeScale = 1f;
            }
            audioSource.volume = .25f;
            pauseMenu.SetActive(false);
            arrowL.SetActive(false);
            arrowR.SetActive(false);
        }
    }
}

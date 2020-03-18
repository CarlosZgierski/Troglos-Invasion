using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class GameManager : MonoBehaviour {

    //Variable for pausing the game and making it come back to normal
    private bool worldIsPaused = false;
    private float defaulTimeScale;

    //Global reference of the pause state
    public static bool IS_WORLD_PAUSED = false;

    //Variables for the pause menu
    [SerializeField] private GameObject pauseCanvas;

    [Space]
    //Variablers for the Map UI
    [SerializeField] private GameObject MapCanvas;
    [Space]
    [SerializeField] private GameObject[] mapFloors;

    [Space]
    //Variables for the Game over
    [SerializeField] private GameObject GameOverCanvas;

    private bool mapIsOpen = false;

    [SerializeField]
    private GameObject Instrucoes;
    private void Start()
    {
       // Cursor.lockState = CursorLockMode.Locked; //Mouse lock State

        defaulTimeScale = Time.timeScale; //The default time Scale of the game
    }

    void LateUpdate()
    {
        PauseWorld();
        MapHandler();
    }

    #region Pause related Stuff

    void PauseWorld()
    {
        if (Input.GetKeyDown(KeyCode.P) || Input.GetKeyDown(KeyCode.Escape))
        {
            if (!worldIsPaused) //if the world is not paused, do this
            {
                Pause();
            }
            else if (worldIsPaused) //if the world is paused, do this
            {
                if (mapIsOpen)
                {
                    MapCanvas.SetActive(false);
                    mapIsOpen = false;
                }
                Unpause();
            }
        }
    }

    public void Pause()
    {
        Cursor.lockState = CursorLockMode.None;
        pauseCanvas.SetActive(true);

        Time.timeScale = 0;
        worldIsPaused = true;
        IS_WORLD_PAUSED = true;
    }

    public void Unpause()
    {
        Cursor.lockState = CursorLockMode.Locked;
        pauseCanvas.SetActive(false);

        Time.timeScale = defaulTimeScale;
        worldIsPaused = false;
        IS_WORLD_PAUSED = false;
    }

    public void GoMenu()
    {
        //make it go back to the menu
        SceneManager.LoadScene(0);
        Time.timeScale = defaulTimeScale;
    }

    public void GoOptions()
    {
        //make it go to the options ui
    }
    #endregion

    #region Map related Stuff

    private void MapHandler()
    {
        if (Input.GetKeyDown(KeyCode.M))
        {
            if (!mapIsOpen)
            {
                MapCanvas.SetActive(true);
                mapIsOpen = true;
                Pause();
            }
            else
            {
                MapCanvas.SetActive(false);
                mapIsOpen = false;
                Unpause();
            }
        }
    }

    //used to change the current floor the player is looking
    public void ChangeFloor(int _floor)
    {
        for (int x = 0; x < mapFloors.Length; x++)
        {
            if (x == _floor - 1)
            {
                mapFloors[x].SetActive(true);
            }
            else
            {
                mapFloors[x].SetActive(false);
            }
        }
    }
    #endregion

    #region Game Over related stuff

    public void GameOver()
    {
        Cursor.lockState = CursorLockMode.None;
        GameOverCanvas.SetActive(true);
    }

    public void TryAgain()
    {
        SceneManager.LoadScene(0);
        Time.timeScale = defaulTimeScale;
    }

    #endregion


    public void Mapa()
    {
        Instrucoes.SetActive(false);
    }

    public void Instructions()
    {
        Instrucoes.SetActive(true);
    }

}

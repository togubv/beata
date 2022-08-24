using UnityEngine;
using UnityEngine.UI;

public class Core : MonoBehaviour
{
    [SerializeField] private Generator generator;

    [SerializeField] private Slider sliderLifes;

    [SerializeField] public GameState gameState;
    [SerializeField] public int mistakes, multiplier, score;
    [SerializeField] public GameObject goPlayer;
    [SerializeField] public int lifes;

    public delegate void UpdateScoreHandler(int oldValue, int newValue);
    public event UpdateScoreHandler UpdateScoreHandlerEvent;
    public delegate void UpdateMultiplierHandler(int oldValue, int newValue);
    public event UpdateMultiplierHandler UpdateMultiplierHandlerEvent;
    public delegate void UpdateMistakeHandler(int oldValue, int newValue);
    public event UpdateMistakeHandler UpdateMistakeHandlerEvent;
    public delegate void UpdateGameSpeedHandler(float speed);
    public event UpdateGameSpeedHandler UpdateGameSpeedHandlerEvent;
    
    public delegate void UpdateGameStateHandler(GameState state);
    public event UpdateGameStateHandler UpdateGameStateHandlerEvent;
    public delegate void UpdateLevelStateHandler(LevelState state);
    public event UpdateLevelStateHandler UpdateLevelStateHandlerEvent;

    public float Speed => speed;

    private float speed = 6, savedSpeed;
    private int currentlvl = 1;

    private void Awake()
    {
        Screen.SetResolution(2160, 1080, true);
        this.savedSpeed = this.speed;
        ToggleGameState(GameState.Pause);
    }

    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.Escape))
        {
            ToggleMenuPause();
        }
    }

    public void ToggleGameState(GameState state)
    {
        this.gameState = state;

        switch ((int)state)
        {
            case 0:
                this.savedSpeed = this.speed;
                UpdateGameSpeed(0);
                break;
            case 1:
                UpdateGameSpeed(this.savedSpeed);
                break;
        }

        UpdateGameStateHandlerEvent?.Invoke(state);
    }

    public void ClickToMenuButtonContinue()
    {
        ToggleGameState(GameState.Game);
    }

    public void ToggleMenuPause()
    {
        if (gameState == GameState.Game)
        {
            ToggleGameState(GameState.Pause);
            return;
        }

        ToggleGameState(GameState.Game);
    }

    public void IncreaseScore(int count)
    {
        int oldValue = this.score;
        this.score += count;

        UpdateScoreHandlerEvent?.Invoke(oldValue, this.score);
    }

    public void DecreaseScore(int count)
    {
        if (this.score < count)
            return;

        int oldValue = this.score;
        this.score -= count;

        UpdateScoreHandlerEvent?.Invoke(oldValue, this.score);
    }

    public void IncreaseMultiplier(int count)
    {
        int oldValue = this.multiplier;
        this.multiplier += count;

        UpdateMultiplierHandlerEvent?.Invoke(oldValue, this.multiplier);
    }

    public void DecreaseMultiplier(int count)
    {
        if (this.multiplier < count)
            return;

        int oldValue = this.multiplier;
        this.multiplier -= count;

        UpdateMultiplierHandlerEvent?.Invoke(oldValue, this.multiplier);
    }

    public void IncreaseMistakes(int count)
    {
        int oldCount = this.mistakes;
        this.mistakes += count;

        UpdateMistakeHandlerEvent?.Invoke(oldCount, this.mistakes);

        if (this.mistakes >= lifes && lifes < 51)
        {
            LoseLevel();
        }
    }

    public void DecreaseMistake(int count)
    {
        if (this.mistakes < count)
            return;

        int oldCount = this.mistakes;
        this.mistakes -= count;

        UpdateMistakeHandlerEvent?.Invoke(oldCount, this.mistakes);
    }

    public void BreakMultiplier(int position)
    {
        DecreaseMultiplier(this.multiplier);
        IncreaseMistakes(1);

        generator.ReturnQuads(position);
    }

    public void AddScore(int position)
    {
        IncreaseMultiplier(1);
        IncreaseScore(10 * multiplier);

        generator.ReturnQuads(position);
    }

    public void UpdateGameSpeed(float speed)
    {
        this.speed = speed;
        UpdateGameSpeedHandlerEvent?.Invoke(this.speed);
    }

    public void FinishLevel()
    {
        UpdateLevelStateHandlerEvent?.Invoke(LevelState.Finish);
        this.savedSpeed = this.speed;
        UpdateGameSpeed(0);
    }

    public void LoseLevel()
    {
        UpdateLevelStateHandlerEvent?.Invoke(LevelState.Lose);
        UpdateGameSpeed(0);
        this.savedSpeed = 0;
    }

    public void NextLevel()
    {
        ToggleGameState(GameState.Game);
        UpdateLevelStateHandlerEvent?.Invoke(LevelState.Start);

        DecreaseMultiplier(this.multiplier);        

        generator.ReturnToPoolOtherPrefabs();
        currentlvl++;
        this.savedSpeed = this.speed + 4;
        UpdateGameSpeed(this.savedSpeed);
        generator.GenerateLevel(20, 10, currentlvl);
    }

    public void StartGame()
    {
        DecreaseScore(this.score);
        DecreaseMistake(this.mistakes);
        this.savedSpeed = 6;
        UpdateGameSpeed(this.savedSpeed);
        lifes = (int)sliderLifes.value;
        currentlvl = 1;
        NextLevel();
    }

    public void ExitGame()
    {
        Application.Quit();
    }
}

public enum GameState
{
    Pause,
    Game
}

public enum LevelState
{
    Start,
    Finish,
    Lose
}

public interface IAnimatable
{
    void PlayDestroyAnimation();
}

public interface IPosition
{
    int Take();
}

public enum QuadType
{
    None,
    Empty,
    Finish
}

public enum JoystickPosition
{
    Right,
    Left
}

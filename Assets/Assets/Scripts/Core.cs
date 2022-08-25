using UnityEngine;
using UnityEngine.UI;

public class Core : MonoBehaviour
{
    [SerializeField] private Generator generator;

    [SerializeField] private Slider sliderLifes;

    [SerializeField] private GameObject _goPlayer;
    [SerializeField] private int _lifes;

    public delegate void Update_scoreHandler(int oldValue, int newValue);
    public event Update_scoreHandler UpdateScoreHandlerEvent;
    public delegate void Update_multiplierHandler(int oldValue, int newValue);
    public event Update_multiplierHandler UpdateMultiplierHandlerEvent;
    public delegate void UpdateMistakeHandler(int oldValue, int newValue);
    public event UpdateMistakeHandler UpdateMistakeHandlerEvent;
    public delegate void UpdateGameSpeedHandler(float speed);
    public event UpdateGameSpeedHandler UpdateGameSpeedHandlerEvent;
    
    public delegate void UpdateGameStateHandler(GameState state);
    public event UpdateGameStateHandler UpdateGameStateHandlerEvent;
    public delegate void UpdateLevelStateHandler(LevelState state);
    public event UpdateLevelStateHandler UpdateLevelStateHandlerEvent;
    
    public float Speed => speed;
    public int mistakes => _mistakes;
    public int multiplier => _multiplier;
    public int score => _score;
    public int lifes => _lifes;
    public GameObject goPlayer => _goPlayer;
    public GameState gameState => _gameState;

    private GameState _gameState;
    private int _mistakes, _multiplier, _score;
    private float speed = 6, savedSpeed;
    private int currentlvl = 1;

    private void Awake()
    {
        Screen.SetResolution(1920, 1080, true);
        this.savedSpeed = this.speed;
        ToggleGameState(GameState.Pause);
        sliderLifes.value = lifes;
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
        this._gameState = state;

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
        if (_gameState == GameState.Game)
        {
            ToggleGameState(GameState.Pause);
            return;
        }

        ToggleGameState(GameState.Game);
    }

    public void IncreaseScore(int count)
    {
        int oldValue = this._score;
        this._score += count;

        UpdateScoreHandlerEvent?.Invoke(oldValue, this._score);
    }

    public void DecreaseScore(int count)
    {
        if (this._score < count)
            return;

        int oldValue = this._score;
        this._score -= count;

        UpdateScoreHandlerEvent?.Invoke(oldValue, this._score);
    }

    public void IncreaseMultiplier(int count)
    {
        int oldValue = this._multiplier;
        this._multiplier += count;

        UpdateMultiplierHandlerEvent?.Invoke(oldValue, this._multiplier);
    }

    public void DecreaseMultiplier(int count)
    {
        if (this._multiplier < count)
            return;

        int oldValue = this._multiplier;
        this._multiplier -= count;

        UpdateMultiplierHandlerEvent?.Invoke(oldValue, this._multiplier);
    }

    public void IncreaseMistakes(int count)
    {
        int oldCount = this._mistakes;
        this._mistakes += count;

        UpdateMistakeHandlerEvent?.Invoke(oldCount, this._mistakes);

        if (this._mistakes >= lifes && lifes < 51)
        {
            LoseLevel();
        }
    }

    public void DecreaseMistake(int count)
    {
        if (this._mistakes < count)
            return;

        int oldCount = this._mistakes;
        this._mistakes -= count;

        UpdateMistakeHandlerEvent?.Invoke(oldCount, this._mistakes);
    }

    public void BreakMultiplier(int position)
    {
        DecreaseMultiplier(this._multiplier);
        IncreaseMistakes(1);

        generator.ReturnQuads(position);
    }

    public void AddScore(int position)
    {
        IncreaseMultiplier(1);
        IncreaseScore(10 * _multiplier);

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

        DecreaseMultiplier(this._multiplier);        

        generator.ReturnToPoolOtherPrefabs();
        currentlvl++;
        this.savedSpeed = this.speed + 4;
        UpdateGameSpeed(this.savedSpeed);
        generator.GenerateLevel(20, 10, currentlvl);
    }

    public void StartGame()
    {
        DecreaseScore(this._score);
        DecreaseMistake(this._mistakes);
        this.savedSpeed = 6;
        UpdateGameSpeed(this.savedSpeed);
        _lifes = (int)sliderLifes.value;
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

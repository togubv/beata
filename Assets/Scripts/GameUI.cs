using System.Collections;
using UnityEngine;
using UnityEngine.UI;

public class GameUI : MonoBehaviour
{
    [SerializeField] private Game game;
    [SerializeField] private Generator generator;

    public GameObject canvasMenu;
    public Text textMultiplier, textScore, textFinishScore, textFinishTitle, textLifes, textMistakesMax, textMistakesCurrent;
    public GameObject goTextMultiplier, goTextScore, panelFinish, uiJoystick, uiJoystickSwitcher, canvasTap;
    public Slider sliderLifes;
    public GameObject buttonContinue, buttonRestart, joystick;
    public Animation animMultiplier, animScore;

    private void OnEnable()
    {
        animMultiplier = goTextMultiplier.GetComponent<Animation>();
        animScore = goTextScore.GetComponent<Animation>();

        game.UpdateScoreHandlerEvent += UpdateScoreUI;
        game.UpdateMultiplierHandlerEvent += UpdateMultiplierUI;

        game.UpdateGameStateHandlerEvent += UpdateGameStateUI;
        game.UpdateLevelStateHandlerEvent += UpdateLevelStateUI;

        OnSliderValueChange();
    }

    private void UpdateGameStateUI(GameState state)
    {
        if (state == GameState.Game)
        {
            canvasMenu.SetActive(false);
            ToggleJoystickUI(true);
            return;
        }

        canvasMenu.SetActive(true);
    }

    private void UpdateLevelStateUI(LevelState state)
    {
        if (state == LevelState.Start)
        {
            panelFinish.SetActive(false);
            return;
        }

        if (state == LevelState.Finish)
        {
            ShowFinishPanel("FINISH!");
            buttonContinue.SetActive(true);
            buttonRestart.SetActive(false);
            return;
        }
        
        if (state == LevelState.Lose)
        {
            ShowFinishPanel("GAME OVER!");
            buttonContinue.SetActive(false);
            buttonRestart.SetActive(true);
            return;
        }
    }
    
    private void ShowFinishPanel(string txt)
    {
        panelFinish.SetActive(true);
        textFinishTitle.text = txt;
        SetText(textFinishScore, game.score.ToString());
        SetText(textMistakesCurrent, game.mistakes.ToString());
        SetText(textMistakesMax, game.lifes.ToString());
        ToggleJoystickUI(false);
    }

    public void UpdateScoreUI(int oldValue, int newValue)
    {
        SetText(textScore, newValue.ToString());

        if (animScore != null)
            animScore.Play("TextUpdateAnimation");
    }

    public void UpdateMultiplierUI(int oldValie, int newValue)
    {
        SetText(textMultiplier, newValue.ToString());

        if (animMultiplier != null)
            animMultiplier.Play("TextUpdateAnimation");
    }

    public void ToggleJoystickUI(bool toggle)
    {
        if (toggle) uiJoystick.SetActive(true);
        else uiJoystick.SetActive(false);
    }

    public void SwitchJoystickPosition()
    {
        Vector2 position_1 = uiJoystick.transform.localPosition;
        Vector2 position_2 = uiJoystickSwitcher.transform.localPosition;

        uiJoystick.transform.localPosition = position_2;
        uiJoystickSwitcher.transform.localPosition = position_1;
    }

    public void StartGameAnimation()
    {
        StartCoroutine(FadeAnim());
    }

    IEnumerator FadeAnim()
    {
        Animator anim = canvasTap.GetComponent<Animator>();
        anim.SetTrigger("Fade");
        canvasTap.GetComponent<Canvas>().enabled = false;
        yield return new WaitForSeconds(1.0f);
        canvasTap.SetActive(false);
        yield return new WaitForSeconds(1.0f);
    }

    public void OnSliderValueChange()
    {
        if (sliderLifes.value > 50)
        {
            SetText(textLifes, "INFINITE");
            return;
        }

        SetText(textLifes, sliderLifes.value.ToString());
    }

    public void Quit()
    {
        Application.Quit();
    }

    private void SetText(Text text, string str)
    {
        text.text = str;
    }
}

public enum JoystickPosition
{
    Right,
    Left
}

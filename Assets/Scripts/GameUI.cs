using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class GameUI : MonoBehaviour
{
    [SerializeField] private Game game;
    [SerializeField] private Generator generator;

    public Text textMultiplier, textScore, textFinishScore;
    public GameObject goTextMultiplier, goTextScore, panelFinish, panelPause, uiJoystick, canvasTap;

    public Animation animMultiplier, animScore;

    private void Start()
    {
        animMultiplier = goTextMultiplier.GetComponent<Animation>();
        animScore = goTextScore.GetComponent<Animation>();
    }

    public void RefreshUI(int multiplier, int score, bool anim_multi, bool anim_score)
    {
        textMultiplier.text = multiplier.ToString();
        textScore.text = score.ToString();

        if (anim_multi) animMultiplier.Play("TextScoreAnimation");
        if (anim_score) animScore.Play("TextScoreAnimation");
    }

    public void FinushLevel(int score)
    {
        ToggleUIJoystick(false);
        textFinishScore.text = score.ToString();
        panelFinish.SetActive(true);
    }

    public void StartLevel()
    {
        ToggleUIJoystick(true);
        panelFinish.SetActive(false);
    }

    public void TogglePausePanel(bool toggle)
    {
        if (toggle)
        {
            ToggleUIJoystick(false);
            panelPause.SetActive(true);
            game.GameStatePause();
        }
        else
        {
            ToggleUIJoystick(true);
            panelPause.SetActive(false);
            game.GameStateGame();
        }
    }

    public void ToggleUIJoystick(bool toggle)
    {
        if (toggle) uiJoystick.SetActive(true);
        else uiJoystick.SetActive(false);
    }

    public void GameMenuContinue()
    {
        TogglePausePanel(false);
    }

    public void GameMenuExit()
    {
        Debug.Log("EXIT");
    }

    public void ToMainMenu()
    {
        // menu menu scene
    }

    public void StartNextLevel()
    {
        game.RestartGame();
    }

    public void StartGameAnimation()
    {
        StartCoroutine(FadeAnim());
    }

    IEnumerator FadeAnim()
    {
        Animator anim = canvasTap.GetComponent<Animator>();
        anim.SetTrigger("Fade");
        yield return new WaitForSeconds(0.5f);
        canvasTap.SetActive(false);
        generator.GenerateLevel(20, 10, 1); // default (x, 20, 10);
        yield return new WaitForSeconds(1.0f);
        game.GameStateGame();
    }

    public void Quit()
    {
        Application.Quit();
    }
}

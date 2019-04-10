using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MenuButton : MonoBehaviour
{
    protected static bool enable = true;
    [SerializeField]
    private GameObject optionPanel = null;
    [SerializeField]
    private GameObject howToPanel = null;
    
    
    public static void SetButtonEnable(bool flag)
    {
        enable = flag;
    }
    
    public void OpenOptionPanel()
    {
        PlayerController.Pause();
        optionPanel.SetActive(true);
    }

    public void CloseOptionPanel()
    {

        PlayerController.PauseEnd();
        optionPanel.SetActive(false);
    }

    public void OpenHowToPanel()
    {
        PlayerController.Pause();
        howToPanel.SetActive(true);
    }

    public void CloseHowToPanel()
    {

        PlayerController.PauseEnd();
        howToPanel.SetActive(false);
    }

    public void PhaseEnd()
    {
        if (!enable)
            return;

        if (!PhaseManager.IsYourTurn())
            return;

        PhaseManager.PhaseTrans();
    }

    public void Resign()
    {
        if (!enable)
            return;

        GameManager.GameEnd(PlayerController.playerColor);
    }
}

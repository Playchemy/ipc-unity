using System.Collections.Generic;
using System.Collections;
using UnityEngine;
using Vuforia;

public class AppState_ViewInAR : State
{
    public GameObject background = null;
    public UnityEngine.UI.Button arButton = null;
    public List<UnityEngine.UI.Image> characterImages;
    public List<SpriteRenderer> arSprites;
    public SpriteHandler_UI characterSpritehandler = null;
    public SpriteHandler_UI arSpritehandler = null;
    public VuforiaBehaviour vuforiaBehaviour = null;

    // State Functions =========================================================================================================
    public override void EnterState()
    {
        TurnOnAR();
    }

    public override void UpdateState()
    {

    }

    public override void ExitState()
    {

    }
    // =========================================================================================================================

    public void TurnOnAR()
    {
        vuforiaBehaviour.enabled = true;
        VuforiaRuntime.Instance.InitVuforia();
        VuforiaRenderer.Instance.Pause(false);

        for (int i = 0; i < characterImages.Count; i++)
        {
            arSprites[i].sprite = characterImages[i].sprite;
            arSprites[i].color = characterImages[i].color;
        }

        arSpritehandler.transform.parent.gameObject.SetActive(true);
        arSpritehandler.skinData = characterSpritehandler.skinData;
        arSpritehandler.clothesData = characterSpritehandler.clothesData;
        arSpritehandler.hairData = characterSpritehandler.hairData;
        arSpritehandler.accessoryData = characterSpritehandler.accessoryData;

        background.SetActive(false);
        arButton.gameObject.SetActive(true);
    }

    public void TurnOffAR()
    {
        VuforiaRenderer.Instance.Pause(true);
        arSpritehandler.transform.parent.gameObject.SetActive(false);

        background.SetActive(true);
        arButton.gameObject.SetActive(false);

        AppStateManager.Instance.SetState(AppStateManager.AppState.InitCharacterSheet);
    }
}

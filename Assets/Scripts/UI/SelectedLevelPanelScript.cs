using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
public class SelectedLevelPanelScript : MonoBehaviour
{
    public System.Action<GameObject> LevelBoughtEvent;
    public System.Action<GameObject> RunLevelEvent;
    public System.Action BackButtonTapEvent;
    private GameObject selectedLevel;
    public GameObject buyButton;
    public GameObject buildbutton;
    public Image levelMinimap;

    public void Initialize(GameObject levelArg, bool isBought)
    {
        SnapshotCreatorScript snapshotCreatorScript = levelArg.GetComponent<SnapshotCreatorScript>();
        levelMinimap.sprite = snapshotCreatorScript.GetLevelSprite();
        selectedLevel = levelArg;
        if (isBought)
        {
            buyButton.SetActive(false);
            buildbutton.SetActive(true);
        }else
        {
            buyButton.SetActive(true);
            buildbutton.SetActive(false);
        }
    }

    public void OnBuyButtonTap()
    {
        LevelBoughtEvent(selectedLevel);
    }

    public void OnBuildButtonTap()
    {
        RunLevelEvent(selectedLevel);
    }
        
    public void OnBackButtonTap()
    {
        BackButtonTapEvent();
    }
}

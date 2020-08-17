using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class OpenMapEvent: IEvent{
    public GameObject levelGO;
    public OpenMapEvent(GameObject levelGOArg)
    {
        this.levelGO = levelGOArg;
    }
}

public class SelectedLevelPanelScript : MonoBehaviour
{
    public System.Action<GameObject> LevelBoughtEvent;
    public System.Action<GameObject> LevelRemoveEvent;
    public System.Action BackButtonTapEvent;
    private GameObject selectedLevel;
    public GameObject buyButton;
    public GameObject buildbutton;
    public Image levelMinimap;
    public Image backgroundImage;
    public GameObject noBlocksPopup;
    bool isInventoryEmpty;

    public void Initialize(GameObject levelArg, bool isBoughtArg, bool isInventoryEmptyArg = false)
    {
        isInventoryEmpty = isInventoryEmptyArg;
        MapTextureDrawer snapshotCreatorScript = levelArg.GetComponent<MapTextureDrawer>();
        levelMinimap.sprite = snapshotCreatorScript.GetLevelSprite();
        backgroundImage.sprite = snapshotCreatorScript.GetBackgroundSprite();
        selectedLevel = levelArg;
        if (isBoughtArg)
        {
            buyButton.SetActive(false);
            buildbutton.SetActive(true);
        }
        else
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
        if (isInventoryEmpty)
        {
            noBlocksPopup.SetActive(true);
        }
        else
        {
            EventManager.SendEvent(new OpenMapEvent(selectedLevel));
        }
    }

    public void OnRemoveButtonTap()
    {
        LevelRemoveEvent(selectedLevel);
    }
        
    public void OnBackButtonTap()
    {
        BackButtonTapEvent();
    }
}

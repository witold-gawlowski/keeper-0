using System.Collections;
using System.Collections.Generic;
using UnityEngine;


public class GemShopUIScript : MonoBehaviour
{
    public GameObject buttonPrefab;
    public BlockButtonScript buttons;
    public GameObject buttonsParentI;
    public GameObject buttonsParentII;

    private void Awake()
    {
        EventManager.AddListener<UpdateGemShopUIEvent>(HandleGemShopUIUpdateEvent);
    }

    public void HandleGemShopUIUpdateEvent(IEvent eventArg)
    {
        UpdateGemShopUIEvent gemShopAwakeEvent = eventArg as UpdateGemShopUIEvent;
        List<Card> cardsTemp = gemShopAwakeEvent.cards;
        RepopulateShopUI(cardsTemp);
    }

    public void RepopulateShopUI(List<Card> cardsArg)
    {
        ClearButtons();
        int parentIchildCount = 0;
        foreach (Card cTemp in cardsArg)
        {
            if (parentIchildCount < 3)
            {
                CreateButton(cTemp, buttonsParentI);
                parentIchildCount++;
            }
            else
            {
                CreateButton(cTemp, buttonsParentII);
            }
        }
    }


    void ClearButtons()
    {
        foreach(Transform tLocal in buttonsParentI.transform)
        {
            Destroy(tLocal.gameObject);
        }
        foreach (Transform tLocal in buttonsParentII.transform)
        {
            Destroy(tLocal.gameObject);
        }
    }

    void CreateButton(Card cardArg, GameObject targetParent)
    {
        GameObject newBlockButton = Instantiate(buttonPrefab, targetParent.transform);
        BlockButtonScript newBlockButtonScript = newBlockButton.GetComponent<BlockButtonScript>();
        Sprite spriteTemp = BlockCodexScript.instance.GetSpriteForPrefab(cardArg.block); 
        newBlockButtonScript.InitializeGemShopButton(spriteTemp, cardArg);
    }


}

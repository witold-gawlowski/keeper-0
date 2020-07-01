using System.Collections;
using System.Collections.Generic;
using UnityEngine;


public class GemShopUIScript : MonoBehaviour
{
    public GameObject buttonPrefab;
    public BlockButtonScript buttons;
    public GameObject buttonsParent;

    private void Awake()
    {
        EventManager.AddListener<UpdateGemShopUIEvent>(HandleGemShopUIUpdateEvent);
    }

    public void HandleGemShopUIUpdateEvent(IEvent eventArg)
    {
        UpdateGemShopUIEvent gemShopAwakeEvent = eventArg as UpdateGemShopUIEvent;
        List<Card> cardsTemp = gemShopAwakeEvent.cards;
        ClearButtons();
        foreach(Card  cTemp in cardsTemp)
        {
            CreateButton(cTemp);
        }
    }

    void ClearButtons()
    {
        foreach(Transform tLocal in buttonsParent.transform)
        {
            Destroy(tLocal.gameObject);
        }
    }

    void CreateButton(Card cardArg)
    {
        GameObject newBlockButton = Instantiate(buttonPrefab, buttonsParent.transform);
        BlockButtonScript newBlockButtonScript = newBlockButton.GetComponent<BlockButtonScript>();
        Sprite spriteTemp = BlockCodexScript.instance.GetSpriteForPrefab(cardArg.block); 
        newBlockButtonScript.InitializeGemShopButton(spriteTemp, cardArg);
    }


}

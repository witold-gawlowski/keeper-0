using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DeckEditUIScript : MonoBehaviour
{
    public GameObject buttonPrefab;
    public GameObject inventoryButtonsParent;
    public GameObject deckButtonsParent;

    void Awake()
    {
        EventManager.AddListener<UpdateInventoryUIEvent>(UpdateInventoryUIEventDispatcher);
        EventManager.AddListener<UpdateDeckUIEvent>(UpdateDeckUIEventDispatcher);
    }


    void ClearButtons(GameObject parentObject)
    {
        foreach (Transform tLocal in parentObject.transform)
        {
            Destroy(tLocal.gameObject);
        }
    }

    public void UpdateDeckUIEventDispatcher(IEvent eventArg)
    {
        UpdateDeckUIEvent deckAwakeEvent = eventArg as UpdateDeckUIEvent;
        List<Card> cardsTemp = deckAwakeEvent.cards;
        ClearButtons(deckButtonsParent);
        foreach (Card cTemp in cardsTemp)
        {
            CreateButton(cTemp, deckButtonsParent, true);
        }
    }

    public void UpdateInventoryUIEventDispatcher(IEvent eventArg)
    {
        UpdateInventoryUIEvent inventoryAwakeEvent = eventArg as UpdateInventoryUIEvent;
        List<Card> cardsTemp = inventoryAwakeEvent.cards;
        ClearButtons(inventoryButtonsParent);
        foreach (Card cTemp in cardsTemp)
        {
            CreateButton(cTemp, inventoryButtonsParent, false);
        }
    }

    void CreateButton(Card cardArg, GameObject parentArg, bool isDeck)
    {
        GameObject newBlockButton = Instantiate(buttonPrefab, parentArg.transform);
        BlockButtonScript newBlockButtonScript = newBlockButton.GetComponent<BlockButtonScript>();
        Sprite spriteTemp = BlockCodexScript.instance.GetSpriteForPrefab(cardArg.block);
        if (isDeck)
        {
            newBlockButtonScript.InitializeDeckButton(spriteTemp, cardArg);
        }
        else
        {
            newBlockButtonScript.InitializeInventoryIButton(spriteTemp, cardArg);
        }
    }



}

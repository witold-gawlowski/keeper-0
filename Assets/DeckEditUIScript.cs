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
        EventManager.AddListener<InventoryAwakeEvent>(HandleInventoryAwake);
        EventManager.AddListener<DeckAwakeEvent>(HandleDeckAwake);
    }

    public void HandleDeckAwake(IEvent eventArg)
    {
        DeckAwakeEvent deckAwakeEvent = eventArg as DeckAwakeEvent;
        List<Card> cardsTemp = deckAwakeEvent.cards;
        foreach (Card cTemp in cardsTemp)
        {
            CreateButton(cTemp, deckButtonsParent);
        }
    }

    public void HandleInventoryAwake(IEvent eventArg)
    {
        InventoryAwakeEvent inventoryAwakeEvent = eventArg as InventoryAwakeEvent;
        List<Card> cardsTemp = inventoryAwakeEvent.cards;
        foreach (Card cTemp in cardsTemp)
        {
            CreateButton(cTemp, inventoryButtonsParent);
        }
    }

    void CreateButton(Card cardArg, GameObject parentArg)
    {
        GameObject newBlockButton = Instantiate(buttonPrefab, parentArg.transform);
        BlockButtonScript newBlockButtonScript = newBlockButton.GetComponent<BlockButtonScript>();
        Sprite spriteTemp = BlockCodexScript.instance.GetSpriteForPrefab(cardArg.block);
        newBlockButtonScript.InitializeGemShopButton(spriteTemp, cardArg);
    }



}

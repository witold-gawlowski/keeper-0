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
        EventManager.AddListener<GemShopAwakeEvent>(HandleGemShopAwake);
    }

    public void HandleGemShopAwake(IEvent eventArg)
    {
        GemShopAwakeEvent gemShopAwakeEvent = eventArg as GemShopAwakeEvent;
        List<Card> cardsTemp = gemShopAwakeEvent.cards;
        foreach(Card  cTemp in cardsTemp)
        {
            CreateButton(cTemp);
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

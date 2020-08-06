using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ShopCardSoldEvent:IEvent
{
    public Card card;
    public ShopCardSoldEvent(Card cArg)
    {
        card = cArg;
    }
}

public class GemShopUIScript : MonoBehaviour
{
    public NoBlockInfoPanelScript purchaseConfirmationScript;
    public GameObject buttonPrefab;
    public GameObject cathegoryPrefab;
    public GameObject cathegoryParent;
    public Dictionary<string, Transform> cathegoryDict;

    public void Awake()
    {
        purchaseConfirmationScript.gameObject.SetActive(false);
        EventManager.AddListener<ShopCardTappedEvent>(ShopCardTappedEventHandler);
    }

    public void ShopCardTappedEventHandler(IEvent evArg)
    {
        ShopCardTappedEvent evData = evArg as ShopCardTappedEvent;
        purchaseConfirmationScript.Init(()=>SignalPurchaseEvent(evData.card));
        purchaseConfirmationScript.gameObject.SetActive(true);
    }

    public void SignalPurchaseEvent(Card  cArg)
    {
        EventManager.SendEvent(new ShopCardSoldEvent(cArg));
    }

    public void CreateButtons(List<Card> cards)
    {
        cathegoryDict = new Dictionary<string, Transform>();
        foreach(Card c in cards)
        {
            string newID = c.cathegoryID;
            Transform newButtonParent = null;
            if (!cathegoryDict.ContainsKey(newID))
            {
                GameObject newCathegoryGO = Instantiate(cathegoryPrefab, cathegoryParent.transform);
                Transform buttonsParent = newCathegoryGO.transform.Find("ButtonsParent");
                if (buttonsParent != null)
                {
                    cathegoryDict.Add(newID, buttonsParent);
                }
                else
                {
                    Debug.Log("Couldn'd find ButtonsParent!");
                }
            }
            newButtonParent = cathegoryDict[newID];
            CreateButton(c, newButtonParent.gameObject);
        }
    }

    public void UpdateButtonDisability(int gems)
    {
        Debug.Log("updateButton disability");
        foreach (Transform cathegoryTransform in cathegoryDict.Values)
        {
            foreach (Transform t in cathegoryTransform)
            {
                BlockButtonScript bbs = t.gameObject.GetComponent<BlockButtonScript>();
                if (bbs != null)
                {
                    int gemCost = bbs.associatedCard.gemCost;
                    bool disabilityCondition = gemCost > gems;
                    bbs.SetDisabled(disabilityCondition);
                }
            }
        }
    }
    
    void CreateButton(Card card, GameObject targetParent)
    {
        GameObject newBlockButton = Instantiate(buttonPrefab, targetParent.transform);
        BlockButtonScript newBlockButtonScript = newBlockButton.GetComponent<BlockButtonScript>();
        Sprite spriteTemp = BlockCodexScript.instance.GetSpriteForPrefab(card.block); 
        newBlockButtonScript.InitializeGemShopButton(spriteTemp, card);
    }


}

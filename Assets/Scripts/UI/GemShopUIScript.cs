using System.Collections;
using System.Collections.Generic;
using UnityEngine;


public class GemShopUIScript : MonoBehaviour
{
    public GameObject buttonPrefab;
    public GameObject cathegoryPrefab;
    public GameObject cathegoryParent;
    public Dictionary<string, Transform> cathegoryDict;

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
                cathegoryDict.Add(newID, newCathegoryGO.transform); 
            }
            newButtonParent = cathegoryDict[newID].Find("ButtonsParent");
            CreateButton(c, newButtonParent.gameObject);
        }
    }

    public void UpdateButtonDisability(int gems)
    {
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

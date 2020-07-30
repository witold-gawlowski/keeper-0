using System.Collections;
using System.Collections.Generic;
using UnityEngine;


public class GemShopUIScript : MonoBehaviour
{
    public GameObject buttonPrefab;
    public GameObject buttonsParent;

    public void CreateButtons(List<Card> cards)
    {
        foreach(Card c in cards)
        {
            CreateButton(c, buttonsParent);
        }
    }

    public void UpdateButtonDisability(int gems)
    {
        foreach (Transform t in buttonsParent.transform)
        {
            BlockButtonScript bbs = t.gameObject.GetComponent<BlockButtonScript>();
            int gemCost = bbs.associatedCard.gemCost;
            bool disabilityCondition = gemCost > gems;
            bbs.SetDisabled(disabilityCondition);
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

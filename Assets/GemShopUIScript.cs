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

        StartCoroutine(asd(cardsTemp));
    }

    public IEnumerator asd(List<Card> cardsTemp)
    {
        ClearButtons();
        yield return new WaitForEndOfFrame();
        foreach (Card cTemp in cardsTemp)
        {
            CreateButton(cTemp);
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

    void CreateButton(Card cardArg)
    {
        Transform targetParent = buttonsParentI.transform;
        if (buttonsParentI.transform.childCount >= 3)
        {
            targetParent = buttonsParentII.transform;
        }
        GameObject newBlockButton = Instantiate(buttonPrefab, targetParent);
        BlockButtonScript newBlockButtonScript = newBlockButton.GetComponent<BlockButtonScript>();
        Sprite spriteTemp = BlockCodexScript.instance.GetSpriteForPrefab(cardArg.block); 
        newBlockButtonScript.InitializeGemShopButton(spriteTemp, cardArg);
    }


}

using System.Collections;
using System.Collections.Generic;
using UnityEngine;


[System.Serializable]
[CreateAssetMenu(fileName = "Card", menuName = "ScriptableObjects/Card", order = 1)]
public class Card: ScriptableObject
{
    public static string[] cathegoryNames = {"Baby", "Adept", "\"Bring it on.\"", "\"Hurt me plenty.\"" , "Classic", "Thesaurus", "Ifinity"};
    public enum Cathegory {Baby, Adept, Bring_It_On, Hurt_MePlenty, Classic, Thesaurus, Infinity};
    public string id {
        get { return name;}
    }
    public int gemCost;
    public GameObject block;
    public int cashCost;
    public int quantity;
    public Cathegory cathegoryID;
}

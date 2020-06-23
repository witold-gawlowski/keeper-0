using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EventManager : MonoBehaviour
{
    public delegate void OnInventoryBlockTap(GameObject blockObjectArg);
    public static event OnInventoryBlockTap onInventoryBlockTap;
    public static void RaiseOnInventoryBlockTap(GameObject blockObjectArg)
    {
        if (onInventoryBlockTap != null)
        {
            onInventoryBlockTap(blockObjectArg);
        }
    }

    public delegate void OnBlockDeleted(GameObject blockObjectArg, int blockCountArg);
    public static event OnBlockDeleted onBlockDeleted;
    public static void RaiseOnBlockDeleted(GameObject blockObjectArg, int blockCountArg)
    {
        if(onBlockDeleted != null)
        {
            onBlockDeleted(blockObjectArg, blockCountArg);
        }
    }

    public delegate void OnDeleteblockButtonTap(GameObject blockObjectArg);
    public static event OnDeleteblockButtonTap OnDeleteBlockButtonTap;
    public static void RaiseOnRemoveBlockInstanceDelegate(GameObject blockObjectArg)
    {
        if (OnDeleteBlockButtonTap != null)
        {
            OnDeleteBlockButtonTap(blockObjectArg);
        }
    }
}
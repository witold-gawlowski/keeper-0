using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EventManager : MonoBehaviour
{

    private void Awake()
    {
        genericEvents = new Dictionary<System.Type, EventDelegate>();
        listeners = new Dictionary<EventDelegate, EventDelegate>();
    }

    public class IEvent { }
    public delegate void EventDelegate(IEvent e);

    Dictionary<System.Type, EventDelegate> genericEvents;
    Dictionary<EventDelegate, EventDelegate> listeners;

    public void AddListener<T>(EventDelegate listener) where T : IEvent{
        EventDelegate del = (e) => listener((T) e);
        genericEvents[typeof(T)]  += del;
        listeners[listener] = del;
    }

    public void SendEvent(IEvent e)
    {
        EventDelegate a = genericEvents[e.GetType()];
        foreach (EventDelegate k in a.GetInvocationList())
        {
            k.Invoke(e);
        }
    }


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
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class IEvent { }

public class EventManager : MonoBehaviour
{
    
    public delegate void EventDelegate(IEvent e);

    static Dictionary<System.Type, EventDelegate> genericEvents = new Dictionary<System.Type, EventDelegate>();
    static Dictionary<EventDelegate, EventDelegate> listeners = new Dictionary<EventDelegate, EventDelegate>();

    public static EventManager instance;


    private void Awake()
    {
        if(instance == null)
        {
            instance = this;
        }
        else
        {
            Destroy(this.gameObject);
        }
    }


    public static void AddListener<T>(EventDelegate listener) where T : IEvent{
        EventDelegate del = (e) => listener((T) e);
        if (!genericEvents.ContainsKey(typeof(T)))
        {
            genericEvents.Add(typeof(T), del);
        }
        else
        {
            genericEvents[typeof(T)] += del;
        }
        listeners[listener] = del;
    }

    public static void SendEvent(IEvent e)
    {
        EventDelegate a = genericEvents[e.GetType()];
        foreach (EventDelegate k in a.GetInvocationList())
        {
            k.Invoke(e);
        }
    }

    public static  void Clear()
    {
        genericEvents = new Dictionary<System.Type, EventDelegate>();
        listeners = new Dictionary<EventDelegate, EventDelegate>();
        onInventoryBlockTap = null;
        onBlockDeleted = null;
        OnDeleteBlockButtonTap = null;
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
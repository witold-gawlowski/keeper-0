using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RunResultScript : MonoBehaviour
{
    public int gems = 0 ;
    public int runNumber = -1;
    public bool completed = false ;
    public static RunResultScript instance;
    private void Awake()
    {
        if (instance == null)
        {
            instance = this;
        }
        else
        {
            Destroy(this.gameObject);
        }

        EventManager.AddListener<RunFinishedEvent>(instance.RunFinishedEventDispatcher);
    }

    void RunFinishedEventDispatcher(IEvent evArg)
    {
        RunFinishedEvent evData = evArg as RunFinishedEvent;
        gems = evData.gems;
        runNumber = evData.runNumber;
        completed = evData.completed;
    }
}

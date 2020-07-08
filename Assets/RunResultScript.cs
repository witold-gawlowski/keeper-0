using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RunResultScript : MonoBehaviour
{
    public int gems;
    public int runNumber;
    public bool completed;
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

        EventManager.AddListener<RunFinishedEvent>(RunFinishedEventDispatcher);
    }

    void RunFinishedEventDispatcher(IEvent evArg)
    {
        RunFinishedEvent evData = evArg as RunFinishedEvent;
        gems = evData.gems;
        runNumber = evData.runNumber;
        completed = evData.completed;
    }
}

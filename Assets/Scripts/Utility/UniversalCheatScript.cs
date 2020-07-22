using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;
public class UniversalCheatScript : MonoBehaviour
{
    public UnityEvent fireRunCompleteCheat;
    public float timer;
    bool used;

    private void Awake()
    {
        timer = 0;
        used = false;
    }

    private void OnEnable()
    {
        timer = 0;
        used = false;
    }

    public void OnButtonTap()
    {
        timer += 1.2f;
    }

    private void Update()
    {
        if (timer > 0)
        {
            if (used == false && timer > 3)
            {
                fireRunCompleteCheat.Invoke();
                used = true;
            }
            timer -= Time.deltaTime;
        }
    }
}

using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LevelCameraFitteer : MonoBehaviour
{
    Map procMap;
    public Camera myCamera;
    public void Setup(GameObject levelObjectArg)
    {
        procMap = levelObjectArg.GetComponent<Map>();
        Vector3 levelCenter = procMap.GetLevelCenterPosition();
    }
}

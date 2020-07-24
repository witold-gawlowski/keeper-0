using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LevelCameraFitteer : MonoBehaviour
{
    ProceduralMap procMap;
    public Camera myCamera;
    public void Setup(GameObject levelObjectArg)
    {
        procMap = levelObjectArg.GetComponent<ProceduralMap>();
        Vector3 levelCenter = procMap.GetLevelCenterPosition();
    }
}

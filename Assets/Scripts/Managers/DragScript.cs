using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

class GemsFoundEvent:IEvent{public int count;public GemsFoundEvent(int countArg){count = countArg;}}
public class DragScript : MonoBehaviour
{

    public delegate void BlockPlacedDelegate(int blockValue, int blockArea);
    public BlockPlacedDelegate blockPlacedEvent;


    // Start is called before the first frame update
    BlockScript draggedBlockScript;
    public BlockShuffleContainer blockFeederScript;
    public BlockSpawnerScript blockSpawnerScript;
    public LevelMoneyManagerScript levelMoneyManagerScript;
    public BuildingUIScript buildingUI;
    public DynamicColorScript dynamicColorScript;
    public BlockUIQueue blockUIQueue;
    public bool useMouse = true;
    bool snapped;
    bool snappedLastFrame;
    public ProceduralMap map;
    public bool firstBlockPlaced;
    public float rotateTimeWindow = 0.5f;


    public void SetProceduralMap(GameObject levelInstanceObject)
    {
        map = levelInstanceObject.GetComponent<ProceduralMap>();
    }

    public void OnStartBuilding()
    {
        firstBlockPlaced = false;
        dynamicColorScript.Init();
    }

    public void OnBlockPlaced(int A, int B, int gemsFound, Vector2Int snappedBlockPosition)
    {
        blockFeederScript.Pop();
        blockSpawnerScript.ResetRotation();
        levelMoneyManagerScript.AddBlockValue(A, B);
        float completedFraction = levelMoneyManagerScript.GetCompletedFraction();
        float completedTrheshold = levelMoneyManagerScript.completionThreshold;
        buildingUI.OnProgressUpdate(completedFraction / completedTrheshold);
        buildingUI.UpdateBarCaption(Mathf.RoundToInt(completedFraction * 100), Mathf.RoundToInt(completedTrheshold * 100));
        levelMoneyManagerScript.CheckCompleteness();
        dynamicColorScript.RegisterTiles(draggedBlockScript.relativeTilePositions, snappedBlockPosition);
        EventManager.SendEvent(new GemsFoundEvent(gemsFound));
    }

    public void OmgDragDown()
    {
        int pointerID = 0;
        if (!Input.GetMouseButton(0))
        {
            pointerID = Input.GetTouch(0).fingerId;
        }
        if (!EventSystem.current.IsPointerOverGameObject(pointerID))
        {
            GameObject draggedBlock = null;
            if (blockFeederScript.Top() != null)
            {
                blockUIQueue.SetTopVisible(false);
                blockSpawnerScript.SpawnNextBlock();
                draggedBlock = blockSpawnerScript.nextBlock;
                draggedBlockScript = draggedBlock.GetComponent<BlockScript>();
                dynamicColorScript.SetBlock(draggedBlock);
                dynamicColorScript.enabled = true;
            }
        }
    }

    // Update is called once per frame
    void Update()
    {
        Touch[] touches = Input.touches;
        if (touches.Length > 0 || Input.GetMouseButton(0))
        {
            Touch mainTouch = new Touch();
            Vector3 newPosition;
            if (touches.Length != 0)
            {
                mainTouch = touches[0];
                newPosition = Camera.main.ScreenToWorldPoint(mainTouch.position);
            }
            else
            {
                newPosition = Camera.main.ScreenToWorldPoint(Input.mousePosition);
            }

            newPosition.z = 0;
            if ((!useMouse && mainTouch.phase == TouchPhase.Began) || Input.GetMouseButtonDown(0))
            {
               //omgdragdown fun goes here 
            }


            snapped = false;
            if (draggedBlockScript != null)
            {
                Vector3 fingerShiftedNewPosition = newPosition + new Vector3(-3, 3) - draggedBlockScript.geometricMiddlePosition;
                draggedBlockScript.transform.position = fingerShiftedNewPosition;
                Vector2Int snappedPointerPosition = new Vector2Int(Mathf.RoundToInt(fingerShiftedNewPosition.x), Mathf.RoundToInt(fingerShiftedNewPosition.y));
                bool blockIsDigger = draggedBlockScript.isDigger;
                bool blockIsStarter = draggedBlockScript.isStarter;
                if (map.AreFree(draggedBlockScript.relativeTilePositions, snappedPointerPosition, firstBlockPlaced && !blockIsStarter, blockIsDigger))
                {
                    draggedBlockScript.transform.position = new Vector3(snappedPointerPosition.x, snappedPointerPosition.y);
                    snapped = true;
                    draggedBlockScript.ChangeColorToSnappedColor();
                    dynamicColorScript.enabled = false;
                }
                if (snappedLastFrame == true && !snapped)
                {
                    dynamicColorScript.enabled = true;
                }
                if(snappedLastFrame == false && snapped)
                {
                    dynamicColorScript.enabled = false;
                }
            }
            snappedLastFrame = snapped;
        }
        if (((touches.Length > 0 && touches[0].phase == TouchPhase.Ended)||Input.GetMouseButtonUp(0)) && draggedBlockScript != null)
        {
            if (snapped)
            {
                Vector2Int snappedBlockPosition = new Vector2Int(
                    Mathf.RoundToInt(draggedBlockScript.transform.position.x),
                    Mathf.RoundToInt(draggedBlockScript.transform.position.y));
                int gemsFound = map.CountGems(draggedBlockScript.relativeTilePositions, snappedBlockPosition);
                map.Block(draggedBlockScript.relativeTilePositions, snappedBlockPosition);
                if (!firstBlockPlaced)
                {
                    firstBlockPlaced = true;
                }
                
                OnBlockPlaced(draggedBlockScript.value, draggedBlockScript.GetArea(), gemsFound, snappedBlockPosition);
                
                draggedBlockScript = null;
            }
            else
            {
                //if (tapLengthCounter < rotateTimeWindow)
                //{
                //    buildingUI.TriggerRotateEvent();
                //}
                blockUIQueue.SetTopVisible(true);
                Destroy(draggedBlockScript.gameObject);
                draggedBlockScript.transform.position = blockSpawnerScript.transform.position;
                draggedBlockScript = null;
            }
            dynamicColorScript.enabled = false;
        }

    }
}

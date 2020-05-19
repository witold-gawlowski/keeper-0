using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DragScriptBACKUP : MonoBehaviour
{

    public delegate void BlockPlacedDelegate(int blockValue);
    public BlockPlacedDelegate blockPlacedEvent;
    

    // Start is called before the first frame update
    BlockScript draggedBlockScript;
    public BlockShuffleContainer blockShuffleContainer;
    public BlockSpawnerScript blockSpawnerScript;
    public LevelMoneyManagerScript levelMoneyManagerScript;
    public BuildingUIScript buildingUI;

    bool snapped;
    private ProceduralMap map;
    bool firstBlockPlaced;

    public void SetProceduralMap(GameObject levelInstanceObject)
    {
        map = levelInstanceObject.GetComponent<ProceduralMap>();
    }

    public void OnStartBuilding()
    {
        firstBlockPlaced = false;
    }

    public void HandleRotButtonTap()
    {
        if (Input.GetMouseButton(0))
        {
            if(draggedBlockScript != null)
            {
                draggedBlockScript.Rotate();
                snapped = false;
            }
        }
    }

    // Update is called once per frame
    void Update()
    {
        if (Input.GetMouseButton(0))
        {
            Vector3 newPosition = Camera.main.ScreenToWorldPoint(Input.mousePosition);
            newPosition.z = 0;
            if (Input.GetMouseButtonDown(0))
            {
                GameObject draggedBlock = null;
                if (blockShuffleContainer.Top() != null) {
                    buildingUI.SetRotateButtonEnabled(true);
                    blockSpawnerScript.SpawnNextBlock();
                    draggedBlock = blockSpawnerScript.nextBlock;
                    draggedBlockScript = draggedBlock.GetComponent<BlockScript>();
                }                
            }

            snapped = false;
            if (draggedBlockScript != null)
            {
                Vector3 fingerShiftedNewPosition = newPosition + new Vector3(-3, 3);
                draggedBlockScript.transform.position = fingerShiftedNewPosition;
                Vector2Int snappedPointerPosition = new Vector2Int(Mathf.RoundToInt(fingerShiftedNewPosition.x), Mathf.RoundToInt(fingerShiftedNewPosition.y));
                if (map.AreFree(draggedBlockScript.relativeTilePositions, snappedPointerPosition, firstBlockPlaced))
                {
                    draggedBlockScript.transform.position = new Vector3(snappedPointerPosition.x, snappedPointerPosition.y);
                    snapped = true;
                }
            }
        }
        if (Input.GetMouseButtonUp(0) && draggedBlockScript != null)
        {
            buildingUI.SetRotateButtonEnabled(false);
            if (snapped)
            {
                Vector2Int snappedBlockPosition = new Vector2Int(
                    Mathf.RoundToInt(draggedBlockScript.transform.position.x),
                    Mathf.RoundToInt(draggedBlockScript.transform.position.y));
                map.Block(draggedBlockScript.relativeTilePositions, snappedBlockPosition);
                if (!firstBlockPlaced)
                {
                    firstBlockPlaced = true;
                }
                blockPlacedEvent(draggedBlockScript.value);
                draggedBlockScript = null;
                blockShuffleContainer.Pop();
            }
            else
            {
                Destroy(draggedBlockScript.gameObject);
                draggedBlockScript.transform.position = blockSpawnerScript.transform.position;
                draggedBlockScript = null;
            }
        }

    }
}

using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DragScript : MonoBehaviour
{

    public delegate void BlockPlacedDelegate(int blockValue);
    public BlockPlacedDelegate blockPlacedEvent;
    

    // Start is called before the first frame update
    BlockScript draggedBlockScript;
    public BlockFeederScript blockFeederScript;
    public BlockSpawnerScript blockSpawnerScript;
    public LevelMoneyManagerScript levelMoneyManagerScript;

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
                if (blockFeederScript.Top() != null) {
                    draggedBlock = blockSpawnerScript.nextBlock;
                    draggedBlockScript = draggedBlock.GetComponent<BlockScript>();
                }                
            }

            snapped = false;
            if (draggedBlockScript != null)
            {
                draggedBlockScript.transform.position = newPosition;
                Vector2Int snappedPointerPosition = new Vector2Int(Mathf.RoundToInt(newPosition.x), Mathf.RoundToInt(newPosition.y));
                if (map.AreFree(draggedBlockScript.relativeTilePositions, snappedPointerPosition, firstBlockPlaced))
                {
                    draggedBlockScript.transform.position = new Vector3(snappedPointerPosition.x, snappedPointerPosition.y);
                    snapped = true;
                }
                if (Input.GetKeyDown(KeyCode.R))
                {
                    draggedBlockScript.Rotate();
                    snapped = false;
                }
            }
        }
        if (Input.GetMouseButtonUp(0) && draggedBlockScript != null)
        {
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
                blockFeederScript.Pop();
                blockSpawnerScript.SpawnNextBlock();
            }
            else
            {
                draggedBlockScript.transform.position = blockSpawnerScript.transform.position;
                draggedBlockScript = null;
            }
        }

    }
}

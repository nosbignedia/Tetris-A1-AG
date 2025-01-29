using System.Collections;
using System.Collections.Generic;
using System.ComponentModel;
using System.Runtime.InteropServices.WindowsRuntime;
using UnityEngine;

public class TetrisPiece : MonoBehaviour
{
    private TetrisGrid grid;
    private float dropTimer;
    private float dropInterval = 1;


    // Start is called before the first frame update
    void Start()
    {
        grid = FindObjectOfType<TetrisGrid>();
        dropTimer = dropInterval;
    }

    // Update is called once per frame
    void Update()
    {
        HandleAutomaticDrop();

        if (Input.GetKeyDown(KeyCode.LeftArrow))
        {
            Move(Vector3.left);
        }

        if (Input.GetKeyDown(KeyCode.RightArrow))
        {
            Move(Vector3.right);
        }

        if (Input.GetKeyDown(KeyCode.DownArrow))
        {
            Move(Vector3.down);
        }

        if (Input.GetKeyDown(KeyCode.UpArrow))
        {
           RotatePiece();
        }

    }

    public void Move(Vector3 direction)
    {
        transform.position += direction;

        if(!IsValidPosition())
        {
            transform.position -= direction; // back if not valid

            if(direction == Vector3.down)
            {
                LockPiece();
            }
        }

    }

    private void RotatePiece()
    {
        transform.Rotate(0, 0, -90);


        if(!IsValidPosition()) {
            {
                transform.Rotate(0, 0, 90); //flips back if filled
            } }
    }


    private bool IsValidPosition()
    {
        foreach(Transform block in transform)
        {
            Vector2Int position = Vector2Int.RoundToInt(block.position);

            if(grid.IsCellOccupied(position))
            {
                return false;
            }
        }
        return true;
    } 

    private void HandleAutomaticDrop()
    {
        dropTimer -= Time.deltaTime;

        if(dropTimer <= 0)
        {
            Move(Vector3.down);
            dropTimer = dropInterval;
        }
    }

    private void LockPiece()
    {
        foreach(Transform block in transform) 
        {
            Vector2Int position = Vector2Int.RoundToInt(block.position);   
            grid.AddBlockToGrid(block, position);
        }

        grid.ClearFullLines();
        if(FindObjectOfType<Grid>().isActiveAndEnabled)
        {
            FindObjectOfType<TetrisSpawner>().SpawnPiece();
        }
        
        Destroy(this);

    }
   

}

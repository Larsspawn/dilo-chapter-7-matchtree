using System.Collections;
using UnityEngine;
using UnityEngine.Events;

public class Tile : MonoBehaviour
{
    private Vector3 firstPosition;
    private Vector3 finalPosition;
    private float swipeAngle;

    private Vector3 tempPosition;

    public float xPosition;
    public float yPosition;
    public int column;
    public int row;
    private Grid grid;
    private GameObject otherTile;
    private int previousColumn;
    private int previousRow;

    public bool isMatched;

    public UnityEvent OnSwipe;
    public UnityEvent OnSwipeFailed;

    private void Start()
    {
        //Menentukan posisi dari tile
        grid = FindObjectOfType<Grid>();
        xPosition = transform.position.x;
        yPosition = transform.position.y;
        column = Mathf.RoundToInt((xPosition - grid.startPos.x) / grid.offset.x);
        row = Mathf.RoundToInt((yPosition - grid.startPos.y) / grid.offset.x);
    }
    
    private void Update()
    {
        if (isMatched)
        {
            SpriteRenderer sprite = GetComponent<SpriteRenderer>();
            sprite.color = Color.grey;
        }

        xPosition = (column * grid.offset.x) + grid.startPos.x;
        yPosition = (row * grid.offset.y) + grid.startPos.y;

        SwipeTile();
    }

    private void OnMouseDown()
    {
        if (GameManager.instance.onGameOver || GameManager.instance.isPaused)
            return;

        firstPosition = Camera.main.ScreenToWorldPoint(Input.mousePosition);
    }

    private void OnMouseUp()
    {
        if (GameManager.instance.onGameOver || GameManager.instance.isPaused)
            return;

        finalPosition = Camera.main.ScreenToWorldPoint(Input.mousePosition);
        
        if (!GameManager.instance.onSwapCooldown )
            CalculateAngle();
    }

    private void CalculateAngle()
    {
        swipeAngle = Mathf.Atan2(finalPosition.y - firstPosition.y, finalPosition.x - firstPosition.x) * 180 / Mathf.PI;
        
        OnSwipe.Invoke();

        MoveTile();
    }

    private void SwipeTile()
    {
        if (Mathf.Abs(xPosition - transform.position.x) > .1)
        {
            //Move towards the target
            tempPosition = new Vector2(xPosition, transform.position.y);
            transform.position = Vector2.Lerp(transform.position, tempPosition, .4f);
        }
        else
        {
            //Directly set the position
            tempPosition = new Vector2(xPosition, transform.position.y);
            transform.position = tempPosition;
            grid.tiles[column, row] = this.gameObject;
        }

        if (Mathf.Abs(yPosition - transform.position.y) > .1)
        {
            //Move towards the target
            tempPosition = new Vector2(transform.position.x, yPosition);
            transform.position = Vector2.Lerp(transform.position, tempPosition, .4f);
        }
        else
        {
            //Directly set the position
            tempPosition = new Vector2(transform.position.x, yPosition);
            transform.position = tempPosition;
            grid.tiles[column, row] = this.gameObject;
        }

        CheckMatches();
    }
    
    private void MoveTile()
    {
        if (GameManager.instance.onGameOver || GameManager.instance.isPaused)
            return;

        previousColumn = column;
        previousRow = row;

        if (swipeAngle > -45 && swipeAngle <= 45)           // Right
        {
            SwipeRightMove();
        }
        else if (swipeAngle > 45 && swipeAngle <= 135)      // Up
        {
            SwipeUpMove();
        }
        else if (swipeAngle > 135 || swipeAngle <= -135)    // Left
        {
            SwipeLeftMove();
        }
        else if (swipeAngle < -45 && swipeAngle >= -135)    // Down
        {
            SwipeDownMove();
        }

        StartCoroutine(checkMove());
    }
    
    private void SwipeRightMove()
    {
        if (column + 1 < grid.gridSizeX)
        {
            otherTile = grid.tiles[column + 1, row];
            otherTile.GetComponent<Tile>().column -= 1;
            column += 1;
        }
    }

    private void SwipeUpMove()
    {
        if (row + 1 < grid.gridSizeY)
        {
            otherTile = grid.tiles[column, row + 1];
            otherTile.GetComponent<Tile>().row -= 1;
            row += 1;
        }
    }

    private void SwipeLeftMove()
    {
        if (column - 1 >= 0)
        {
            otherTile = grid.tiles[column - 1, row];
            otherTile.GetComponent<Tile>().column += 1;
            column -= 1;
        }
    }

    private void SwipeDownMove()
    {
        if (row - 1 >= 0)
        {
            otherTile = grid.tiles[column, row - 1];
            otherTile.GetComponent<Tile>().row += 1;
            row -= 1;
        }
    }

    private void CheckMatches()
    {
        //Check horizontal matching
        if (column > 0 && column < grid.gridSizeX -1)
        {
            //Check left and right side
            GameObject leftTile = grid.tiles[column - 1, row];
            GameObject rightTile = grid.tiles[column + 1, row];
            if(leftTile != null && rightTile != null)
            {
                if (leftTile.CompareTag(gameObject.tag) && rightTile.CompareTag(gameObject.tag))
                {
                    isMatched = true;
                    rightTile.GetComponent<Tile>().isMatched = true;
                    leftTile.GetComponent<Tile>().isMatched = true;

                    if (gameObject.CompareTag("SpecialTile"))
                    {
                        GameObject[] specialTiles = GameObject.FindGameObjectsWithTag("SpecialTile");

                        foreach (GameObject tile in specialTiles)
                        {
                            tile.GetComponent<Tile>().isMatched = true;
                        }
                    }
                }
            }
        }
        //Check vertical matching
        if (row > 0 && row < grid.gridSizeY - 1)
        {
            //Check up and bottom side
            GameObject upTile = grid.tiles[column, row + 1];
            GameObject downTile = grid.tiles[column, row -1];
            if (upTile != null && downTile != null)
            {
                if (upTile.CompareTag(gameObject.tag) && downTile.CompareTag(gameObject.tag))
                {
                    isMatched = true;
                    downTile.GetComponent<Tile>().isMatched = true;
                    upTile.GetComponent<Tile>().isMatched = true;

                    if (gameObject.CompareTag("SpecialTile"))
                    {
                        GameObject[] specialTiles = GameObject.FindGameObjectsWithTag("SpecialTile");

                        foreach (GameObject tile in specialTiles)
                        {
                            tile.GetComponent<Tile>().isMatched = true;
                        }
                    }
                }
            }
        }
    }

    private IEnumerator checkMove()
    {
        GameManager.instance.swapCooldownTimer = 0.5f;
        GameManager.instance.onSwapCooldown = true;

        yield return new WaitForSeconds(.5f);

        if (otherTile != null)
        {
            if (!isMatched && !otherTile.GetComponent<Tile>().isMatched)
            {
                otherTile.GetComponent<Tile>().row = row;
                otherTile.GetComponent<Tile>().column = column;
                row = previousRow;
                column = previousColumn;

                // Reset multiplier back to normal (1)
                GameManager.instance.ResetMultiplier();

                OnSwipeFailed.Invoke();
            }
            else
            {
                grid.DestroyMatches(); 
            }
        }
        otherTile = null;
    }   
}
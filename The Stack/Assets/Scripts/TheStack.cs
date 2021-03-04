using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class TheStack : MonoBehaviour
{
    public Text scoreText;
    public Text scoreTextAtEnd;

    public Material black, white;

    public GameObject endGameObject;

    public int highScore = 0;
    public int score = 0;

    private const float BOUNDS_SIZE = 3.5f;
    private const float STACK_MOVING_SPEED = 4.0f;
    private const float ERROR_MARGIN = .1f;
    private const float STACK_BOUNDS_GAIN = .25f;
    private const int COMBO_GAIN = 3;

    private GameObject[] theStack;

    private Vector2 stackBounds = new Vector2(BOUNDS_SIZE, BOUNDS_SIZE);

    private int stackIndex;
    private int scoreCount = 0;
    private int combo = 0;

    private float tileTransition = 0.0f;
    private float tileSpeed = 2.5f;

    private float secondaryPosition;

    private Vector3 desiredPosition;
    private Vector3 lastTilePosition;

    private bool isMovingOnX = true;
    private bool isGameOver = false;

    void Start()
    {
        theStack = new GameObject[transform.childCount];
        for(int i=0; i<transform.childCount; i++)
        {
            theStack[i] = transform.GetChild(i).gameObject;
        }
        stackIndex = transform.childCount - 1;
        lastTilePosition = transform.GetChild(0).localPosition;

        PlayerPrefs.SetInt("highScore", highScore);
    }


    void Update()
    {
        if (Input.GetMouseButtonDown(0))
        {
            if (PlaceTile())
            {
                scoreCount++;
                SpawnTile();
            }
            else
            {
                EndGame();
            }
            
        }
        MoveTile();

        //Move stack down 
        transform.position = Vector3.Lerp(transform.position, desiredPosition, STACK_MOVING_SPEED * Time.deltaTime);
    }

    private void CreateRubble(Vector3 pos, Vector3 scale, Material material)
    {
        GameObject go = GameObject.CreatePrimitive(PrimitiveType.Cube);

        if(go.GetComponent<MeshRenderer>() != null)
        {
            go.GetComponent<MeshRenderer>().material = material;
        }
        go.transform.localPosition = pos;
        go.transform.localScale = scale;
        go.AddComponent<Rigidbody>();
    }

    private void MoveTile()
    {
        if(isGameOver)
            return;

        tileTransition += Time.deltaTime * tileSpeed;
        if(tileTransition >= 2 * Mathf.PI )
        {
            tileTransition = 0.0f;
        }

        if (isMovingOnX)
        {
            theStack[stackIndex].transform.localPosition = new Vector3(Mathf.Sin(tileTransition) * BOUNDS_SIZE, scoreCount, secondaryPosition);
        }
        else
        {
            theStack[stackIndex].transform.localPosition = new Vector3(secondaryPosition, scoreCount, Mathf.Sin(tileTransition) * BOUNDS_SIZE);   
        }
    }

    private void SpawnTile()
    {
        lastTilePosition = theStack[stackIndex].transform.localPosition;

        Transform lastTile = theStack[stackIndex].transform;

        stackIndex--;
        if (stackIndex < 0)
        {
            stackIndex = transform.childCount - 1;
        }

        desiredPosition = Vector3.down * scoreCount;

        theStack[stackIndex].transform.localScale = lastTile.localScale;

        theStack[stackIndex].transform.localPosition = new Vector3(0, scoreCount, 0);    
    }

    private bool PlaceTile()
    {
        Transform t = theStack[stackIndex].transform;

        if (isMovingOnX)
        {
            float deltaX = lastTilePosition.x - t.position.x;
            if(Mathf.Abs(deltaX) > ERROR_MARGIN)
            {
                //cut the tile
                combo = 0;

                stackBounds.x -= Mathf.Abs(deltaX);
                if (stackBounds.x <= 0f)
                    return false;

                float middle = lastTilePosition.x + t.localPosition.x / 2;
                t.localScale = new Vector3(stackBounds.x, 1f, stackBounds.y);

                CreateRubble(new Vector3((t.position.x > 0)
                    ? t.position.x + (t.localScale.x/2)
                    : t.position.x - (t.localScale.x / 2), t.position.y, t.position.z),
                             new Vector3(Mathf.Abs(deltaX), 1f, t.localScale.z), white);

                t.localPosition = new Vector3(middle - (lastTilePosition.x /2), scoreCount, lastTilePosition.z);
            }
            else
            {
                if(combo > COMBO_GAIN)
                {
                    stackBounds.x += STACK_BOUNDS_GAIN;
                    if (stackBounds.x >= BOUNDS_SIZE)
                        stackBounds.x = BOUNDS_SIZE;

                    float middle = lastTilePosition.x + t.localPosition.x / 2;
                    t.localScale = new Vector3(stackBounds.x, 1f, stackBounds.y);
                    t.localPosition = new Vector3(middle - (lastTilePosition.x / 2), scoreCount, lastTilePosition.z);
                }
                combo++;
                t.localPosition = lastTilePosition + Vector3.up;
            }
        }
        else
        {
            float deltaZ = lastTilePosition.z - t.position.z;
            if (Mathf.Abs(deltaZ) > ERROR_MARGIN)
            {
                //cut the tile
                combo = 0;

                stackBounds.y -= Mathf.Abs(deltaZ);
                if (stackBounds.y <= 0f)
                    return false;

                float middle = lastTilePosition.z + t.localPosition.z / 2;
                t.localScale = new Vector3(stackBounds.x, 1f, stackBounds.y);

                CreateRubble(new Vector3(t.position.x, t.position.y, (t.position.z > 0)
                    ? t.position.z + (t.localScale.z / 2)
                    : t.position.z - (t.localScale.z / 2)),
                             new Vector3(t.localScale.x, 1f, Mathf.Abs(deltaZ)), black);

                t.localPosition = new Vector3(lastTilePosition.x, scoreCount, middle - (lastTilePosition.z / 2));
            }
            else
            {
                if (combo > COMBO_GAIN)
                {
                    stackBounds.y += STACK_BOUNDS_GAIN;
                    if (stackBounds.y >= BOUNDS_SIZE)
                        stackBounds.y = BOUNDS_SIZE;

                    float middle = lastTilePosition.z + t.localPosition.z / 2;
                    t.localScale = new Vector3(stackBounds.x, 1f, stackBounds.y);
                    t.localPosition = new Vector3(lastTilePosition.x, scoreCount, middle - (lastTilePosition.z / 2));
                }
                combo++;
                t.localPosition = lastTilePosition + Vector3.up;
            }
        }

        secondaryPosition = (isMovingOnX)
            ? t.localPosition.x
            : t.localPosition.z;
        isMovingOnX = !isMovingOnX;

        score++;
        scoreText.text = score.ToString();
        PlayerPrefs.SetInt("score", score);
        if (score > highScore)
        {
            PlayerPrefs.SetInt("highScore", score);
        }
        return true;
    }
    private void EndGame()
    {
        endGameObject.SetActive(true);

        scoreTextAtEnd.text = PlayerPrefs.GetInt("score").ToString();

        isGameOver = true;

        if(theStack[stackIndex].GetComponent<Rigidbody>() == null)
            theStack[stackIndex].AddComponent<Rigidbody>();
    }
}

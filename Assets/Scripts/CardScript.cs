using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
public class CardScript : MonoBehaviour
{
    public float zOffset = -1f;

    public bool isPlaced { get; private set; }
    public bool teleportBack { get; private set; }
    public bool isOnPile;
    public bool isFromPacket;
    public bool isBackCard;

    public AudioManagerScript audioManager;

    private const char DELIMITER = '_';

    public Rank RankValue { get; private set; }
    public Suit SuitValue { get; private set; }

    public int rank;
    private int suit;

    //private Stack<GameObject> cardStack;
    private GameObject pile;
    private DrawScript drawScript;
    private GameManager gameManager;
    private Vector3 touchPosition;
    private Vector3 offset;
    private Vector3 originalPosition;

    private void Awake()
    {
        string[] parts = name.Split(DELIMITER);
        parts[1] = parts[1].Replace("(Clone)", "");

        if (parts.Length != 2)
            throw new ArgumentException("Invalid card name: " + name);

        if (!Enum.TryParse(parts[0], result: out Suit suit_Awake))
            throw new ArgumentException("Invalid rank: " + parts[0]);

        if (!Enum.TryParse(parts[1], result: out Rank rank_Awake))
            throw new ArgumentException("Invalid suit: " + parts[1]);

        RankValue = rank_Awake;
        SuitValue = suit_Awake;
        rank = (int)RankValue;
        suit = (int)SuitValue;
        isOnPile = false;
        isPlaced = false;
        teleportBack = false;
        isFromPacket = false;
    }
    private void Start()
    {
        originalPosition = transform.position;
        audioManager = GameObject.Find("Audio Manager").GetComponent<AudioManagerScript>();
        drawScript = GameObject.Find("Packet").GetComponent<DrawScript>();
        gameManager = GameObject.Find("Game Manager").GetComponent<GameManager>();
        //cardStack = new Stack<GameObject>();
        //cardStack.Push(gameObject);
    }

    public int GetRank() => rank;

    public GameObject GetAttachedPile() => pile;

    public PileScript GetPileScriptForAttachedPile() => pile.GetComponent<PileScript>();

    public void SetAttachedPile(GameObject pile) => this.pile = pile;

    public void SetPosition()
    {
        if (!isPlaced)
        {
            transform.position = new Vector3(transform.position.x, transform.position.y, (float)rank / 100);
        }
        else
        {
            transform.position = new Vector3(transform.position.x, transform.position.y, 0.5f - (float)rank / 100);
        }
    }
    private void OnMouseDown()
    {
        if (!isPlaced && !isBackCard)
        {
            touchPosition = Camera.main.ScreenToWorldPoint(Input.mousePosition);
            offset = transform.position - touchPosition;

            //originalPosition = transform.position;
            transform.position = new Vector3(transform.position.x, transform.position.y, (float)rank / 100);

            if (isOnPile)
            {
                gameManager.TransferCardsToCardStack(gameObject, pile.GetComponent<PileScript>());
                //cardStack = pile.GetComponent<PileScript>().RemoveCards(rank);
            }
            audioManager.PlayCardPickup();
        }
    }

    private void OnMouseDrag()
    {
        if (!isPlaced && !isBackCard)
        {
            touchPosition = Camera.main.ScreenToWorldPoint(Input.mousePosition);
            transform.position = new Vector3(touchPosition.x + offset.x, touchPosition.y + offset.y, transform.position.z);

            if (isOnPile)
            {
                //int tempY = 0;
                //GameObject obj;
                //Stack<GameObject> tempStack = new Stack<GameObject>(cardStack);

                //while (tempStack.Count > 0)
                //{
                //    obj = tempStack.Pop();
                //    obj.transform.position = new Vector3(transform.position.x, transform.position.y - ((float)tempY) / 5, obj.transform.position.z);
                //    tempY++;
                //}
                gameManager.MoveCardStack();
            }
        }
    }
    //private void Place(Stack<GameObject> stack)
    //{
    //    int tempY = 0;
    //    GameObject obj;
    //    Stack<GameObject> tempStack = new Stack<GameObject>(stack);

    //    while (tempStack.Count > 0)
    //    {
    //        obj = tempStack.Pop();
    //        obj.transform.position = new Vector3(transform.position.x, transform.position.y - ((float)tempY) / 5, obj.transform.position.z);
    //        tempY++;
    //    }
    //}

    private void OnMouseUp()
    {
        if (!isBackCard)
        {
            touchPosition = Vector3.zero;
            offset = Vector3.zero;

            Vector3 size = gameObject.GetComponent<BoxCollider2D>().bounds.size;
            Collider2D[] overlaps = Physics2D.OverlapBoxAll(gameObject.transform.position, size, 0f);


            if (overlaps.Length > 1) // Ignore self-collision
            {
                foreach (Collider2D overlap in overlaps)
                {
                    if (overlap.gameObject == gameObject)
                    {
                        continue;
                    }

                    RowsScript rowScript = overlap.gameObject.GetComponent<RowsScript>();
                    PileScript pileScript = overlap.gameObject.GetComponent<PileScript>();
                    CardScript cardScript = overlap.gameObject.GetComponent<CardScript>();

                    if (rowScript && rowScript.CanPlace(rank, suit))
                    {
                        isPlaced = true;
                        isOnPile = false;
                        teleportBack = false;

                        if (isFromPacket)
                        {
                            drawScript.drawnCardsStack.Pop();
                        }
                        if (!isFromPacket && GetPileScriptForAttachedPile())
                        {
                            GetPileScriptForAttachedPile().CheckForCards();
                        }

                        isFromPacket = false;
                        SetPosition();
                        Destroy(this);
                        break;
                    }
                    else if (pileScript && pileScript.rankValue == 13 && rank == 13 && pileScript.pileStack.Count == 0)
                    {
                        pileScript.AddCards(gameManager.cardStack);
                        if (isFromPacket)
                        {
                            SetPosition();
                            drawScript.drawnCardsStack.Pop();
                        }
                        isOnPile = true;
                        teleportBack = false;
                        //transform.position = overlap.transform.position;

                        if (!isFromPacket && GetPileScriptForAttachedPile())
                        {
                            GetPileScriptForAttachedPile().CheckForCards();
                        }

                        isFromPacket = false;

                        SetAttachedPile(overlap.gameObject);
                        gameManager.TransferCardsToPileStack(pileScript);
                        break;
                    }
                    else if (cardScript && cardScript != gameObject.GetComponent<CardScript>() && cardScript.isOnPile && !cardScript.isBackCard &&
                        cardScript.rank - 1 == rank && cardScript.GetPileScriptForAttachedPile().pileStack.Peek().Equals(cardScript.gameObject))
                    {
                        Debug.Log("card peste card");
                        //transform.position = new Vector3(overlap.transform.position.x, overlap.transform.position.y - 0.2f, (float)rank / 100);
                        gameManager.TransferCardsToPileStack(cardScript.GetPileScriptForAttachedPile());
                        //cardScript.GetPileScriptForAttachedPile().AddCards(rank, cardStack);
                        if (isFromPacket)
                        {
                            SetPosition();
                            drawScript.drawnCardsStack.Pop();
                        }
                        isFromPacket = false;
                        isOnPile = true;
                        teleportBack = false;

                        if (this.pile != null && this.pile.GetComponent<PileScript>())
                        {
                            GetPileScriptForAttachedPile().CheckForCards();
                        }
                        this.pile = cardScript.GetAttachedPile();
                        break;
                    }
                    else
                    {
                        if(cardScript && cardScript != gameObject.GetComponent<CardScript>())
                        {
                            //Debug.Log("cardscript rank: " + cardScript.rank + "card rank: " + rank);
                        }
                        teleportBack = true;
                    }
                }
            }
            else
            {
                teleportBack = true;
            }

            if (teleportBack)
            {
                //transform.position = originalPosition;
                //Debug.Log("before" + this.pile.GetComponent<PileScript>().pileStack.Count);
                if (!isFromPacket)
                {
                    gameManager.TransferCardsToPileStack(GetPileScriptForAttachedPile());
                    //GetPileScriptForAttachedPile().AddCards(rank, cardStack);
                    //Debug.Log("after" + this.pile.GetComponent<PileScript>().pileStack.Count);
                    //Place(cardStack);
                }
                teleportBack = false;
                audioManager.PlayError();
            }
            else
            {
                audioManager.PlayCardPutDown();
            }

        }

    }
}

using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
public class CardScript : MonoBehaviour
{
    public bool IsPlaced { get; private set; }
    public bool TeleportBack { get; private set; }
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
        ParseCardName();
        rank = (int)RankValue;
        suit = (int)SuitValue;
        isOnPile = false;
        IsPlaced = false;
        TeleportBack = false;
        isFromPacket = false;
    }
    private void Start()
    {
        originalPosition = transform.position;
        audioManager = GameObject.Find("Audio Manager").GetComponent<AudioManagerScript>();
        drawScript = GameObject.Find("Packet").GetComponent<DrawScript>();
        gameManager = GameObject.Find("Game Manager").GetComponent<GameManager>();
    }

    private void ParseCardName()
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
    }

    public int GetRank() => rank;

    public GameObject GetAttachedPile() => pile;

    public PileScript GetPileScriptForAttachedPile() => pile.GetComponent<PileScript>();

    public void SetAttachedPile(GameObject pile) => this.pile = pile;

    public void SetPosition()
    {
        if (!IsPlaced)
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
        if (!IsPlaced && !isBackCard)
        {
            touchPosition = Camera.main.ScreenToWorldPoint(Input.mousePosition);
            offset = transform.position - touchPosition;
            transform.position = new Vector3(transform.position.x, transform.position.y, (float)rank / 100);

            if (isOnPile)
            {
                gameManager.TransferCardsToCardStack(gameObject, pile.GetComponent<PileScript>());
            }
            audioManager.PlayCardPickup();
        }
    }

    private void OnMouseDrag()
    {
        if (!IsPlaced && !isBackCard)
        {
            touchPosition = Camera.main.ScreenToWorldPoint(Input.mousePosition);
            transform.position = new Vector3(touchPosition.x + offset.x, touchPosition.y + offset.y, transform.position.z);

            if (isOnPile)
            {
                gameManager.MoveCardStack();
            }
        }
    }

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

                    if (rowScript && rowScript.CanPlace(rank, suit) && gameManager.cardStack.Count == 1)
                    {
                        //IsPlaced = true;
                        //isOnPile = false;
                        //TeleportBack = false;

                        //if (isFromPacket)
                        //{
                        //    drawScript.drawnCardsStack.Pop();
                        //}
                        //if (!isFromPacket && GetPileScriptForAttachedPile())
                        //{
                        //    GetPileScriptForAttachedPile().CheckForCards();
                        //}

                        //isFromPacket = false;
                        //transform.position = overlap.transform.position;
                        //SetPosition();
                        //gameManager.cardStack.Pop();
                        //Destroy(this);
                        HandleValidPlacement(rowScript, overlap);
                        break;
                    }
                    else if (pileScript && rank == 13 && pileScript.pileStack.Count == 0)
                    {
                        //Debug.Log("gol");
                        //if (this.pile != null && this.pile.GetComponent<PileScript>())
                        //{
                        //    GetPileScriptForAttachedPile().CheckForCards();
                        //}

                        //gameManager.SetAttachedPileForCardStack(overlap.gameObject);
                        //gameManager.TransferCardsToPileStack(pileScript);
                        //if (isFromPacket)
                        //{
                        //    SetPosition();
                        //    drawScript.drawnCardsStack.Pop();
                        //}
                        //isOnPile = true;
                        //TeleportBack = false;

                        //if (!isFromPacket && GetPileScriptForAttachedPile())
                        //{
                        //    GetPileScriptForAttachedPile().CheckForCards();
                        //}

                        //isFromPacket = false;

                        HandleKingPlacement(pileScript, overlap);
                        break;
                    }
                    else if (cardScript && cardScript != gameObject.GetComponent<CardScript>() && cardScript.isOnPile && !cardScript.isBackCard &&
                        cardScript.rank - 1 == rank && cardScript.GetPileScriptForAttachedPile().pileStack.Peek().Equals(cardScript.gameObject))
                    {
                        //if (this.pile != null && this.pile.GetComponent<PileScript>())
                        //{
                        //    GetPileScriptForAttachedPile().CheckForCards();
                        //}
                        //gameManager.SetAttachedPileForCardStack(cardScript.GetAttachedPile());
                        //gameManager.TransferCardsToPileStack(cardScript.GetPileScriptForAttachedPile());

                        //if (isFromPacket)
                        //{
                        //    SetPosition();
                        //    drawScript.drawnCardsStack.Pop();
                        //}
                        //isFromPacket = false;
                        //isOnPile = true;
                        //TeleportBack = false;
                        HandleSequentialPlacement(cardScript, overlap);
                        break;
                    }
                    else
                    {
                        TeleportBack = true;
                    }
                }
            }
            else
            {
                TeleportBack = true;
            }

            if (TeleportBack)
            {
                HandleTeleportBack();
                //if (!isFromPacket)
                //{
                //    gameManager.TransferCardsToPileStack(GetPileScriptForAttachedPile());
                //}
                //else
                //{
                //    transform.position = originalPosition;
                //}
                //TeleportBack = false;
                //audioManager.PlayError();
            }
            else
            {
                audioManager.PlayCardPutDown();
            }

        }

    }
    private void HandleValidPlacement(RowsScript rowScript, Collider2D overlap)
    {
        IsPlaced = true;
        isOnPile = false;
        TeleportBack = false;

        if (isFromPacket)
        {
            drawScript.drawnCardsStack.Pop();
        }
        if (!isFromPacket && GetPileScriptForAttachedPile())
        {
            GetPileScriptForAttachedPile().CheckForCards();
        }

        isFromPacket = false;
        transform.position = overlap.transform.position;
        SetPosition();
        gameManager.cardStack.Pop();
        Destroy(this);
    }

    private void HandleKingPlacement(PileScript pileScript, Collider2D overlap)
    {
        if (this.pile != null && this.pile.GetComponent<PileScript>())
        {
            GetPileScriptForAttachedPile().CheckForCards();
        }

        gameManager.SetAttachedPileForCardStack(overlap.gameObject);
        gameManager.TransferCardsToPileStack(pileScript);
        if (isFromPacket)
        {
            SetPosition();
            drawScript.drawnCardsStack.Pop();
        }
        isOnPile = true;
        TeleportBack = false;

        if (!isFromPacket && GetPileScriptForAttachedPile())
        {
            GetPileScriptForAttachedPile().CheckForCards();
        }

        isFromPacket = false;
    }

    private void HandleSequentialPlacement(CardScript cardScript, Collider2D overlap)
    {
        if (this.pile != null && this.pile.GetComponent<PileScript>())
        {
            GetPileScriptForAttachedPile().CheckForCards();
        }
        gameManager.SetAttachedPileForCardStack(cardScript.GetAttachedPile());
        gameManager.TransferCardsToPileStack(cardScript.GetPileScriptForAttachedPile());

        if (isFromPacket)
        {
            SetPosition();
            drawScript.drawnCardsStack.Pop();
        }
        isFromPacket = false;
        isOnPile = true;
        TeleportBack = false;
    }

    private void HandleTeleportBack()
    {
        if (!isFromPacket)
        {
            gameManager.TransferCardsToPileStack(GetPileScriptForAttachedPile());
        }
        else
        {
            transform.position = originalPosition;
        }
        TeleportBack = false;
        audioManager.PlayError();
    }

}


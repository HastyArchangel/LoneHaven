using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PileScript : MonoBehaviour
{
    public int rankValue;
    public Stack<GameObject> pileStack;
    //private Stack<GameObject> helperStack;
    public PileEnum PileValue { get; private set; }

    [SerializeField] GameObject backCard;
    GameManager gameManager;

    private int pileNr;
    private int spawnVariable;
    private int zOffSet;
    int randomSpawner;

    private void Awake()
    {
        if (!Enum.TryParse(name, result: out PileEnum pile_Awake))
        {
            throw new ArgumentException("Invalid pile: " + name);
        }
        PileValue = pile_Awake;
        pileNr = (int)PileValue;
    }

    private void Start()
    {
        pileStack = new Stack<GameObject>();
        //backCardStack = new Stack<GameObject>();
        //helperStack = new Stack<GameObject>();
        spawnVariable = 0;
        zOffSet = 1;
        gameManager = GameObject.Find("Game Manager").GetComponent<GameManager>();
        GenerateCards();
        //gameManager.AddFromStack(helperStack);
    }
    private void GenerateBackCards(int nr)
    {
        GameObject card;
        CardScript playingCardScript;
        while (nr > 0)
        {
            randomSpawner = (int)UnityEngine.Random.Range(0, gameManager.cardList.Count);
            card = Instantiate(gameManager.cardList[randomSpawner], GetCardPosition(spawnVariable, zOffSet), gameManager.cardList[randomSpawner].transform.rotation);
            card.transform.GetChild(0).gameObject.SetActive(false);
            playingCardScript = card.GetComponent<CardScript>();
            playingCardScript.isOnPile = true;
            playingCardScript.SetAttachedPile(this.gameObject);
            playingCardScript.isBackCard = true;
            gameManager.cardList.RemoveAt(randomSpawner);
            pileStack.Push(card);
            spawnVariable++;
            zOffSet++;
            nr--;
        }
    }

    private void GenerateGeneralCard()
    {
        GameObject card;
        CardScript playingCardScript;
        randomSpawner = (int)UnityEngine.Random.Range(0, gameManager.cardList.Count);
        card = Instantiate(gameManager.cardList[randomSpawner], GetCardPosition(spawnVariable, 0f),
            gameManager.cardList[randomSpawner].transform.rotation);
        card.transform.GetChild(1).gameObject.SetActive(false);
        // Deactivate backcard 

        playingCardScript = card.GetComponent<CardScript>();
        playingCardScript.isOnPile = true;
        playingCardScript.isBackCard = false;
        playingCardScript.SetPosition();
        playingCardScript.SetAttachedPile(this.gameObject);
        pileStack.Push(card);
        rankValue = playingCardScript.GetRank();
        gameManager.cardList.RemoveAt(randomSpawner);
    }
    private void GenerateCards()
    {
        //GameObject card;
        //while (pileNr > 0)
        //{
        //    randomSpawner = (int)UnityEngine.Random.Range(0, gameManager.cardList.Count);
        //    card = Instantiate(backCard, GetCardPosition(spawnVariable, zOffSet), backCard.transform.rotation);
        //    card.GetComponent<BackCardScript>().card = gameManager.cardList[randomSpawner];
        //    card.GetComponent<BackCardScript>().SetPile(this.gameObject);
        //    gameManager.cardList.RemoveAt(randomSpawner);
        //    helperStack.Push(card.GetComponent<BackCardScript>().card);
        //    backCardStack.Push(card);
        //    spawnVariable++;
        //    zOffSet++;
        //    pileNr--;
        //}
        //randomSpawner = (int)UnityEngine.Random.Range(0, gameManager.cardList.Count);
        //card = Instantiate(gameManager.cardList[randomSpawner], GetCardPosition(spawnVariable, 0f),
        //    gameManager.cardList[randomSpawner].transform.rotation);
        //card.GetComponent<CardScript>().SetPosition();
        //card.GetComponent<CardScript>().SetAttachedPile(this.gameObject);
        //card.GetComponent<CardScript>().isOnPile = true;
        //pileStack.Push(card);
        //helperStack.Push(gameManager.cardList[randomSpawner]);
        //rankValue = card.GetComponent<CardScript>().GetRank();
        //gameManager.cardList.RemoveAt(randomSpawner);
        GenerateBackCards(pileNr);
        GenerateGeneralCard();
    }

    private Vector3 GetCardPosition(int spawnVariable, float zOffset)
    {
        return transform.position - new Vector3(0f, (float)spawnVariable / 5, zOffset / 1000);
    }

    public void CheckForCards()
    {

        if (pileStack.Count == 0)
        {
            rankValue = 13;
        }
        else if (pileStack.Peek().transform.GetChild(1) == true)
        {
            GameObject card = pileStack.Peek();
            card.transform.GetChild(1).gameObject.SetActive(false);
            card.transform.GetChild(0).gameObject.SetActive(true);
            card.GetComponent<CardScript>().isBackCard = false;
        }
    }

    public void AddCards(Stack<GameObject> cardStack)
    {
        Stack<GameObject> tempStack = new(cardStack);
        while (tempStack.Count > 0)
        {
            pileStack.Push(tempStack.Pop());
            rankValue--;
        }
    }

    public Stack<GameObject> RemoveCards(int val)
    {
        Stack<GameObject> newCardStack = new();
        while (val > rankValue - 1)
        {
            newCardStack.Push(pileStack.Pop());
            rankValue++;
        }
        return new(newCardStack);
    }
}
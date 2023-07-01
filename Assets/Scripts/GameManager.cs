using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class GameManager : MonoBehaviour
{
    private int score;
    public GameObject[] cards;
    public List<GameObject> cardList;
    public List<GameObject> pileCardList;
    private float zPosition;
    public Stack<GameObject> cardStack;

    private void Awake()
    {
        cardList = new List<GameObject>(cards);
        pileCardList = new List<GameObject>();
        cardStack = new Stack<GameObject>();
    }
    void Start()
    {
        score = 0;
        zPosition = -0.4f;
        
    }
    public float GetZPosition()
    {
        zPosition -= 0.0001f;
        return zPosition;
    }
    public void AddFromStack(Stack <GameObject> concatStack)
    {
        foreach(GameObject obj in concatStack)
        {
            pileCardList.Add(obj);
        }
    }

    public void SetAttachedPileForCardStack(GameObject pile)
    {
        Stack<GameObject> stack = new(cardStack);
        GameObject obj;
        while (stack.Count > 0)
        {
            obj = stack.Pop();
            obj.GetComponent<CardScript>().SetAttachedPile(pile);
        }
    }
    public void TransferCardsToCardStack(GameObject card, PileScript pile)
    {
        Stack<GameObject> stack = pile.pileStack;
        while (stack.Count > 0)
        {
            GameObject obj = stack.Pop();
            cardStack.Push(obj);
            if (obj.Equals(card))
            {
                break;
            }
        }
    }
    public void TransferCardsToPileStack(PileScript pile)
    {
        Stack<GameObject> stack = pile.pileStack;
        while (cardStack.Count > 0)
        {
            GameObject obj = cardStack.Pop();
            float yPosition = pile.transform.position.y - (stack.Count / 5.0f);
            float zPosition = - (1 + stack.Count / 100.0f);
            obj.transform.position = new Vector3(pile.transform.position.x, yPosition, zPosition);
            stack.Push(obj);
        }
    }

    public void MoveCardStack()
    {
        Stack<GameObject> reverseStack = new(cardStack);
        Stack<GameObject> stack = new (reverseStack);
        float tempY = 1.0f;
        float dragZ = -2.0f;
        GameObject first = stack.Pop();
        GameObject obj;
        first.transform.position = new Vector3(first.transform.position.x, first.transform.position.y, dragZ);
        while (stack.Count > 0)
        {
            obj = stack.Pop();
            obj.transform.position = new Vector3(first.transform.position.x, first.transform.position.y - (tempY / 5), dragZ - (tempY / 5));
            tempY++;
        }
    }
    public void UpdateScore()
    {
        score++;
    }

}

using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DrawScript : MonoBehaviour
{
    private Vector3 lastPosition;
    private Vector3 subtrVect;

    public List<GameObject> generationList;
    public Stack<GameObject> drawStack;
    public Stack<GameObject> drawnCardsStack;
    public Stack<GameObject> drawStackCopy;

    private Vector3 backCardSpawnPosition;
    GameManager gameManager;
    [SerializeField] GameObject backCard;

    void Start()
    {
        subtrVect = new Vector3(0f, 0f, 0.001f);
        backCardSpawnPosition = transform.position - subtrVect;
        gameManager = GameObject.Find("Game Manager").GetComponent<GameManager>();
        generationList = new List<GameObject>(gameManager.cards);
        drawStack = new Stack<GameObject>();
        drawnCardsStack = new Stack<GameObject>();
        drawStackCopy = new Stack<GameObject>();
        GenerateList();
        GenerateCards();
        //PlaceCards();
    }

    // Update is called once per frame
    void Update()
    {
        //Debug.Log(drawStack.Count);
    }
    private void GenerateList()
    {
        
        HashSet<GameObject> set2 = new HashSet<GameObject>(gameManager.pileCardList);
        generationList.RemoveAll(element => set2.Contains(element));

    }
    private void GenerateCards()
    {
        List<GameObject> myList = new(generationList);
        while(myList.Count > 0)
        {
            int randomNr = Random.Range(0, myList.Count);
            drawStack.Push(myList[randomNr]);
            myList.RemoveAt(randomNr);
        }
    }

    //private void PlaceCards()
    //{
    //    drawStackCopy = new(drawStack);
    //    while (drawStackCopy.Count > 0)
    //    {
    //        GameObject card = Instantiate(backCard, backCardSpawnPosition, backCard.transform.rotation);
    //        //card.GetComponent<BackCardScript>().card = drawStackCopy.Pop();
    //        //card.GetComponent<BackCardScript>().isFromPacket = true;
    //        drawnCardsStack.Push(card);
    //        backCardSpawnPosition -= subtrVect;
    //    }
    //}

    private void OnMouseDown()
    {
        Debug.Log(drawnCardsStack.Count + " " + drawStack.Count);
        if(drawStack.Count == 0 && drawnCardsStack.Count != 0)
        {
            foreach(GameObject obj in drawnCardsStack)
            {
                drawStack.Push(obj);
            }
            drawnCardsStack = new Stack<GameObject>();
            // PlaceCards();
        } 
    }
}

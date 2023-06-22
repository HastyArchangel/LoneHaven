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

    // Start is called before the first frame update
    private void Awake()
    {
        cardList = new List<GameObject>(cards);
        pileCardList = new List<GameObject>();
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

    public void UpdateScore()
    {
        score++;
    }

}

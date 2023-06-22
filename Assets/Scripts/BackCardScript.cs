using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BackCardScript : MonoBehaviour
{
    public GameObject card;
    public bool isFromPacket;
    private GameObject pile;
    private PileScript pileScript;
    private GameManager gameManager;
    private DrawScript packet;
    // Start is called before the first frame update
    private void Awake()
    {
        isFromPacket = false;
    }
    void Start()
    {
        packet = GameObject.Find("Packet").GetComponent<DrawScript>();
        gameManager = GameObject.Find("Game Manager").GetComponent<GameManager>();
    }
    public GameObject GetPile()
    {
        return this.pile;
    }
    public void SetPile(GameObject pile)
    {
        this.pile = pile;
    }



    public void DestroySelf()
    {
        GameObject obj;
        pileScript = pile.GetComponent<PileScript>();
        obj = Instantiate(this.card, transform.position, this.card.transform.rotation);
        obj.GetComponent<CardScript>().SetPosition();
        obj.GetComponent<CardScript>().SetAttachedPile(pile.gameObject);
        obj.GetComponent<CardScript>().isOnPile = true;
        pileScript.pileStack.Push(obj);
        pileScript.rankValue = obj.GetComponent<CardScript>().rank;
        Destroy(this.gameObject);
    }

    private void OnMouseDown()
    {
        if (isFromPacket)
        {
            GameObject obj;
            obj = Instantiate(this.card, new Vector3(transform.position.x + 2, transform.position.y, gameManager.GetZPosition()), this.card.transform.rotation);
            obj.GetComponent<CardScript>().isFromPacket = true;
            packet.drawStack.Pop();
            Destroy(this.gameObject);
        }
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}

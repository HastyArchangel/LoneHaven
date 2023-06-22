using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RowsScript : MonoBehaviour
{
    public int rankValue;
    public int suitValue;
    // Start is called before the first frame update
    void Start()
    {
        rankValue = 1;
        suitValue = -1;
    }

    public bool CanPlace(int rank, int suit)
    {
        if(rank == rankValue && (suit == suitValue || (suitValue == -1 && rank == 1)))
        {
            rankValue++;
            suitValue = suit;
            return true;
        }    
        return false;
    }


}

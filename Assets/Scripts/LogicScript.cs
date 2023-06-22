using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LogicScript : MonoBehaviour
{

    public LayerMask layerMask;
    RaycastHit2D hitWithMinZ;
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        if (Input.GetMouseButtonDown(0))
        {
            RaycastHit2D[] hits = Physics2D.RaycastAll(Camera.main.ScreenToWorldPoint(Input.mousePosition), Vector2.down, Mathf.Infinity, layerMask);

            // Find the hit with the smallest Z value
            float minZ = float.PositiveInfinity;
            hitWithMinZ = new RaycastHit2D();
            foreach (var hit in hits)
            {
                if (hit.transform != null && hit.transform.position.z < minZ)
                {
                    Debug.Log("succes");
                    hitWithMinZ = hit;
                    minZ = hit.transform.position.z;
                }
            }

            // Set the tag of the game object of the hit with the smallest Z value
            if (hitWithMinZ.transform != null)
            {
                hitWithMinZ.transform.gameObject.tag = "Selected";
            }
        }
        if (Input.GetMouseButtonUp(0))
        {
            if (hitWithMinZ.transform != null)
            {
                hitWithMinZ.transform.gameObject.tag = "Untagged";
            }
        }
            
        //if (Input.GetMouseButtonDown(0))
        //{
        //    RaycastHit2D[] hits = Physics2D.RaycastAll(Camera.main.ScreenToWorldPoint(Input.mousePosition), Vector2.down, Mathf.Infinity, layerMask);

        //    if (hits.Length > 0)
        //    {
        //        for (int i = 0; i < hits.Length; i++)
        //        {
        //            RaycastHit2D hit = hits[i];
        //            Debug.Log("Hit object: " + hit.collider.name);
        //            // Do something with the hit object
        //        }
        //    }
        //Debug.Log("succes");
        //    float closestDistance = Mathf.Infinity;
        //    GameObject closestObject = null;

        //Vector3 size = gameObject.GetComponent<Renderer>().bounds.size;
        //Collider2D[] overlaps = Physics2D.OverlapBoxAll(gameObject.transform.position, size, 0f);

        //foreach (Collider2D overlap in overlaps)
        //{
        //    float distance = Mathf.Abs(overlap.transform.position.z - Camera.main.transform.position.z);
        //    if (distance < closestDistance)
        //    {
        //        closestDistance = distance;
        //        closestObject = overlap.gameObject;
        //    }
        //}

        //if (closestObject != null)
        //{
        //    // Do something with the closest object here
        //    Debug.Log("Closest object is " + closestObject.name);
        //}

        //}
    }

}

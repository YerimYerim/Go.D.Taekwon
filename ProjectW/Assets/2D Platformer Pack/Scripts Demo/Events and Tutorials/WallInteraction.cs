using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WallInteraction : MonoBehaviour
{
    public List<GameObject> targetsRemaining;

    public void OpenWay()
    {
        if (this.name == ("Wall Interactive"))
        {
            gameObject.GetComponent<Animator>().Play("Open");
        }

        if (this.name == ("Wall Interactive 2"))
        {
            gameObject.GetComponent<Animator>().Play("Open 2");
        }
    }

    void Update()
    {

        if (targetsRemaining.Count == 0)
        {
            OpenWay();
            gameObject.GetComponent<WallInteraction>().enabled = false;
        }
        
    }


}

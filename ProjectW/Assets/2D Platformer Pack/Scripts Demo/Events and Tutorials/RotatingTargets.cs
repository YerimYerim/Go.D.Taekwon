using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RotatingTargets : MonoBehaviour
{
    public GameObject target;
    private GameObject targetClone;
    private List<GameObject> spawnedTargets = new List<GameObject>();
    private bool canSpawn;


    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.CompareTag("Player"))
        {
            canSpawn = true;
            gameObject.GetComponent<Collider2D>().enabled = false;
        }
    }


    private void Update()
    {
        if(canSpawn && spawnedTargets.Count < 20)
        {
            StartCoroutine(Spawn());
            canSpawn = false;
        }

        //makes objects spawned rotate
        foreach (GameObject targetClone in spawnedTargets)
        {
            if (targetClone != null)
            {
                targetClone.transform.RotateAround(transform.position, Vector3.forward, 30f * Time.deltaTime);
            }
        }
    }
    private IEnumerator Spawn()
    {
        CreateTargets();

        yield return new WaitForSeconds(0.4f);
        
        canSpawn = true;
        
    }

    private void CreateTargets()
    {
        Vector2 spawnPoint = new Vector2(transform.position.x + 11, transform.position.y + 2);

        targetClone = Instantiate(target, spawnPoint, Quaternion.identity);
        spawnedTargets.Add(targetClone);

    }

}

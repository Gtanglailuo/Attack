using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class Spawen : MonoBehaviour
{
    private List<SpawenPoint> spawenPointList;

    private List<Character> spawnedCharacters;
    private bool hasSpawned;

    public Collider _collider;

    public UnityEvent myEvent;
    private void Awake()
    {
        var spawenPointArray = transform.parent.GetComponentsInChildren<SpawenPoint>();
        spawenPointList = new List<SpawenPoint>(spawenPointArray);
        spawnedCharacters = new List<Character>();
    }
    private void Update()
    {
        if (!hasSpawned || spawnedCharacters.Count==0)
        {
            return;
        }

        bool allSpawnedAreDead = true;

        foreach (var c in spawnedCharacters)
        {
            if (c.CurrentState!=Character.CharacterState.Dead)
            {
                allSpawnedAreDead = false;
                break;
            }
        }
        if (allSpawnedAreDead)
        {
            if (myEvent!=null)
            {
                myEvent.Invoke();
            }

            spawnedCharacters.Clear();
        }

    }
    public void SpawnCharacters()
    {
        if (hasSpawned)
        {
            return;
        }
        hasSpawned = true;

        foreach (var item in spawenPointList)
        {
            if (item.EnemyToSpawn!=null)
            {
                GameObject spawendGameObject = Instantiate(item.EnemyToSpawn,item.transform.position,Quaternion.identity);
                spawnedCharacters.Add(spawendGameObject.GetComponent<Character>());
            }
        }

    
    }
    private void OnTriggerEnter(Collider other)
    {
        if (other.tag=="Player")
        {
            SpawnCharacters();
        }
    }

    private void OnDrawGizmos()
    {
        Gizmos.color = Color.red;
        Gizmos.DrawWireCube(transform.position,_collider.bounds.size);


    }


}

using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DropWeapons : MonoBehaviour
{
    public List<GameObject> Weapons;

    public void DropSwords()
    {
        Debug.Log("���Ѿ�����");
        foreach (GameObject item in Weapons)
        {
            
            item.AddComponent<Rigidbody>();
            item.AddComponent<BoxCollider>();
            item.transform.parent = null;
        }
    

    }


}

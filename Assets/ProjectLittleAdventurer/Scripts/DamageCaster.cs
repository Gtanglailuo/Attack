using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DamageCaster : MonoBehaviour
{
    private Collider _damageCasterCollider;

    public int Damage = 30;

    public string TargetTag;

    private List<Collider> _damageTargetList;

    private void Awake()
    {
        _damageCasterCollider = GetComponent<Collider>();
        _damageCasterCollider.enabled = false;
        _damageTargetList = new List<Collider>();
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.tag==TargetTag && !_damageTargetList.Contains(other))
        {
            Character targetCC = other.GetComponent<Character>();

            if (targetCC!=null)
            {
                targetCC.ApplyDamage(Damage,transform.parent.position);

                VFXManger vFXManger = transform.parent.GetComponent<VFXManger>();

                if (vFXManger!=null)
                {
                    RaycastHit hit;

                    Vector3 pos = transform.forward * _damageCasterCollider.bounds.extents.z + transform.position;

                    bool IsHit = Physics.BoxCast(pos, _damageCasterCollider.bounds.extents / 2, transform.forward,
                                                 out hit, transform.rotation,
                                                 _damageCasterCollider.bounds.extents.z, 1 << 6);
                    if (IsHit)
                    {
                        vFXManger.PlaySlash(hit.point);
                    }


                }

            }
            _damageTargetList.Add(other);

        }
    }

    public void EnableDamageCaster()
    {
        _damageTargetList.Clear();
        _damageCasterCollider.enabled = true;
    }

    public void DisableDamageCaster()
    {
        _damageTargetList.Clear();
        _damageCasterCollider.enabled = false;

    }

    private void OnDrawGizmos()
    {
        if (_damageCasterCollider ==null)
        {
            _damageCasterCollider = transform.GetComponent<Collider>();
        }

        RaycastHit hit;

        Vector3 pos = transform.forward * _damageCasterCollider.bounds.extents.z+transform.position;

        bool IsHit = Physics.BoxCast(pos, _damageCasterCollider.bounds.extents/2,transform.forward,
                                     out hit,transform.rotation, 
                                     _damageCasterCollider.bounds.extents.z,1 << 6);

        if (IsHit)
        {

            Gizmos.color = Color.yellow;
            Gizmos.DrawWireSphere(hit.point,0.3f);
        }

    }

}

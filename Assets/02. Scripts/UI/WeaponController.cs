using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WeaponController : MonoBehaviour
{
    [SerializeField]
    float hitBoxDisappearTime;
    [SerializeField]
    GameObject numberHitBox;

    Vector3 target;

    //private void OnCollisionEnter(Collision collision)
    //{
    //    if (collision.gameObject.tag == "Enemy")
    //    {
    //        Debug.Log("Hit");
    //        target = collision.contacts[0].point;
    //        Debug.Log(target);
    //        NumerHitBox();
    //    }
    //}

    private void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.tag == "Enemy")
        {
            Debug.Log("Hit");
            target = this.gameObject.transform.position;
            Debug.Log(target);
            NumerHitBox();
        }
    }

    private void NumerHitBox()
    {
        var numBox = Instantiate(numberHitBox, target, Quaternion.identity);
        Destroy(numBox, hitBoxDisappearTime);
    }
}

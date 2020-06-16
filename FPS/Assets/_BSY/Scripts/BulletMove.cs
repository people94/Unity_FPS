using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BulletMove : MonoBehaviour
{
    // Update is called once per frame
    void Update()
    {
        transform.Translate(transform.forward * 5 * Time.deltaTime);
    }

    private void OnCollisionEnter(Collision collision)
    {
        //gameObject.SetActive(false);
    }
}

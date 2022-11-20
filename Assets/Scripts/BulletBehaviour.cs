using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BulletBehaviour : MonoBehaviour
{
    [SerializeField] private GameObject bulletDecal;
    [SerializeField] private float timeToDestroy = 3.0f;
    [SerializeField] private float speed = 60f;

    public Vector3 target { get; set; }
    public bool hit { get; set; }

    private void OnEnable()
    {
        Destroy(gameObject, timeToDestroy);
    }

    // Update is called once per frame
    void Update()
    {
        transform.position = Vector3.MoveTowards(transform.position, target, speed * Time.deltaTime);
        if (!hit && Vector3.Distance(transform.position, target) < Mathf.Epsilon)
        {
            Destroy(gameObject);
        }
    }

    private void OnCollisionEnter(Collision collision)
    {
        ContactPoint contact = collision.GetContact(0);
        GameObject decal = GameObject.Instantiate(bulletDecal, contact.point + contact.normal * 0.001f, Quaternion.LookRotation(contact.normal));
        Destroy(gameObject);
        //StartCoroutine(DestroyDecal(decal));
    }

    //IEnumerator DestroyDecal(GameObject go)
    //{
    //    Destroy(go);
    //    yield return new WaitForSeconds(2.0f);
    //}
}

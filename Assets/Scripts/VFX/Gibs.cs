using System.Collections.Generic;
using UnityEngine;

public class Gibs : MonoBehaviour
{
    [SerializeField]
    private List<Sprite> gibs;
    [SerializeField]
    private GameObject gibPrefab;

    [SerializeField]
    private float minforce = 10f;
    [SerializeField]
    private float maxForce = 20f;

    public void Explode() 
    {
        int gibAmount = Random.Range((int) gibs.Count/2, gibs.Count);

        for (int i = 0; i < gibAmount; i++) 
        {
            GameObject gib = Instantiate(gibPrefab, transform.position, Quaternion.identity);
            gib.transform.Rotate(0f, 0f, Random.Range(0f, 360f));

            Rigidbody2D rb = gib.GetComponent<Rigidbody2D>();

            rb.AddForce(Random.insideUnitCircle.normalized * Random.Range(minforce, maxForce));
            rb.angularVelocity = Random.Range(-360f, 360f);

            int gibIndex = Random.Range(0, gibs.Count);
            gibPrefab.GetComponent<SpriteRenderer>().sprite = gibs[gibIndex];
            gibs.RemoveAt(gibIndex);
        }
    }
}

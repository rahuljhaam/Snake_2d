using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Burner : MonoBehaviour
{
    public BoxCollider2D gridArea;

    private void Start()
    {
        StartCoroutine(ChangePosition());
    }

    private void RandomizePosition()
    {
        Bounds bounds = this.gridArea.bounds;

        float x = Random.Range(bounds.min.x, bounds.max.x);
        float y = Random.Range(bounds.min.y, bounds.max.y);

        this.transform.position = new Vector3(Mathf.Round(x), Mathf.Round(y), 0.0f);

    }
    private IEnumerator ChangePosition()
    {

        RandomizePosition();
        yield return new WaitForSeconds(Random.Range(8f, 10f));
        StartCoroutine(ChangePosition());
    }

    private void OnTriggerEnter2D(Collider2D other)
    {
        if (other.tag == "Player")
        {
            RandomizePosition();
        }
        else if (other.tag == "Player1")
        {
            RandomizePosition();
        }
    }
}

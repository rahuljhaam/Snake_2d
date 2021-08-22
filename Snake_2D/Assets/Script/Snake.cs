using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;


public class Snake : MonoBehaviour
{
    private Vector2 _direction = Vector2.right;

    private List<Transform> _segements = new List<Transform>();
    public Transform segmentPrefab;

    private Renderer[] renderers;
    private bool isWrappingX = false;
    private bool isWrappingY = false;
    public GameObject gameover;
    



    void Start()
    {
        ResetState();
        renderers = GetComponentsInChildren<Renderer>();
    }

    public int initialSize = 4;

    private void ScreenWrap()
    {
        bool isVisible = CheckRenderers();
        if(isVisible)
        {
            isWrappingX = false;
            isWrappingY = false;
            return;
        }

        if(isWrappingX || isWrappingY)
        {
            return;
        }
        Vector3 newPosition = transform.position;

        if(newPosition.x > 1 || newPosition.x < 0)
        {
            newPosition.x = -newPosition.x ;
            isWrappingX = true;
        }
        if (newPosition.y > 1 || newPosition.y < 0)
        {
            newPosition.y = -newPosition.y ;
            isWrappingY = true;
        }
        transform.position = newPosition;

    }


    bool CheckRenderers()
    {
        foreach (var renderer in renderers)
        {
            // If at least one render is visible, return true
            if (renderer.isVisible)
            {
                return true;
            }
        }

        // Otherwise, the object is invisible
        return false;
    }


    private void Update()
    {

        if (Input.GetKeyDown(KeyCode.W))
        { _direction = Vector2.up; }
        else if (Input.GetKeyDown(KeyCode.S))
        { _direction = Vector2.down; }
        else if (Input.GetKeyDown(KeyCode.A))
        { _direction = Vector2.left; }
        else if (Input.GetKeyDown(KeyCode.D))
        { _direction = Vector2.right; }
    }

    private void FixedUpdate()
    {

        for(int i = _segements.Count - 1; i > 0; i--)
        {
            _segements[i].position = _segements[i - 1].position;
        }

        this.transform.position = new Vector3(
            Mathf.Round(this.transform.position.x) + _direction.x,
            Mathf.Round(this.transform.position.y) + _direction.y,
            0.0f
            );

        ScreenWrap();
    }

    private void ResetState()
    {
        for(int i =1; i< _segements.Count; i++)
        {
            Destroy(_segements[i].gameObject);
        }
        _segements.Clear();
        _segements.Add(this.transform);

        for (int i = 1; i < this.initialSize; i++)
        {
            _segements.Add(Instantiate(this.segmentPrefab));
        }

        this.transform.position = Vector3.zero;

    }

    private void Grow()
    {
        Transform segement = Instantiate(this.segmentPrefab);
        segement.position = _segements[_segements.Count - 1].position;
        _segements.Add(segement);
    }

    private void Reduce()
    {
        Transform segement = _segements[_segements.Count - 1].transform;
        _segements.Remove(segement);
        Destroy(segement.gameObject);
    }



    private void OnTriggerEnter2D(Collider2D other)
    {
        if (other.tag == "Food")
        {
            Grow();
        }else if (other.tag == "Obstacle")
        { GameOver(); }
        else if (other.tag == "Burner")
        {
            if(_segements.Count > 3)
                Reduce(); 
        }
    }
    public void GameOver()
    {
        gameover.SetActive(true);
    }
    public void Reset()
    {
        SceneManager.LoadScene(1);
    }
    public void Menu()
    {
        SceneManager.LoadScene(0);
    }
}

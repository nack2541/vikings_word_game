﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ball : MonoBehaviour
{
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void update()
    {

    }

    private void OnMouseDown() {
        GameObject.Find("GameManager").GetComponent<GameManager>().ScoreUp();
        Destroy(gameObject);
    }
    


}
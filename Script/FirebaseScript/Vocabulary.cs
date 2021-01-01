using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Vocabulary : MonoBehaviour
{
    public string number;
    public string vocabulary;

    public Vocabulary()
    {
        this.number = firebase.vocabNoText;
        this.vocabulary = firebase.vocabText;
    }
}

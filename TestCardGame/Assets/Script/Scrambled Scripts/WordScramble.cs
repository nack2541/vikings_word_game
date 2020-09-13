using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;


[System.Serializable]
public class Word
{
    public string word;
    [Header("leave empty if you want randomized")]
    public string desiredRandom;
    public string GetString()
    {
        if (!string.IsNullOrEmpty(desiredRandom))
        {
            return desiredRandom;
        }
        string result = word;
        while (result == word)
        {
            result = "";
            List<char> charaters = new List<char>(word.ToCharArray());
            //ramdom char
            while (charaters.Count > 0)
            {
                int indexChar = Random.Range(0, charaters.Count - 1);
                result = result + charaters[indexChar];

                charaters.RemoveAt(indexChar);
            }
        }

        return result;
    }

}

public class WordScramble : MonoBehaviour
{
    public Word[] words;

    [Header("UI Referrence")]
    public CharObject prefab;
    public Transform container;
    public GameObject vocabField;
    public Text pointText;
    public float space;
    public float lerpSpeed;

    List<CharObject> charObjects = new List<CharObject>();
    CharObject firstSelected;
    public int currentWord;

    public static WordScramble Instance;

    void Awake()
    {
        Instance = this;
        vocabField.SetActive(false);
    }
    public void ShowVocabField(float Duration)
    {
        StartCoroutine(ShowVocabFieldCoroutine(Duration));
    }

    IEnumerator ShowVocabFieldCoroutine(float Duration)
    {
        //Debug.Log("Showing some message. Duration: " + Duration);
        vocabField.SetActive(true);

        yield return new WaitForSeconds(Duration);

        vocabField.SetActive(false);
        //TODO
        Command.CommandExecutionComplete();
    }
    // Start is called before the first frame update
    void Start()
    {
        ShowScramble(currentWord);
    }

    // Update is called once per frame
    void Update()
    {
        RePositionObject();
    }
    void RePositionObject()
    {
        if (charObjects.Count == 0)
        {
            return;
        }
        float center = (charObjects.Count - 1) / 2;
        for (int i = 0; i < charObjects.Count; i++)
        {
            charObjects[i].rectTransform.anchoredPosition = Vector2.Lerp(charObjects[i].rectTransform.anchoredPosition,
                                                            new Vector2((i - center) * space, 0),lerpSpeed*Time.deltaTime);
            charObjects[i].index = i;
        }
    }
    public void ShowScramble()
    {
        ShowScramble(Random.Range(0, words.Length - 1));
    }
    public void ShowScramble(int index)
    {
        charObjects.Clear();
        foreach (Transform child in container)
        {
            Destroy(child.gameObject);
        }
        if (index > words.Length - 1)
        {
            Debug.LogError("index out of length" + (words.Length - 1).ToString());
        }
        char[] chars = words[index].GetString().ToCharArray();
        foreach (char c in chars)
        {
            CharObject clone = Instantiate(prefab.gameObject).GetComponent<CharObject>();
            clone.transform.SetParent(container);

            charObjects.Add(clone.Init(c));

            currentWord = index;
        }
    }
    public void Swap(int indexA, int indexB)
    {
        CharObject tmpA = charObjects[indexA];
        charObjects[indexA] = charObjects[indexB];
        charObjects[indexB] = tmpA;

        charObjects[indexA].transform.SetAsLastSibling();
        charObjects[indexB].transform.SetAsLastSibling();

        CheckWord();
    }

    public void Select(CharObject charObject)
    {
        if (firstSelected)
        {
            Swap(firstSelected.index, charObject.index);

            //unselected
            firstSelected.Select();
            charObject.Select();

        }
        else
        {
            firstSelected = charObject;
        }
    }
    public void Unselect()
    {
        firstSelected = null;
    }

    public void CheckWord()
    {
        StartCoroutine(coCheckWord());
    }

    IEnumerator coCheckWord()
    {
        yield return new WaitForSeconds(0.5f);

        string word = "";
        foreach (CharObject charObject in charObjects)
        {
            word += charObject.charactor;
        }
        if (word == words[currentWord].word)
        {
            currentWord++;
            pointText.text=currentWord.ToString();
            ShowScramble(currentWord);
        }
    }
    

}

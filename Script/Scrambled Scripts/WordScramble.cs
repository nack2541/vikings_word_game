using Firebase;
using Firebase.Database;
using Firebase.Unity.Editor;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

[System.Serializable]
public class Word
{
    public string word;

    public string GetString()
    {
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

//-------------------------------------------END WORD CLASS----------------------------------------------------------

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
    public Text totalScoreText;

    private List<CharObject> charObjects = new List<CharObject>();
    private CharObject firstSelected;
    private int currentWord;

    public static WordScramble Instance;

    private int currentScore;
    private int totalScore=0;
    public int manaFromScore;

    private DatabaseReference reference;

    [System.Obsolete]
    private void Awake()
    {
        Instance = this;
        vocabField.SetActive(false);
        manaFromScore = 1;

        //words = new Word[20];
        //CreateWordFromDB();
    }

    public void ShowVocabField(float Duration)
    {
        StartCoroutine(ShowVocabFieldCoroutine(Duration));
    }

    private IEnumerator ShowVocabFieldCoroutine(float Duration)
    {
        //Debug.Log("Showing some message. Duration: " + Duration);
        vocabField.SetActive(true);
        //ShowScramble(currentWord);
        ShowScramble();

        yield return new WaitForSeconds(Duration);
        vocabField.SetActive(false);
        manaFromScore = currentScore;
        // Debug.Log("Showing thisMana " + thisMana);

        ResetPoint();
        //TODO
        Command.CommandExecutionComplete();
    }

    // Start is called before the first frame update
    [System.Obsolete]
    private void Start()
    {
        FirebaseApp.DefaultInstance.SetEditorDatabaseUrl("https://vikingswordgame.firebaseio.com/");
        reference = FirebaseDatabase.DefaultInstance.RootReference;
    }
    public void CreateWordFromDB()
    {
        FirebaseDatabase.DefaultInstance.GetReference("vocabularys").ValueChanged += Script_ValueChanged;
    }

    private void Script_ValueChanged(object sender, ValueChangedEventArgs e)
    {
        //words[0].word = e.Snapshot.Child("1").Child("vocabulary").GetValue(true).ToString();

        for (int i = 0; i < words.Length; i++)
        {
            words[i].word = e.Snapshot.Child(""+(i+1)).Child("vocabulary").GetValue(true).ToString();
        }
    }
    //private void Script_ValueChanged(object sender, ValueChangedEventArgs e)
    //{
    //    for (int i = 0; i < words.Length; i++)
    //    {
    //        words[i].word = e.Snapshot.Child("WordNo" + (i + 1)).GetValue(true).ToString();
    //    }
    //}

    // Update is called once per frame
    private void Update()
    {
        RePositionObject();
        ShowTotalScore();
    }

    public void ResetPoint()
    {
        totalScore = totalScore + currentScore;
        //Debug.Log("totalscore+ " + totalScore);

        currentScore = 0;
        pointText.text = currentScore.ToString();
    }

    public void ShowTotalScore()
    {
        totalScoreText.text = totalScore.ToString();
    }

    private void RePositionObject()
    {
        if (charObjects.Count == 0)
        {
            return;
        }
        float center = (charObjects.Count - 1) / 2;
        for (int i = 0; i < charObjects.Count; i++)
        {
            charObjects[i].rectTransform.anchoredPosition = Vector2.Lerp(charObjects[i].rectTransform.anchoredPosition,
                                                            new Vector2((i - center) * space, 0), lerpSpeed * Time.deltaTime);
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

    private IEnumerator coCheckWord()
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
            currentScore++; // Increase score
            pointText.text = currentScore.ToString();//change score
            //ShowScramble(currentWord);
            ShowScramble();
        }
    }
}
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using Firebase;
using Firebase.Database;
using Firebase.Unity.Editor;
using System;
using Proyecto26;

public class firebase : MonoBehaviour
{
    DatabaseReference reference;
    public InputField vocabNoTextField;
    public InputField vocabTextField;
    public Text dataText;

    public static string vocabNoText;
    public static string vocabText;

    Vocabulary vocab = new Vocabulary();


    void Start()
    {
        FirebaseApp.DefaultInstance.SetEditorDatabaseUrl("https://vikingswordgame.firebaseio.com/");
        reference = FirebaseDatabase.DefaultInstance.RootReference;

    }

    // Update is called once per frame
    void Update()
    {

    }
    public void saveData()
    {
        reference.Child("Vocabularys").Child("WordNo1").SetValueAsync(vocabTextField.text.ToString());
        Debug.Log("save Success");
    }
    public void loadData()
    {
        FirebaseDatabase.DefaultInstance.GetReference("1").ValueChanged += Script_ValueChanged;
        Debug.Log("load Success");
    }
    public void checkData()
    {
        FirebaseDatabase.DefaultInstance.GetReference("Vocabularys").ValueChanged += CheckScript_ValueChanged;

    }

    private void Script_ValueChanged(object sender, ValueChangedEventArgs e)
    {
        dataText.text = e.Snapshot.Child("vocabulary").GetValue(true).ToString();
    }
    private void CheckScript_ValueChanged(object sender, ValueChangedEventArgs e)
    {
        if (vocabTextField.text.ToString().Equals(e.Snapshot.Child("WordNo1").GetValue(true).ToString()))
        {
            dataText.text = "same word";
        }
        else if (!vocabTextField.text.ToString().Equals(e.Snapshot.Child("WordNo1").GetValue(true).ToString()))
        {
            dataText.text = "wrong word";
        }

    }

    public void addNewVocabulary()
    {
        vocabNoText = vocabNoTextField.text;
        vocabText = vocabTextField.text;
        postToDatabase();
    }

    public void loadVocabulary()
    {
        retrieveFromDatabase();
    }
    private void updateScore()
    {
        dataText.text = vocab.number + "123 " + vocab.vocabulary;
    }

    private void postToDatabase()
    {
        Vocabulary vocab = new Vocabulary();
        RestClient.Put("https://vikingswordgame.firebaseio.com/"+"vocabularys/" + vocabNoText + ".json", vocab);
        Debug.Log("add word sucess");
        dataText.text = "add word sucess";
    }

    private void retrieveFromDatabase()
    {
        RestClient.Get<Vocabulary>("https://vikingswordgame.firebaseio.com/" + vocabNoTextField.text + ".json").Then(response =>
        {
            vocab = response;
            updateScore();
        }
        );
    }

}

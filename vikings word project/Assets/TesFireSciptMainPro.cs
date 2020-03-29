using System.Collections;
using System.Collections.Generic;
using Firebase;
using Firebase.Database;
using Firebase.Unity.Editor;
using UnityEngine;
using UnityEngine.UI;

public class TesFireSciptMainPro : MonoBehaviour
{
    public Text data;
    public InputField InputFields;
    DatabaseReference reference; 
    // Start is called before the first frame update
    void Start()
    {
        // Set up the Editor before calling into the realtime database.
        FirebaseApp.DefaultInstance.SetEditorDatabaseUrl("https://viking-words.firebaseio.com/");

        // Get the root reference location of the database.
        reference = FirebaseDatabase.DefaultInstance.RootReference;
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }
    public void saveData()
    {
        if (!InputFields.text.Equals(""))
        {
            //save data
            reference.Child("Users").Child("Username").Child("Email").SetValueAsync(InputFields.text.ToString());

        }else{

        }
    }
    public void LoadData()
    {
        FirebaseDatabase.DefaultInstance
                .GetReference("Users")
                .ValueChanged += firebasetest_ValueChanged;
    }

    public void firebasetest_ValueChanged(object sender, ValueChangedEventArgs e)
    {
        if (e.Snapshot.Child("Username").Exists)
        {
            data.text = e.Snapshot.Child("Username").Child("Email").GetValue(true).ToString();

        }else
        {
            
        }
    }
}

using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ShowVocabFieldCommand : Command
{
    float duration;
    // Start is called before the first frame update
    public ShowVocabFieldCommand(float duration)
    {
        this.duration = duration;
    }

    public override void StartCommandExecution()
    {
        WordScramble.Instance.ShowVocabField(duration);
    }
}

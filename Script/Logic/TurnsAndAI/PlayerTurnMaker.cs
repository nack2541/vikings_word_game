using UnityEngine;
using System.Collections;

public class PlayerTurnMaker : TurnMaker 
{
    // public WordScramble wordScramble;
    public override void OnTurnStart()
    {
        base.OnTurnStart();

        // dispay a message that it is player`s turn
        new ShowMessageCommand("Your Turn!", 1.0f).AddToQueue();
        new ShowVocabFieldCommand(29.0f).AddToQueue();

        p.DrawACard();
    }
    
}

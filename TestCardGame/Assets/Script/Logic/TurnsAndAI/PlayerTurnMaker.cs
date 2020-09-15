using UnityEngine;
using System.Collections;

public class PlayerTurnMaker : TurnMaker 
{
    public WordScramble wordScramble;
    public override void OnTurnStart()
    {
        base.OnTurnStart();
        // dispay a message that it is player`s turn
        new ShowMessageCommand("Your Turn!", 2.0f).AddToQueue();
        new ShowVocabFieldCommand(10.0f).AddToQueue();
        p.DrawACard();
    }
    
}

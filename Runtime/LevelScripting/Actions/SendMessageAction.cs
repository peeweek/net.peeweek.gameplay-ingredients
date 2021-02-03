using UnityEngine;

namespace GameplayIngredients.Actions
{
    [HelpURL("https://peeweek.readthedocs.io/en/latest/gameplay-ingredients/messager/")]
    public class SendMessageAction : ActionBase
    {
        public string MessageToSend = "Message";

        public override void Execute(GameObject instigator = null)
        {
            Messager.Send(MessageToSend, instigator);
        }
    }
}



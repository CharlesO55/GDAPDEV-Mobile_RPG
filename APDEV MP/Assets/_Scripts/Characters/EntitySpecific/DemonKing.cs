using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DemonKing : CharacterScript
{
    /*protected override void AdditionalUpdateFunctions()
    {
        EndGameOnDeath();
    }*/

    public override void TriggerPlayerDeath()
    {
        base.TriggerPlayerDeath();
        this.EndGameOnDeath();
    }

    private void EndGameOnDeath()
    {
        if(this._characterData.CurrHealth <= 0)
        {
            Debug.Log("Demon King was slain");

            SceneLoaderManager.Instance.LoadScene(GameSettings.END_SCENE_INDEX);
        }
    }
}
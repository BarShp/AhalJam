using System.Collections;
using System.Collections.Generic;
using UnityEngine.SceneManagement;

public class PlayerDamageble : DamageableComponent
{
    public override void OnDamage()
    {
        SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex);
    }
}

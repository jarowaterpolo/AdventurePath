using UnityEngine;

public class EnemyApplyPoison : MonoBehaviour
{
    public void Execute()
    {
        var data = Data.Instance;

        data.player_poison += 2;

        Debug.Log("Apply Poison Used By Enemy");
    }
}

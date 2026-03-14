using UnityEngine;

public class EnemyProtect : MonoBehaviour
{
    public void Execute()
    {
        var data = Data.Instance;

        data.enemyshield += 5;

        Debug.Log("Protect Used By Enemy");
    }
}

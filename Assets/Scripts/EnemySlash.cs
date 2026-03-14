using UnityEngine;

public class EnemySlash : MonoBehaviour
{
    public void Execute()
    {
        var data = Data.Instance;

        // Apply damage to the shield first
        if (data.playershield > 0)
        {
            data.playershield -= data.Eslash;

            // If the shield goes negative, apply leftover damage to player HP
            if (data.playershield < 0)
            {
                data.playerhp += data.playershield; // Negative shield reduces HP
                data.playershield = 0;
            }
        }
        else
        {
            // No shield, apply damage directly to HP
            data.playerhp -= data.Eslash;
        }

        Debug.Log("Slash Used By Enemy");
    }
}

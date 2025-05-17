using UnityEngine;

public class RandomValueGenerator : MonoBehaviour
{
    void Start()
    {
        string randomValue = GenerateRandomValue(10000000, 50000000);
        Debug.Log("Random Value: " + randomValue);
    }

    public string GenerateRandomValue(int min, int max)
    {
        string noise = (Random.Range(min, max + 1)).ToString();

        return noise; // max is exclusive in int version, so we add 1
    }
}

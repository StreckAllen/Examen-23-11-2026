using UnityEngine;

public class GameManager : MonoBehaviour
{
    public static GameManager Instance { get; private set; }

    [SerializeField] private int coinGold = 0;
    [SerializeField] private int coinSilver = 0;
    [SerializeField] private int trophies = 0;


    public int Gold => coinGold;
    public int Silver => coinSilver;
    public int Trophies => trophies;

    private void Awake()
    {
        if (Instance == null)
        {
            Instance = this;

            DontDestroyOnLoad(gameObject);
        }
        else
        {
            Destroy(gameObject);
        }
    }


    public void CollectCoin(TypeCoin type, int amount)
    {
        switch (type)
        {
            case TypeCoin.Gold:

                coinGold += amount;

                break;


            case TypeCoin.Silver:

                coinSilver += amount;

                while (coinSilver >= 3)
                {
                    coinSilver -= 3;
                    coinGold += 1;
                }

                break;


            case TypeCoin.Trophy:

                trophies += amount;

                break;
        }
    }
}
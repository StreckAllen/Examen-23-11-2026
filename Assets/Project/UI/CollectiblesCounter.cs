using UnityEngine;
using TMPro;

public class CollectiblesCounter : MonoBehaviour
{
    [SerializeField] private TMP_Text counterText;

    private void Awake()
    {
        if (counterText == null) counterText = GetComponent<TMP_Text>();
    }

    private void Update()
    {
        var manager = GameManager.Instance;
        if (manager == null || counterText == null) return;

        counterText.SetText("Trofeos: {0}\nOro: {1}\nPlata: {2}",
            manager.Trophies, manager.Gold, manager.Silver);
    }
}

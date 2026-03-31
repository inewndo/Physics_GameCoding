using TMPro;
using UnityEngine;

public class ScoreManager : MonoBehaviour
{
    public TextMeshProUGUI scoreDisplay;
    private int scoreNumber = 0;
    public GameObject ballPrefab;
    public Transform spawnPoint;

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
    }

    private void OnTriggerEnter(Collider other)
    {
       
        if (other.gameObject.CompareTag("Interactable"))
        {
            scoreNumber++;
            scoreDisplay.text = "Scored:" + scoreNumber.ToString();
            Destroy(other.gameObject, 2f);
            Instantiate(ballPrefab, spawnPoint.position, spawnPoint.rotation);
        }
    }
    
}

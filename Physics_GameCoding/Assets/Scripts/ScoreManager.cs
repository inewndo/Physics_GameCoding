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
            //count the score
            scoreNumber++;
            //display score
            scoreDisplay.text = "Scored:" + scoreNumber.ToString();
            //destroy the ball which just scored and  spawn new one in the designated space, so the player doesnt have to chase the bouncing away ball
            Destroy(other.gameObject, 2f);
            Instantiate(ballPrefab, spawnPoint.position, spawnPoint.rotation);
        }
    }
    
}

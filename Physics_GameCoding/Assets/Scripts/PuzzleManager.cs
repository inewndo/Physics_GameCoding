using UnityEngine;
using UnityEngine.Events;

public class PuzzleManager : MonoBehaviour
{
    //attach this to empty GO
    //holds the list of all puzzles. Fires on solved when every puzzle is done
    //listens to each OnCompleted

    //drag each puzzzle event object into this list
    public PuzzleEvent1[] puzzles;
    //fires when the entire list is solved
    public UnityEvent OnAllSolved;
    private bool[] solvedStates;
    private bool allSolved = false;

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        if(puzzles == null || puzzles.Length == 0)
        {
            Debug.Log("Puzzle manager has no puzzles assigned");
            return;
        }

        solvedStates = new bool[puzzles.Length];

        //subscribe to each puzzzle on complated and onreset evets
        //use a local copy of i for the lambda to capture the right index
        for(int i = 0; i< puzzles.Length; i++)
        {
            int index = 1;
            puzzles[i].OnCompleted.AddListener(() => OnPuzzleSolved(index));
            puzzles[i].OnReset.AddListener(() => OnPuzzleReset(index));

        }
    }

    //call when puzzle event fires on Completed
    void OnPuzzleSolved(int index)
    {
        solvedStates[index] = true;
        Debug.Log($"Puzzle {index} solved. Checking all puzzles. . .");
        CheckAllSolved();
    }
    //call when puzzle event fires on Reset

    void OnPuzzleReset(int index)
    {
        solvedStates[index] = false;
        allSolved = false;
        Debug.Log($"Puzzle {index} reset.");
    }
    //loops through all solved states adn fires on ALLSOLVED if every one is true/completed
    void CheckAllSolved()
    {
        foreach(bool state in solvedStates)
        {
            //if even one is not completed - exit the function
            if (!state) return;
        }
        if (!allSolved)
        {
            allSolved = true;
            Debug.Log("all puzzzles solved");
            OnAllSolved?.Invoke();
        }
    }
}

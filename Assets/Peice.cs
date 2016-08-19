using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class Peice : MonoBehaviour
{
    public Position position;
    public int team;
    private Game game;
    private static readonly Color[] TeamColors = {
        new Color(1.0f, 0.0f, 0.0f, 0.25f), // Red
        new Color(0.0f, 0.0f, 1.0f, 0.25f), // Blue
        new Color(0.0f, 1.0f, 0.0f, 0.25f), // Green
        new Color(0.0f, 1.0f, 1.0f, 0.25f), // Cyan
        new Color(1.0f, 0.0f, 1.0f, 0.25f), // Pink
        new Color(1.0f, 1.0f, 0.0f, 0.25f), // Yellow
        new Color(1.0f, 1.0f, 1.0f, 0.25f), // White
        new Color(0.0f, 0.0f, 0.0f, 0.25f)  // Black
    };

    // Use this for initialization
    void Start()
    {

    }

    public void Init(Game inGame, int inTeam, int inX, int inY, int inZ)
    {
        game = inGame;
        team = inTeam;
        position = new Position(inX, inY, inZ);
        transform.position = game.GetPeicePosition(position);
        GetComponent<Renderer>().material.color = TeamColors[team];
        transform.parent = game.transform;
        string teamString;
        switch (team)
        {
            case 0: teamString = "Red"; break;
            case 1: teamString = "Blue"; break;
            case 2: teamString = "Green"; break;
            case 3: teamString = "Cyan"; break;
            case 4: teamString = "Pink"; break;
            case 5: teamString = "Yellow"; break;
            case 6: teamString = "White"; break;
            case 7: teamString = "Black"; break;
            default: teamString = "Blocker"; break;
        }
        name = teamString + " Peice";
        Debug.Log("Spawned " + name + " at " + position);
    }

    // Update is called once per frame
    void Update()
    {

    }
}

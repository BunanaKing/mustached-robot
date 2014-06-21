using UnityEngine;
using System.Collections;

public class Score : MonoBehaviour {

    static int distance = 0;
    static int highDistance = 0;    
    static int cookies = 0;

    Transform player;

    // Use this for initialization
    void Start()
    {
        highDistance = PlayerPrefs.GetInt("highDistance", 0);

        GameObject player_go = GameObject.FindGameObjectWithTag("Character");
        if (player_go == null)
        {
            Debug.LogError("Couldn't find an object with tag 'Character'!");
            return;
        }
        player = player_go.transform;        
    }

    static public void AddCookie()
    {
        cookies++;
    }

    // Update is called once per frame
    void Update()
    {
        if (player != null)
            distance = (int)player.position.x;

        guiText.text = "Cookies: " + cookies + "\n" + "Distance: " + distance + " mts";
    }

    void OnDestroy()
    {
        PlayerPrefs.SetInt("highDistance", highDistance);
    }
}

using UnityEngine;
using System.Collections;

public class CollectableCookie : MonoBehaviour {
    
	
	// Update is called once per frame
	void Update () 
    {
	}

    void OnTriggerEnter2D(Collider2D collider)
    {        
        if (collider.gameObject.layer == LayerMask.NameToLayer(Definitions.character_layer) ||
            collider.gameObject.layer == LayerMask.NameToLayer(Definitions.deadCharacter_layer))
        {
            Score.AddCookie();
            GameObject.Destroy(gameObject);
        }
    }
}

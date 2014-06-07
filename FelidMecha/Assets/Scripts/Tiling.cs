using UnityEngine;
using System.Collections;

[RequireComponent (typeof(SpriteRenderer))]

public class Tiling : MonoBehaviour {

	public int offsetX = 2;				// The offset
	
	public int numberOfBackgrounds = 6; 

	public int repeatCount = 0;

	public float delaySpeed = 0;

	private float spriteWidth = 0f; 
	private Camera cam;

	void Awake(){
		cam = Camera.main;
	}

	// Use this for initialization
	void Start () {
		SpriteRenderer sRenderer = GetComponent<SpriteRenderer>();
		spriteWidth = sRenderer.sprite.bounds.size.x;
	}
	
	// Update is called once per frame
	void Update () {

		if(delaySpeed != 0){
			Vector3 pos = transform.position;
			pos.x += delaySpeed * Time.deltaTime;
			this.transform.position = pos;
		}

		//Calculate the cameras extend (half the width) of what the camera can see in world coordinates
		float camHorizontalExtend = cam.orthographicSize * Screen.width/Screen.height;

		//Calculate the x position where the camera can see the right edge of the sprite (element)
		float edgeVisiblePositionRight = (transform.position.x + spriteWidth/2) + camHorizontalExtend; 

		//Check if the camera has already passed the edge of the element 
		if(cam.transform.position.x >= edgeVisiblePositionRight + offsetX)
		{
			//We move this sprite to the final at the right
			Vector3 pos = transform.position;			
			pos.x += spriteWidth * numberOfBackgrounds;		
			transform.position = pos;
			repeatCount++;
		}

	}

}

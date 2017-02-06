using System.Collections;
using UnityEngine;

[RequireComponent(typeof(SpriteRenderer))]

public class Tiling : MonoBehaviour {

    public int offsetX = 2;

    public bool hasARightBuddy = false;
    public bool hasALeftBuddy = false;

    public bool reverseScale = false;   // Used if object not tileable

    private float spriteWidth = 0f;
    private Camera cam;
    private Transform myTansform;

    void Awake()
    {
        cam = Camera.main;
        myTansform = transform;
    }

	// Use this for initialization
	void Start () {
        SpriteRenderer sRenderer = GetComponent<SpriteRenderer>();
        spriteWidth = sRenderer.sprite.bounds.size.x;
	}
	
	// Update is called once per frame
	void Update () {
		if(hasALeftBuddy == false || hasARightBuddy == false)
        {
            // Calculate the cameras extend (half the width) of what the camera can see in world coordinates
            float camHorizontalExtend = cam.orthographicSize * Screen.width / Screen.height;

            // Calculate the x position where the camera can see the edge of the sprite
            float edgeVisiblePositionRight = (myTansform.position.x + spriteWidth / 2) - camHorizontalExtend;
            float edgeVisiblePositionLeft = (myTansform.position.x - spriteWidth / 2) + camHorizontalExtend;

            // Checking if we can see the edge of the element
            if(cam.transform.position.x >= edgeVisiblePositionRight - offsetX && hasARightBuddy == false)
            {
                MakeNewBuddy(1);
                hasARightBuddy = true;
            }
            else if (cam.transform.position.x <= edgeVisiblePositionLeft + offsetX && hasALeftBuddy == false)
            {
                MakeNewBuddy(-1);
                hasALeftBuddy = true;
            }
        }
	}

    void MakeNewBuddy(int rightOrLeft)
    {
        // Left = -1, Right = 1
        Vector3 newPosition = new Vector3(myTansform.position.x + spriteWidth * rightOrLeft, myTansform.position.y, myTansform.position.z);
        Transform newBuddy = (Transform)Instantiate(myTansform, newPosition, myTansform.rotation);

        // If not tileable, reverse scale to mirror object to tile it atleast somewhat better
        if(reverseScale == true)
        {
            newBuddy.localScale = new Vector3(newBuddy.localScale.x * -1, newBuddy.localScale.y, newBuddy.localScale.z);
        }

        newBuddy.parent = myTansform.parent;
        if(rightOrLeft > 0)
        {
            newBuddy.GetComponent<Tiling>().hasALeftBuddy = true;
        }
        else
        {
            newBuddy.GetComponent<Tiling>().hasARightBuddy = true;
        }
    }
}

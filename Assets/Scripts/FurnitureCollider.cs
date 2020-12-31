using System.Collections;
using System.Collections.Generic;
using UnityEngine;
// using .UI, so we can get text component text
using UnityEngine.UI;

public class FurnitureCollider : MonoBehaviour
{
    // UI text game object, we set so all 3 colliders in map have same output
    public GameObject pointCounter;
    // Sound for when you collide furniture with collider
    public AudioClip collisionSound;

    // All gameobjects have both a trigger and normal collider, but here we use colission, so nothing bad happens
    private void OnCollisionEnter(Collision collision)
    {
        // Check if colliding gameobject is of type "Furniture" (which is a tag we give furniture objects in the editior)
        if (collision.gameObject.tag == "Furniture")
        {
            // Destroy furniture
            Destroy(collision.gameObject);

            // Change text of pointcounter objects component Text. We parse what was there before, and add 1 to it, to add a point to the score, and then we output it to string, so we can set the text to it
            pointCounter.GetComponent<Text>().text = (int.Parse(pointCounter.GetComponent<Text>().text) + 1).ToString();

            // Set audiosource of collisionbox to colission sound, and then play it once
            GetComponent<AudioSource>().clip = collisionSound;
            GetComponent<AudioSource>().PlayOneShot(collisionSound);
        }
    }
}

using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Player : MonoBehaviour
{
    // Public int we set in editor, to represent the 4 different players, who all should have different inputs
    public int playerID;

    // 1 vector, consisting of input from controller, and one called translation, we use to actuate our input vector so we can apply the force to our player
    Vector3 input, translation;

    // Speed, which we also change in editor, because theif should be faster then catchers for balance resons
    public float speed;

    // Bool only used for player 1, who can grab objects, so we use this to check if they want to hold object
    bool tryHolding;

    // Audioclips for 2 sounds. one when going visible and one when going invisible again
    public AudioClip invisSound, visSound;
    // audioSource to play clips
    AudioSource audioSource;

    void Start()
    {
        // only player 1 has a audio source, so setting variable from start prevents any further problems
        audioSource = GetComponent<AudioSource>();
    }

    // Update has a lot, so take it one step at a time
    void Update()
    {
        // First of all input. We take "input" vector from before, and set its value to the same value as the respective players vertical and horizontal joysticks
        // in simple language this means that player 3, who has playerid 3, has controller number 3 controlling the gameobject called player 3
        // in the unity input manager we have a horizontal, vertical, a button and b button for all 4 possible players, so we just need to check which one we need by attaching the in playerid to the inputs
        input = new Vector3(Input.GetAxis("Vertical" + playerID), 0, Input.GetAxis("Horizontal" + playerID));
        // to actuate the input, we first time it with the speed, and then secondly the time so we can use it to move the player
        translation = speed * input * Time.deltaTime;
        // finally we use transform.translate, that moves a object the direction and distance of given vector, that we set before
        transform.Translate(translation);

        // to handle rotation, we have a model as a child of the player, whose rotate we change using the tangent of our two input axis, to get one number of the degrees the player should be rotated
        transform.Find("Karakter").rotation = Quaternion.Euler(0f, Mathf.Atan2(Input.GetAxis("Vertical"+playerID), Input.GetAxis("Horizontal"+playerID)) * Mathf.Rad2Deg + 90, 0f);


        // Only player 1 can be invisible and grab objects, so all if statements under have that requrement

        // If player 1 is pressing the a button, then enable the character so they can be seen
        if (playerID == 1 && Input.GetAxis("AButton1") > 0.5)
            transform.Find("Karakter").gameObject.SetActive(true);

        // Otherwise, disable it, so they can't be seen
        else if (playerID == 1 && Input.GetAxis("AButton1") < 0.5)
            transform.Find("Karakter").gameObject.SetActive(false);

        // If they are holding the b button, they are trying to grab a object, so enable the bool that allows us to grab objects
        if (playerID == 1 && Input.GetAxis("BButton1") > 0.5)
            tryHolding = true;
        // but if they arnt holding the b button, or the player isn't player 1, then disable it, they can't grab objects
        else
            tryHolding = false;


        // Finally to play sounds, we check for a inconsistency in if the character is enabled, and if it should/shouldn't be

        // If player 1 character is disabled (so they are invisible), and they are holding the button as to if they should be visible, then play visibilty sound
        if (playerID == 1 && !transform.Find("Karakter").gameObject.active && Input.GetAxis("AButton1") > 0.5)
        {
            // Set audiosource of player 1 to Visibility sound, and then play it once
            audioSource.clip = visSound;
            audioSource.PlayOneShot(visSound);
        }
        // and again, we check if player 1 is visible, and they aren't pressing the button, so they should be invisible
        if (playerID == 1 && transform.Find("Karakter").gameObject.active && Input.GetAxis("AButton1") < 0.5)
        {
            // Set audiosource of player 1 to invisibility sound, and then play it once
            audioSource.clip = invisSound;
            audioSource.PlayOneShot(invisSound);
        }
    }

    // Players trigger colliders are much bigger then their models, so  we use trigger when grabbing objects, so you dont have to be kissing them to grab them
    private void OnTriggerStay(Collider other)
    {
        // only player 1 can grab
        if (playerID==1)
        {
            // if you are triggering with a piece of furniture, AND trying to grab the object
            if (other.gameObject.tag == "Furniture" && tryHolding)
                // Set the transform to be a child of the player, so the furniture is constantly moving with the same input as the player
                other.gameObject.transform.SetParent(this.gameObject.transform);
            else
                // otherwise, set the parent to null, so it just has its own movement (which in all cases should be none/ or the force applied when ramming into the object, because physics)
                other.gameObject.transform.parent = null;
        }
    }
    // With the samme triggerbox, we can check when something enters it, to see if the theif has been caught
    private void OnTriggerEnter(Collider other)
    {

        // if the object youre collising with has a player script attached, move along, otherwise dont go further.
        // This is to avoid alot of console messages with "Colliding object has no player script", so we can debug other features easier
        if (other.gameObject.GetComponent<Player>()!=null)
        {
            // However if the object has a player script attached, check if it its player one. This is so the game dosnt end if the other players collide with eachother
            if (other.gameObject.GetComponent<Player>().playerID == 1)
            {
                // if player 2,3 or 4 collides with player 1, find all players (who are all nammed Player x), and destroy them with i from for loop, that runs 4 times, once for each player
                for (int i = 0; i < 5; i++)
                {
                    Destroy(GameObject.Find("Player " + i));
                }
                // Enables final screen for ending game
                GameObject.Find("MenuController").GetComponent<MenuController>().EndGame();
            }
        }
    }
    // when object exits our trigger, make sure it still dosnt follow the player
    private void OnTriggerExit(Collider other)
    {
        // Do this by setting no parent, so the object is totally free
        other.gameObject.transform.parent = null;
    }
}

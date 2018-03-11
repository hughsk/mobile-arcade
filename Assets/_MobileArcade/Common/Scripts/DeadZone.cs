using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// Destroys player gameObjects once they enter a collider.
///	The gameObject you assign this script to must have a collider with the trigger option enabled.
/// Useful for knowing how many players are left in the game.
/// </summary>
public class DeadZone : MonoBehaviour {

	void OnTriggerEnter(Collider _col)
	{
		if (_col.gameObject.tag == "Player")
		{
			Destroy(_col.gameObject);
			ArenaManager.amountOfPlayersAlive--;
			ArenaManager.CheckRoundDone();
		}
	}
}

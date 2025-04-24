using System.Collections.Generic;
using UnityEngine;

public class Door : MonoBehaviour
{
    public enum DoorType
    {
        None,
        Open,
        Locked
    }

    public enum DoorFunction
    { 
        enter,
        exit,
        Bidirectional
    }

    [Header("Door functions")]
    [SerializeField] DoorType doorType;
    [SerializeField] DoorFunction doorFunction;
    [SerializeField] int doorID;

    [SerializeField] int doorOrder = -1;

    [SerializeField] List<Door> doors = new List<Door>();


    private void Start()
    {
        int doorCount = 0;
        if (doorOrder == -1)
        {
            doorOrder = 1;
            doorCount++;
        }

        if (doorFunction == DoorFunction.enter || doorFunction == DoorFunction.Bidirectional)
        { 
            Door[] otherDoors = FindObjectsByType<Door>(FindObjectsSortMode.None);

            for (int i = 0; i < otherDoors.Length; i++)
            {
                Door currentDoor = otherDoors[i];

                if (currentDoor.doorID == doorID 
                    && currentDoor != this 
                    && currentDoor.doorFunction != DoorFunction.enter 
                    && currentDoor.doorType != DoorType.None)
                {
                    if (currentDoor.doorOrder == -1)
                    {
                        doorCount++;
                        currentDoor.doorOrder = doorCount;
                    }
                    doors.Add(currentDoor);
                }
            }
        }
    }

    void TeleportPlayer(GameObject player)
    {
        if (doorFunction == DoorFunction.exit)
            return;


        if (doorType == DoorType.Open)
        {
            player.GetComponent<Player>().onPlayerAction -= TeleportPlayer;
            player.transform.position = doors[0].transform.position;
            Debug.Log("Player teleported to door: " + doors[0].doorID);
        }
        else if (doorType == DoorType.Locked)
        {
            // Handle locked door logic
            Debug.Log("Door is locked.");
        }
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.CompareTag("Player"))
        { 
            Player player = collision.GetComponent<Player>();
            if (player != null)
            {
                player.onPlayerAction += TeleportPlayer;
            }
        }
    }

    private void OnTriggerExit2D(Collider2D collision)
    {
        if (collision.CompareTag("Player"))
        {
            Player player = collision.GetComponent<Player>();
            if (player != null)
            {
                player.onPlayerAction -= TeleportPlayer;
            }
        }
    }
}

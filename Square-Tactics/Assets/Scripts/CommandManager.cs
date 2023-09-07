using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class CommandManager : MonoBehaviour
{
    [SerializeField] private TMP_InputField input;
    [SerializeField] private GameObject uICommand;
    [SerializeField] private Transform canvas;
    [SerializeField] private int maxCommandSize;
    [SerializeField] private Player player;
    private List<GameObject> oldCommands= new List<GameObject>();

    //private GameObject oldCommand;
    void Start()
    {
        input.onEndEdit.AddListener(Reselect);
        StartCoroutine(ReselectInputField());
    }
    
    // Update is called once per frame
    void Update()
    {
        if (input != null && input.text.Length > 0 && Input.GetKeyDown(KeyCode.Return))
        {
            List<MoveType> moves = new List<MoveType>();

            string[] commands = input.text.Split(' ');

            
            foreach (string command in commands)
            {
                if (Enum.TryParse(command.ToUpper(), out MoveType parsedEnum))
                {
                    moves.Add(parsedEnum);
                }
                else
                {
                    print("command not recognized" + command.ToUpper());
                }                
            }
            
            player.SetNextMoves(moves);
            SpawnCommands();
            DeleteOldCommands();
            MoveCommands();
        }

    }
    void SpawnCommands()
    {
        GameObject oldCommand = Instantiate(uICommand, input.transform.position, Quaternion.identity, canvas);
        oldCommand.GetComponent<TMP_Text>().text = input.text;
        input.text = "";
        oldCommands.Insert(0, oldCommand);
    }
    private void DeleteOldCommands()
    {
        if (oldCommands.Count > maxCommandSize)
        {
            Destroy(oldCommands[oldCommands.Count - 1]);
            oldCommands.RemoveAt(oldCommands.Count - 1);
        }
    }

    private void MoveCommands()
    {
        foreach (GameObject command in oldCommands)
        {
            command.transform.position += new Vector3(0, 50, 0);
        }
    }

    private void Reselect(string arg0)
    {
        StartCoroutine(ReselectInputField());
    }

    private IEnumerator ReselectInputField()
    {
        yield return null;
        input.Select();
        input.ActivateInputField();
    }
}

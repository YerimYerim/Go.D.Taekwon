using System.Collections;
using System.Collections.Generic;
using Script.Manager;
using UnityEngine;

public class CommandManager : Singleton<CommandManager>
{
    private Queue<(ICommand commands, float seconds)> _commandQueue = new();

    private Coroutine _executeCommandsCoroutine;

    public void StartGameCommand()
    {
        if (_executeCommandsCoroutine != null)
        {
            // StopCoroutine(_executeCommandsCoroutine);
            // _executeCommandsCoroutine = null;
        }
        else
        {
            _executeCommandsCoroutine = StartCoroutine(ExecuteCommands());
        }
    }

    public void AddCommand(ICommand command, float time)
    {
        _commandQueue.Enqueue((command,time));
    }

    private IEnumerator ExecuteCommands()
    {
        while (_commandQueue.Count > 0)
        {
            var command = _commandQueue.Dequeue();
            command.commands.Execute();
            yield return new WaitForSeconds(command.seconds); //command.seconds); // Example delay between commands
        }
        _executeCommandsCoroutine = null;
    }
}

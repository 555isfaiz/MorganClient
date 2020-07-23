using System.Collections.Generic;
using System;
using UnityEngine;

public class ModControl : ModBase
{
    public enum Command
    {
        MOVE_FORWARD,
        MOVE_BACKWARD,
        MOVE_LEFT,
        MOVE_RIGHT,
        DASH_FORWARD,
        DASH_BACKWARD,
        DASH_LEFT,
        DASH_RIGHT,
        JUMP,
        SWITCH_LOCK,
    }

    public int[] commands_ = new int[50];        // 50 commands total, consider expand in future

    int[] commandBuff_ = new int[50];
        
    long nextFlush_;

    MSSimpleExecutor executor = new MSSimpleExecutor();

    public ModControl(MonoBehaviour owner) : base(owner) { nextFlush_ = Utils.GetTimeMilli(); }

    public override void StartOverride() {}

    public override void UpdateOverride()
    {
        long now = Utils.GetTimeMilli();

        // MOVE W
        if (Input.GetKeyDown(KeyCode.W))
        {
            MoveCommand(Command.MOVE_FORWARD);
            Debug.Log("key down WWWWWWWWWWWWWWWWW");
        }
        if (Input.GetKeyUp(KeyCode.W))
        {
            // commandBuff_[(int)Command.MOVE_FORWARD] = 0;
            MoveDelayClean(Command.MOVE_FORWARD, now);
        }

        // MOVE S
        if (Input.GetKeyDown(KeyCode.S))
        {
            MoveCommand(Command.MOVE_BACKWARD);
            Debug.Log("key down SSSSSSSSSSSSSSSSS");
        }
        if (Input.GetKeyUp(KeyCode.S))
        {
            // commandBuff_[(int)Command.MOVE_BACKWARD] = 0;
            MoveDelayClean(Command.MOVE_BACKWARD, now);
        }

        // MOVE A
        if (Input.GetKeyDown(KeyCode.A))
        {
            MoveCommand(Command.MOVE_LEFT);
            Debug.Log("key down AAAAAAAAAAAAAAAAAAAAA");
        }
        if (Input.GetKeyUp(KeyCode.A))
        {
            // commandBuff_[(int)Command.MOVE_LEFT] = 0;
            MoveDelayClean(Command.MOVE_LEFT, now);
        }

        // MOVE D
        if (Input.GetKeyDown(KeyCode.D))
        {
            MoveCommand(Command.MOVE_RIGHT);
            Debug.Log("key down DDDDDDDDDDDDDDDDDDDD");
        }
        if (Input.GetKeyUp(KeyCode.D))
        {
            // commandBuff_[(int)Command.MOVE_RIGHT] = 0;
            MoveDelayClean(Command.MOVE_RIGHT, now);
        }

        if (Input.GetKeyDown(KeyCode.J))
        {
            commands_[IndexOf(Command.SWITCH_LOCK)] = 1;
        }
        
        if (Input.GetKeyDown(KeyCode.Space))
        {
            commands_[IndexOf(Command.JUMP)] = 1;
        }

        executor.Update(now);
        if (now > nextFlush_)
        {
            FlushBuff();
            nextFlush_ = now + 100;     //0.1s
        }
    }

    public override void StopOverride() {}

    int IndexOf(Command command) 
    {
        return (int)command;
    }

    void MoveCommand(Command command) 
    {
        int index = IndexOf(command);
        if (commandBuff_[index] != 0)
        {
            // transformed into dash
            commands_[index + 4] = 1;
            commandBuff_[index] = 0;
            return;
        }

        commandBuff_[index] = 1;
    }

    void MoveDelayClean(Command command, long now)
    {
        executor.Add(new MSTask
        {
            delay = now + 80,       // this will affect the minumn time for double tap
            func = () =>
            {
                // when a move key is up. After 0.1s delay, the move command will be removed in commandBuff_
                // but if the same move key is pressed down before the remove, the move will be ignored and be 
                // transformed into a dash. So this solution is ok.
                if (commandBuff_[(int)command] == 0)
                {
                    return;
                }
                commandBuff_[(int)command] = 0;
            }
        });
    }

    void FlushBuff() 
    {
        for (int i = 0; i < commandBuff_.Length; i++)
        {
            commands_[i] = commandBuff_[i];
        }
    }

    public bool TryExecuteCommand(Command command) 
    {
        int index = IndexOf(command);
        if (commands_[index] == 0)
        {
            return false;
        }

        // clean every commands except MOVE commands
        if (command != Command.MOVE_FORWARD && command != Command.MOVE_BACKWARD && command != Command.MOVE_LEFT && command != Command.MOVE_RIGHT)
        {
            commands_[index] = 0;
        }

        return true;
    } 
}
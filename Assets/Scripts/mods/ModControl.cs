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
        FIRE_1,
        FIRE_2,
    }

    GameObject player;

    MonoBehaviour camera;

    public int[] commands_ = new int[50];        // 50 commands total, consider expand in future

    int[] commandBuff_ = new int[50];
        
    long nextFlush_;

	float bottomLimit;//the cos value

    Vector3 dirVector;

    MSSimpleExecutor executor = new MSSimpleExecutor();

    public ModControl(MonoBehaviour owner) : base(owner) { nextFlush_ = Utils.GetTimeMilli(); }

    public override void StartOverride() 
    {
        // init camera
        player = MSMain.modGameMaster.GetMainPlayer();
        camera = MSMain.modGameMaster.GetCameraObject();
    }

    public override void UpdateOverride()
    {
        long now = Utils.GetTimeMilli();

        if (Input.GetKeyDown(KeyCode.Escape))
        {
            MSMain.Reset();
        }

        // MOVE W
        if (Input.GetKeyDown(KeyCode.W))
        {
            MoveCommand(Command.MOVE_FORWARD);
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
        }
        if (Input.GetKeyUp(KeyCode.D))
        {
            // commandBuff_[(int)Command.MOVE_RIGHT] = 0;
            MoveDelayClean(Command.MOVE_RIGHT, now);
        }

        // if (Input.GetKeyDown(KeyCode.J))
        // {
        //     commands_[IndexOf(Command.SWITCH_LOCK)] = 1;
        // }
        
        if (Input.GetKeyDown(KeyCode.Space))
        {
            commands_[IndexOf(Command.JUMP)] = 1;
        }

        if (Input.GetKeyDown(KeyCode.Mouse0))
        {
            commands_[IndexOf(Command.FIRE_1)] = 1;
        }
        if (Input.GetKeyUp(KeyCode.Mouse0))
        {
            commands_[IndexOf(Command.FIRE_1)] = 0;
        }

        if (Input.GetKeyDown(KeyCode.Mouse1))
        {
            commands_[IndexOf(Command.FIRE_2)] = 1;
        }
        if (Input.GetKeyUp(KeyCode.Mouse1))
        {
            commands_[IndexOf(Command.FIRE_2)] = 0;
        }

        executor.Update(now);
        if (now > nextFlush_)
        {
            FlushBuff();
            nextFlush_ = now + 100;     //0.1s
        }
    }

    public override void FixedUpdateOverride()
    {
        CameraMove();
    }

    public override void StopOverride() {}

    int IndexOf(Command command) 
    {
        return (int)command;
    }

    protected override void OnEventGameJoin(params object[] args)
    {
        Cursor.visible = false;
		Cursor.lockState = CursorLockMode.Locked;
        dirVector = Vector3.Normalize(player.transform.position - camera.transform.position);
        bottomLimit = Mathf.Cos(MSGlobalParams.bottomLimitAngle / 180 * Mathf.PI);
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
            delay = now + MSGlobalParams.keyCleanDelay,       // this will affect the minumn time for double tap
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

    void CameraMove()
    {
        // var posv3 = new Vector3(camera.transform.position.x, MSGlobalParams.fixedHeight + player.transform.position.y, camera.transform.position.z);

        //Camera Move
		float fMouseX = Input.GetAxis("Mouse X");
		float fMouseY = Input.GetAxis("Mouse Y");
        // Debug.Log("mouse x:" + fMouseX + ", mouse y:" + fMouseY);
 
		//avoid dithering
		if (Vector3.Dot(-dirVector.normalized, -player.transform.up.normalized) > bottomLimit) 
        {
			if (fMouseY > 0) 
            {
				fMouseY = 0;
			}
		}

		//Rotate Horizontal
		camera.transform.RotateAround(player.transform.position + new Vector3(0f, 1.3f, 0f) ,player.transform.up, MSGlobalParams.cameraMoveSpeed * fMouseX);
		//Rotate Vertical
		camera.transform.RotateAround(player.transform.position + new Vector3(0f, 1.3f, 0f), -VerticalRotateAxis(dirVector), MSGlobalParams.cameraMoveSpeed * fMouseY);
		//distance Control
		dirVector = Vector3.Normalize(player.transform.position - camera.transform.position);
        camera.transform.eulerAngles = new Vector3(camera.transform.eulerAngles.x, camera.transform.eulerAngles.y, 0);
    }

    Vector3 VerticalRotateAxis(Vector3 dirVector){
		Vector3 player2Camera = -dirVector.normalized;
		float x = player2Camera.x;
		float z = player2Camera.z;
		Vector3 rotateAxis = Vector3.zero;
		rotateAxis.z = Mathf.Sqrt (x * x / (x * x + z * z));
		rotateAxis.x = Mathf.Sqrt (z * z / (x * x + z * z));
		if (x >= 0) {
			if (z >= 0) {
				rotateAxis.x = -rotateAxis.x;
			}
		} else {
			if (z >= 0) {
				rotateAxis.x = -rotateAxis.x;
				rotateAxis.z = -rotateAxis.z;
			} else {
				rotateAxis.z = -rotateAxis.z;
			}
		}
		// Debug.Log (rotateAxis);
		return rotateAxis;
	}
}
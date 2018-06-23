using System.Collections;
using System.Collections.Generic;
using BeardedManStudios.Forge.Networking;
using BeardedManStudios.Forge.Networking.Generated;
using BeardedManStudios.Forge.Networking.Unity;
using UnityEngine;

public class Player : PlayerBehavior 
{
	private void Update()
	{
		if (Input.GetKeyDown(KeyCode.UpArrow))
			networkObject.SendRpc(RPC_MOVE_UP, Receivers.All);

		else if (Input.GetKeyDown(KeyCode.DownArrow))
			networkObject.SendRpc(RPC_MOVE_DOWN, Receivers.All);
	}
	
	public override void MoveUp(RpcArgs args)
	{
		MainThreadManager.Run(() =>
		{
			transform.position += Vector3.up;
		});
	}

	public override void MoveDown(RpcArgs args)
	{
		MainThreadManager.Run(() =>
		{
			transform.position += Vector3.down;
		});
	}
}

using UnityEngine;

public class InputSystem : MonoBehaviour
{
    public Player player;

	public GameEvent eventMoveUp;
	public GameEvent eventMoveLeft;
	public GameEvent eventMoveRight;
	public GameEvent eventAttack;
    public GameEvent eventGrabOrThrow;

    private void Update()
	{
		float moveX = Input.GetAxis(player.horizontalInput);
		//float moveY = Input.GetAxis(player.verticalInput);

		// Movement
		if (moveX > 0)
		{
			eventMoveRight.sentFloat = moveX;
			eventMoveRight.Raise();
		}
		else if (moveX < 0)
		{
			eventMoveLeft.sentFloat = moveX;
			eventMoveLeft.Raise();
		} else
		{
			eventMoveRight.sentFloat = 0;
			eventMoveRight.Raise();

			eventMoveLeft.sentFloat = 0;
			eventMoveLeft.Raise();
		}

		if (Input.GetButtonDown(player.fireInput))
		{
			eventAttack.Raise();
		}

		if (Input.GetButtonDown(player.jumpInput))
		{
			eventMoveUp.sentVector2 = Vector2.up;
			eventMoveUp.Raise();
		}

        if (Input.GetButtonDown(player.grabInput))
        {
            eventGrabOrThrow.Raise();
        }
	}
}

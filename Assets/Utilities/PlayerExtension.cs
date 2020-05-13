using Photon.Realtime;

public static class PlayerExtensions
{
#region PlayerReadyState

	public static void SetReady(this Player player, bool value)
	{
		player.SetPropertyValue(PlayerProperties.Ready, value);
	}

	public static bool IsReady(this Player player)
	{
		if (player.CustomProperties.TryGetValue(PlayerProperties.Ready, out var activState))
		{
			return (bool) activState;
		}

		return false;
	}

#endregion
}
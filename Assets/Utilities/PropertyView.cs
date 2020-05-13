using Photon.Pun;
using UnityEngine;
using UnityEngine.UI;

namespace Utilities
{
	/// <summary>
	/// Contains no relevant Game code.
	/// Only purposes is to gain some information to debug the Client.
	/// </summary>
	public class PropertyView : MonoBehaviour
	{
		[SerializeField] private Text m_content = null;
		[SerializeField] private GameObject m_view = null;

		private void Update()
		{
			if (Input.GetKeyDown(KeyCode.KeypadMinus) || Input.GetKeyDown(KeyCode.Minus))
			{
				m_view.SetActive(!m_view.activeInHierarchy);
			}
		}

		private void OnGUI()
		{
			if (!m_view.activeInHierarchy) return;
			var text = "";
			text += $"Region : {PhotonNetwork.CloudRegion}\n";
			text += $"State : {PhotonNetwork.NetworkClientState}\n";
			text += $"In Room : {PhotonNetwork.InRoom}\n";
			text += $"Rooms : {PhotonNetwork.CountOfRooms}\n";
			text += $"In Lobby : {PhotonNetwork.InLobby}\n";

			if (PhotonNetwork.InLobby)
			{
				text +=
					$"Lobby Infos: {PhotonNetwork.CurrentLobby.Name ?? ""} , {PhotonNetwork.CurrentLobby.Type}\n";
			}

			text += $"Connected and Ready : {PhotonNetwork.IsConnectedAndReady}\n";
			text += $"Game Version: {PhotonNetwork.GameVersion}\n";
			text += $"App Version: {PhotonNetwork.PhotonServerSettings.AppSettings.AppVersion}\n";
			text += $"Count of Players: {PhotonNetwork.CountOfPlayers}\n";

			if (PhotonNetwork.LocalPlayer != null && PhotonNetwork.InRoom)
			{
				text += $"###Player Properties###\n";
				foreach (var property in PhotonNetwork.LocalPlayer.CustomProperties)
				{
					var prop = $"Player Props : {property.Key} = {property.Value}\n";
					text += prop;
				}

				text += $"###Room Properties###\n";
				foreach (var property in PhotonNetwork.CurrentRoom.CustomProperties)
				{
					var prop = $"Room Props : {property.Key} = {property.Value}\n";
					text += prop;
				}
			}

			m_content.text = text;
		}
	}
}
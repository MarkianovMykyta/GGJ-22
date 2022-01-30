using System;
using Characters.Player;
using Contexts;
using UnityEngine;

namespace Dialogs
{
	public class DialogStarter : MonoBehaviour, IInteractable
	{
		[SerializeField] private Dialog _dialog;

		private DialogManager _dialogManager;

		private PlayerController _playerController;
		
		private void Awake()
		{
			_dialogManager = GameMaster.Instance.DialogManager;
		}

		public void Interact(PlayerController playerController)
		{
			_dialogManager.DialogCompleted += OnDialogCompleted;

			_playerController = playerController;
			_playerController.Freeze();
			
			_dialogManager.StartDialog(_dialog);
		}

		private void OnDialogCompleted()
		{
			_dialogManager.DialogCompleted -= OnDialogCompleted;
			
			_playerController.UnFreeze();
		}
	}
}
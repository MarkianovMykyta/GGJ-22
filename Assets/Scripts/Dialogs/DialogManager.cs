using System;
using UnityEngine;
using UnityEngine.InputSystem;

namespace Dialogs
{
	public class DialogManager : MonoBehaviour
	{
		public event Action DialogCompleted;
		
		[SerializeField] private DialogView _dialogView;

		private Dialog _activeDialog;
		private int _partIndex;
		private DefaultInputActions _defaultInputActions;

		private void Awake()
		{
			_defaultInputActions = new DefaultInputActions();
			_defaultInputActions.UI.Click.performed += NextPart;
			_defaultInputActions.UI.Submit.performed += NextPart;
		}

		public void StartDialog(Dialog dialog)
		{
			_partIndex = 0;
			_activeDialog = dialog;
			
			_dialogView.Open();
			_dialogView.UpdateText(dialog.Parts[_partIndex]);
			
			_defaultInputActions.UI.Enable();
		}

		private void NextPart(InputAction.CallbackContext obj)
		{
			_partIndex++;

			if (_activeDialog.Parts.Length <= _partIndex)
			{
				CompleteDialog();
			}
			else
			{
				_dialogView.UpdateText(_activeDialog.Parts[_partIndex]);
			}
		}

		private void CompleteDialog()
		{
			_dialogView.Close();
			_defaultInputActions.UI.Disable();
			
			DialogCompleted?.Invoke();
		}
	}
}
using TMPro;
using UnityEngine;

namespace Dialogs
{
	public class DialogView : MonoBehaviour
	{
		[SerializeField] private TMP_Text _dialogText;
		
		public void UpdateText(string text)
		{
			_dialogText.text = text;
		}

		public void Open()
		{
			gameObject.SetActive(true);
		}

		public void Close()
		{
			gameObject.SetActive(false);
		}
	}
}
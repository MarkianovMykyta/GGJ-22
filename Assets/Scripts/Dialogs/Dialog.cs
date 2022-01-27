using UnityEngine;

namespace Dialogs
{
	[CreateAssetMenu(fileName = "Dialog", menuName = "Scriptables/Dialog")]
	public class Dialog : ScriptableObject
	{
		public string[] Parts;
	}
}
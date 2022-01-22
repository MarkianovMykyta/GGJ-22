using Characters;
using UnityEngine;

namespace Enviroment
{
    public class WaypointManager : MonoBehaviour
    {
        public CharacterWaypointPair[] _characterWaypoints;

        public WaypointPath GetWaypointsByCharacter(Character character)
        {
            for (int i = 0; i < _characterWaypoints.Length; i++)
            {
                if (_characterWaypoints[i].Character.Equals(character))
                {
                    return _characterWaypoints[i].Path;
                }
            }

            throw new System.Exception($"Not way for this character {character}");
        }
    }
}
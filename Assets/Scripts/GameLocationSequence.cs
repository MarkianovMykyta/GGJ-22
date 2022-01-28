using System;
using System.Collections;
using UnityEngine;

[CreateAssetMenu(fileName = "LocationSequence_", menuName = "GameData/Create Game Location Sequence")]
public class GameLocationSequence : ScriptableObject
{
    public LocationStagePair[] LocationSequence;

    public LocationType GetNextLocation(Checkpoint lastCheckpoint)
    {
        for (int i = 0; i < LocationSequence.Length-1; i++)
        {
            LocationStagePair locationStagePair = LocationSequence[i];
            if (locationStagePair.Location == lastCheckpoint.LocationType && locationStagePair.LocationState == lastCheckpoint.LocationState)
            {
                return LocationSequence[i + 1].Location;
            }
        }

        Debug.LogWarning("Location is End");
        return LocationType.None;
    }
}

[System.Serializable]
public class LocationStagePair
{
    public LocationType Location;
    public LocationState LocationState;
}

public enum LocationState
{
    Stage_1,
    Stage_2,
    Stage_3,
    Stage_4,
}

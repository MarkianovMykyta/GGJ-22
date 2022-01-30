using System.Collections.Generic;
using UnityEngine;

namespace Characters.Crowd
{
	public class PeopleSpawner : MonoBehaviour
	{
		[SerializeField] private Silhouette[] _silhouettes;
		[SerializeField] private Transform[] _spawnPoints;
		[SerializeField] private int _numberOfPeople;
		[SerializeField] private float _distanceToTravel;
		[SerializeField] private float _maxMoveSpeed;
		[SerializeField] private float _minMoveSpeed;

		private List<Silhouette> _activePeople;

		private void Awake()
		{
			SpawnPeople();
		}

		private void SpawnPeople()
		{
			_activePeople = new List<Silhouette>(_numberOfPeople);
			
			for (var i = 0; i < _numberOfPeople; i++)
			{
				var randIndex = Random.Range(0, _silhouettes.Length);
				var silhouette = Instantiate(_silhouettes[randIndex]);
				silhouette.TravelComplete += OnTravelComplete;
				_activePeople.Add(silhouette);
				
				OnTravelComplete(silhouette);
			}
		}

		private Transform GetRandomSpawnPoint()
		{
			var randIndex = Random.Range(0, _spawnPoints.Length);
			return _spawnPoints[randIndex];
		}

		private void OnTravelComplete(Silhouette silhouette)
		{
			var spawner = GetRandomSpawnPoint();
			
			silhouette.Init(Random.Range(_minMoveSpeed, _maxMoveSpeed), spawner.forward, spawner.position, _distanceToTravel);
		}
	}
}
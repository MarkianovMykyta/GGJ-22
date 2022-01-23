using Contexts;
using System.Collections;
using UnityEngine;

namespace Characters.Enemies.States
{
    public class LocalInspection : PatrolState
    {
        private int _lookBackID = Animator.StringToHash("LookBack");

        private Coroutine _inspectationRoutine;
        private Coroutine _fov;

        public LocalInspection(Enemy enemy, StateMachine stateMachine, Context context) : base(enemy, stateMachine, context)
        {
        }

        public override void Enter()
        {
            _enemy.Animator.SetBool(_lookBackID, true);
            _inspectationRoutine = _enemy.StartCoroutine(InpectectationRoutine());
        }

        public override void Exit()
        {
            _enemy.Animator.SetBool(_lookBackID, false);
            _enemy.StopCoroutine(_fov);
            _enemy.StopCoroutine(_inspectationRoutine);
        }

        public override void Update()
        {
        }


        private IEnumerator InpectectationRoutine()
        {
            _fov = _enemy.StartCoroutine(FOVRoutine());
            yield return new WaitForSeconds(4f);
            stateMachine.ChangeState(new PatrolState(_enemy, stateMachine, context));
        }
    }
}
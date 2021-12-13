using UnityEngine;
using System.Collections;

namespace Pathfinding {
	/// <summary>
	/// Sets the destination of an AI to the position of a specified object.
	/// This component should be attached to a GameObject together with a movement script such as AIPath, RichAI or AILerp.
	/// This component will then make the AI move towards the <see cref="target"/> set on this component.
	///
	/// See: <see cref="Pathfinding.IAstarAI.destination"/>
	///
	/// [Open online documentation to see images]
	/// </summary>
	[UniqueComponent(tag = "ai.destination")]
	[HelpURL("http://arongranberg.com/astar/docs/class_pathfinding_1_1_a_i_destination_setter.php")]
	public class AIDestinationSetter : VersionedMonoBehaviour {
		/// <summary>The object that the AI should move to</summary>
		public Transform target;
		public float speed;
		public float checkJarak;
		public float attackJarak;

		public bool muter;
		public LayerMask whatIsPlayer;
		
		public Transform[] Path;
		public int currentPoint;
		public Transform currentGoal;
		public float roundingDistance;
		/* public NavMeshAgent agent; */

		//private Rigidbody2D rb;
		private Animator anim;
		private Vector2 movement;
		private Vector2 ptroll;
		public Vector3 dir;
		public Vector3 dir1;
		public Vector3 temp;
		AudioSource audioSrc;
		private bool isInChaseRange;
		private bool isInAttackRange;
		bool isMoving = false;
		IAstarAI ai;

		void OnEnable () {
			ai = GetComponent<IAstarAI>();
			// Update the destination right before searching for a path as well.
			// This is enough in theory, but this script will also update the destination every
			// frame as the destination is used for debugging and may be used for other things by other
			// scripts as well. So it makes sense that it is up to date every frame.
			if (ai != null) ai.onSearchPath += Update;
		}

		void OnDisable () {
			if (ai != null) ai.onSearchPath -= Update;
		}

		private void Start()
		{
			anim = GetComponent<Animator>();
			audioSrc = GetComponent<AudioSource>();
			target = GameObject.FindWithTag("Player").transform;
		}

		/// <summary>Updates the AI's destination every frame</summary>
		void Update () {
			anim.SetBool("ngejar", true);

			isInChaseRange = Physics2D.OverlapCircle(transform.position, checkJarak, whatIsPlayer);
			isInAttackRange = Physics2D.OverlapCircle(transform.position, attackJarak, whatIsPlayer);
			temp = Path[currentPoint].position;

			dir = target.position - transform.position;
			float angle = Mathf.Atan2(dir.y, dir.x) * Mathf.Rad2Deg;
			dir.Normalize();
			movement = dir;

			if (isInChaseRange)
				isMoving = true;
			else
				isMoving = false;
			if (isMoving)
			{
				if (!audioSrc.isPlaying)
					audioSrc.Play();
			}
			else
				audioSrc.Stop();

			dir1 = Path[currentPoint].position - transform.position;
			if (muter && isInChaseRange) {
				anim.SetFloat("X", dir.x);
				anim.SetFloat("Y", dir.y);
			}
			else if (muter && !isInChaseRange) {
				anim.SetFloat("X", dir1.x);
				anim.SetFloat("Y", dir1.y);
			}

			if (target != null && ai != null) {
				/* if (srr.isInChaseRange && !srr.isInAttackRange) {
					ai.destination = target.position;
				}
				else if (!srr.isInChaseRange && !srr.isInAttackRange) {
					ai.destination = srr.temp;
				} */

			}
		}

		private void FixedUpdate() {
			if (isInChaseRange && !isInAttackRange) {
					ai.destination = target.position;
			}
			else if (!isInChaseRange && !isInAttackRange) {
				PlsWork();
			}
		}

		private void ChangeGoal() {
			if (currentPoint == Path.Length - 1) {
				currentPoint = 0;
				currentGoal = Path[0];
			}
			else {
				currentPoint++;
				currentGoal = Path[currentPoint];
			}
		}

		private void PlsWork() {
			if (Vector3.Distance(transform.position, Path[currentPoint].position) > roundingDistance) {
				ai.destination = Path[currentPoint].position;
			}
			else {
				ChangeGoal();
			}
    	}
	}
}

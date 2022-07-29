using UnityEngine;
using System.Collections;

namespace Pathfinding
{
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
	public class DestinationAICustom : VersionedMonoBehaviour
	{
		[SerializeField]
		private float stopDistance = 10;
		/// <summary>The object that the AI should move to</summary>
		IAstarAI ai;
		private GameObject player;

		public GameObject spawnSubject;
		public Animator animator;
		public bool isSpawner;

		[SerializeField]
		private int maxSpawn = 0;
		private int currSpawn = 0;
		private float nextSpawnTime = 0.0f;
		[SerializeField]
		private float spawnRate = 5.0f;

		private void Start()
		{
			player = GameObject.Find("Robot");
		}


		/// <summary>Updates the AI's destination every frame</summary>
		void Update()
		{

			//calcolo distanza auto-robot
			//float distanzaX = (this.GetComponent<Transform>().position.x - player.GetComponent<Transform>().position.x);
			//float distanzaY = (this.GetComponent<Transform>().position.y - player.GetComponent<Transform>().position.y);
			//float distanza = Mathf.Sqrt(distanzaX * distanzaX + distanzaY * distanzaY);

			if (player.GetComponent<Transform>().position != null && ai != null) ai.destination = player.GetComponent<Transform>().position;
			float distance = ai.remainingDistance;


			if (distance <= stopDistance)
			{
				if (isSpawner && currSpawn < maxSpawn && Time.time > nextSpawnTime)
				{
					//aggiorno il timer
					nextSpawnTime = Time.time + spawnRate;

					animator.SetTrigger("Open");
					currSpawn += 1;

					Vector3 spawnPos = gameObject.GetComponent<Transform>().position;
					float rot = gameObject.GetComponent<Rigidbody2D>().rotation;

					float dis = 1;
					spawnPos.x = spawnPos.x + dis * Mathf.Cos(rot * Mathf.Deg2Rad);
					spawnPos.y = spawnPos.y + dis * Mathf.Sin(rot * Mathf.Deg2Rad);
					Instantiate(spawnSubject, spawnPos, new Quaternion(0, 0, rot, 0));
				}

				this.ai.isStopped = true;

				//this.enabled = false;
			}

			if (distance > stopDistance) this.ai.isStopped = false;
		}

		void OnEnable()
		{
			ai = GetComponent<IAstarAI>();
			if (ai != null) ai.onSearchPath += Update;
		}

		void OnDisable()
		{
			if (ai != null) ai.onSearchPath -= Update;
		}
	}
}

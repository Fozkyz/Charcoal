using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Charcoal.Components
{
	public class Transform : Component
	{

		public GameObject GameObject { get; set; }

		public List<Transform> Transforms = new List<Transform>();

		public Transform Root { get; set; }

		public Transform Parent { get; set; }

		public void Awake()
		{

		}
	}
}

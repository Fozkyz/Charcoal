using Charcoal.Components;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Charcoal.Application
{
	public class Scene : GameObject
	{
		public static Scene Current { get; internal set; }

		internal protected List<GameObject> _gameObjects;

		public Scene()
			: base()
		{
			Name = "Scene-" + ID;

			_transform.Root = _transform;
			_gameObjects = new List<GameObject>();
			_scene = this;
		}

		public override void Initialize()
		{
			_initialized = true;

			foreach (var gameObject in _gameObjects)
			{
				if (gameObject.Enabled)
					gameObject.Initialize();
			}
		}

		public override void Update()
		{
			base.Update();

			foreach (var gameObject in _gameObjects)
			{
				if (gameObject.Enabled)
					gameObject.Update();
			}
		}

		public override bool Add(GameObject gameObject)
		{
			return Add(gameObject, false);
		}

		public bool Add(GameObject gameObject, bool noCheck)
		{
			var canAdd = base.Add(gameObject);

			if (canAdd)
			{
				_gameObjects.Add(gameObject);
				gameObject._scene= this;
				gameObject._transform.Root = _transform;

				if (gameObject.Enabled)
				{
					CheckComponents(gameObject, ComponentChangeType.Add);
				}

				if (_initialized && !gameObject._initialized)
					gameObject.Initialize();
			}

			return canAdd;
		}

		public bool Remove(GameObject gameObject, bool noCheck)
		{
			bool canRemove = base.Remove(gameObject);

			if (canRemove)
			{
				foreach (var component in gameObject.Components)
				{
					// Checkcomponents to remove
				}

				_gameObjects.Remove(gameObject);
			}

			return canRemove;
		}

		protected void CheckComponents(GameObject gameObject, ComponentChangeType type)
		{
			foreach (var component in gameObject.Components)
			{
				CheckComponent(component, type);
			}
		}

		protected void CheckComponent(Component component, ComponentChangeType type)
		{
			if (type == ComponentChangeType.Update)
				return;

			// TODO :
			// Check if component is a certain type (renderer, collider, light...)
			// And add/remove them to list of renderers/colliders/lights...
			// To keep track of every components of these types in the scene
		}


		public enum ComponentChangeType
		{
			Add,
			Update,
			Remove
		}
	}
}

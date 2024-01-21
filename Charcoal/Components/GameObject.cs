using Charcoal.Application;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Charcoal.Components
{
	public class GameObject : IDisposable
	{
		// protected scene

		internal protected Transform _transform;

		internal protected Scene _scene;

		internal protected bool _enabled;

		internal protected bool _initialized;

		protected List<Component> _components;

		public Transform Transform
		{
			get { return _transform; }
			internal protected set { _transform = value; }
		}

		public Scene Scene
		{
			get { return _scene; }
			internal protected set { _scene = value; }
		}

		public List<Component> Components
		{
			get { return _components; }
		}

		public bool Enabled
		{
			get { return _enabled; }
			set
			{
				if (_enabled != value)
				{
					_enabled = value;
		
					if (_transform != null)
					{
						foreach (var transform in _transform.Transforms)
						{
							transform.GameObject.Enabled = _enabled;
						}
					}

					if (_enabled)
						OnEnabled();
					else
						OnDisabled();
				}
			}
		}

		public bool Initialized => _initialized;

		public string Name { get; set; }

		public string ID { get; set; }

		public GameObject()
		{
			InternalConstructor();
		}

		public GameObject(string name)
		{
			InternalConstructor(name);
		}

		private void InternalConstructor(string name = "")
		{
			if (_transform == null)
			{
				_components = new List<Component>();

				_transform = new Transform();
				_transform.GameObject = this;
				_transform.Awake();
				_components.Add(_transform);

				_enabled = true;
				_initialized = false;

				ID = "GameObject_" + Guid.NewGuid();
				Name = string.IsNullOrEmpty(name) ? ID : name;

				Scene.Current.Add(this);
			}
		}

		public void SetActive(bool active)
		{
			Enabled = active;
		}

		public virtual void Initialize()
		{
			if (!_initialized)
			{
				_initialized = true;

				foreach (var component in _components)
				{
					if (component.Enabled)
					{
						component._initialized = true;
						component.Start();
					}
				}
			}
		}

		public virtual void Update()
		{
			foreach (var component in _components)
			{
				if (component.Enabled)
					component.Update();
			}
		}

		public virtual bool Add(GameObject gameObject)
		{
			if (!_transform.Transforms.Contains(gameObject._transform) && gameObject != this)
			{
				if (this != _scene)
				{
					if (_scene == null)
						throw new InvalidOperationException("Scene is null");
					else
						_scene.Add(gameObject);
				}
				// Remove gameObject's current parent transform
				if (gameObject._transform.Parent != null)
					gameObject._transform.Parent.Transforms.Remove(gameObject._transform);

				// Add this as gameObject's parent transform
				gameObject._transform.Parent = _transform;
				gameObject._transform.Root = _transform.Root;
				_transform.Transforms.Add(gameObject._transform);
				gameObject.Enabled = _enabled;

				return true;
			}

			return false;
		}

		public virtual bool Remove(GameObject gameObject)
		{
			if (gameObject._transform.Parent == _transform)
			{
				_transform.Transforms.Remove(gameObject._transform);
				gameObject._transform.Parent = _transform.Root;

				return true;
			}

			return false;
		}

		public Component AddComponent(Component component)
		{
			if (component == null)
				return component;

			var serializedTransform = component as Transform;

			if (serializedTransform != null)
			{

			}
			else
			{
				component._gameObject = this;
				component._transform = _transform;
				component.Awake();
				_components.Add(component);
			}

			if (_initialized && !component._initialized)
			{
				component._initialized = true;
				component.Start();
			}

			return component;
		}

		public T AddComponent<T>() where T : Component, new()
		{
			var component = new T();

			return AddComponent(component) as T;
		}

		public T GetComponent<T>() where T : Component
		{
			foreach (var component in _components)
			{
				if (component is T)
					return component as T;
			}

			return null;
		}

		public List<T> GetComponents<T>() where T : Component
		{
			var list = new List<T>();
			foreach (var component in _components)
			{
				if (component is T)
					list.Add(component as T);
			}

			return list;
		}

		public T GetComponentInChildren<T>() where T : Component
		{
			foreach (var component in _components)
			{ 
				if (component is T)
					return component as T;
			}

			var transforms = _transform.Transforms;
			foreach (var transform in transforms)
			{
				var component = transform._gameObject.GetComponentInChildren<T>();
				if (component != null)
					return component;
			}

			return null;
		}

		public List<T> GetComponentsInChildren<T>() where T : Component
		{
			var list = GetComponents<T>();

			var transforms = _transform.Transforms;
			foreach (var transform in transforms)
				list.AddRange(transform._gameObject.GetComponentsInChildren<T>());

			return list;
		}

		public T GetComponentInParent<T>() where T : Component
		{
			return _transform.Parent._gameObject.GetComponent<T>();
		}

		public List<T> GetComponentsInParent<T>() where T : Component
		{
			return _transform.Parent._gameObject.GetComponents<T>();
		}

		public bool RemoveComponent(Component component)
		{
			if (component is Transform)
				return false;

			return _components.Remove(component);
		}

		public virtual void OnEnabled()
		{

		}

		public virtual void OnDisabled()
		{

		}

		public void Dispose()
		{
			foreach (var component in _components)
				component.Dispose();
		}

		public static void Destroy(GameObject go)
		{
			if (go != null)
				Scene.Current.Remove(go);
		}

		public static GameObject Find(string name)
		{
			foreach (var gameObject in Scene.Current._gameObjects)
			{
				if (gameObject.Name == name)
					return gameObject;
			}

			return null;
		}
	}
}

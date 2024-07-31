using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Charcoal.Components
{
	public abstract class Component : IDisposable
	{
		internal protected bool _enabled;
		internal protected bool _initialized;

		internal protected GameObject _gameObject;

		internal protected Transform _transform;

		public bool Enabled
		{
			get { return _enabled; }
			set
			{
				if (_enabled != value)
				{
					_enabled = value;
					if (_enabled)
						OnEnabled();
					else
						OnDisabled();
				}
			}
		}

		public bool Initialized => _initialized;

		public GameObject GameObject
		{
			get { return _gameObject; }
			internal set { _gameObject = value; }
		}

		public Transform Transform
		{
			get { return _transform; }
			internal set { _transform = value; }
		}

		public string ID { get; set; }

		public Component()
		{
			_initialized = false;
			_enabled = true;
			ID = "Component_" + Guid.NewGuid();
		}

		public virtual void OnEnabled() { }

		public virtual void OnDisabled() { }

		public virtual void Awake()
		{
			if (_transform == null)
			{
				_transform = GameObject.Transform;
			}
			// set transform if not null
		}

		public virtual void Start()
		{
			_initialized = true;
		}

		public virtual void Update() { }

		public T AddComponent<T>() where T : Component, new()
		{
			return _gameObject.AddComponent<T>();
		}

		public T GetComponent<T>() where T : Component
		{
			return _gameObject.GetComponent<T>();
		}

		public List<T> GetComponents<T>() where T : Component
		{
			return _gameObject.GetComponents<T>();
		}

		public T GetComponentInChildren<T>() where T : Component
		{
			return _gameObject.GetComponentInChildren<T>();
		}

		public List<T> GetComponentsInChildren<T>() where T : Component
		{
			return _gameObject.GetComponentsInChildren<T>();
		}

		public T GetComponentInParent<T>() where T : Component
		{
			return _gameObject.GetComponentInParent<T>();
		}

		public List<T> GetComponentsInParent<T>() where T : Component
		{
			return _gameObject.GetComponentsInParent<T>();
		}

		public virtual object Clone()
		{
			return MemberwiseClone();
		}

		public virtual void Dispose() { }
	}
}

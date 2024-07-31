using Microsoft.Xna.Framework;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Charcoal.Components
{
	public class Transform : Component
	{
		internal Matrix _worldMatrix;

		private Vector3 _localPosition;

		private Vector3 _localRotation;

		private Vector3 _localScale;

		private Transform _root;

		private Transform _parent;

		private List<Transform> _transforms;

		private bool _dirty;

		public Transform Root
		{
			get { return _root; }
			internal set { _root = value; }
		}

		public Transform Parent
		{
			get { return _parent; }
			set
			{
				if (_parent != null)
					_parent._transforms.Remove(this);

				_parent = value;

				if (_parent != null)
					_parent._transforms.Add(this);

				_dirty = true;
			}
		}

		public List<Transform> Transforms => _transforms;

		public bool Dirty
		{
			get { return _dirty; }
			set
			{
				if (!_dirty)
					_dirty = value;
			}
		}

		public Vector3 Position
		{
			get
			{
				UpdateWorldMatrix();
				return _worldMatrix.Translation;
			}
			set
			{
				UpdateWorldMatrix();
				_localPosition = Vector3.Transform(value, Matrix.Invert(_worldMatrix));
				_dirty = true;
			}
		}

		public Vector3 LocalPosition
		{
			get { return _localPosition; }
			set
			{
				_localPosition = value;
				_dirty = true;
			}
		}

		public Vector3 Rotation
		{
			get
			{
				UpdateWorldMatrix();
				_worldMatrix.Decompose(out Vector3 scale, out Quaternion r, out Vector3 translation);
				return r.ToEuler();
			}
			set
			{
				_localRotation = value;
				_dirty = true;
			}
		}

		public Quaternion Quaternion
		{
			get
			{
				UpdateWorldMatrix();
				_worldMatrix.Decompose(out Vector3 scale, out Quaternion r, out Vector3 translation);
				return r;
			}
			set
			{
				value.ToEuler(ref _localRotation);
			}
		}

		public Matrix RotationMatrix
		{
			get { return Matrix.CreateFromYawPitchRoll(_localRotation.Y, _localRotation.X, _localRotation.Z); }
		}

		public Vector3 LocalRotation
		{
			get { return _localRotation; }
			set
			{
				_localRotation = value;
				_dirty = true;
			}
		}

		public Vector3 LocalScale
		{
			get { return _localScale; }
			set
			{
				_localScale = value;
				_dirty = true;
			}
		}

		public Matrix WorldMatrix => _worldMatrix;

		public Vector3 Forward => _worldMatrix.Forward;

		public Vector3 Backward => _worldMatrix.Backward;

		public Vector3 Right => _worldMatrix.Right;

		public Vector3 Left => _worldMatrix.Left;

		public Vector3 Up => _worldMatrix.Up;

		public Vector3 Down => _worldMatrix.Down;

		public Transform () : base ()
		{
			_localPosition = Vector3.Zero;
			_localRotation = Vector3.Zero;
			_localScale = Vector3.One;
			_parent = null;
			_root = null;
			_transforms = new List<Transform>();
			_dirty = false;
			_worldMatrix = Matrix.Identity;
		}

		public void SetParent(Transform parent)
		{
			Parent = parent;
			_dirty = true;
		}

		public Transform GetChild(int index)
		{
			if (index > 0 && index < _transforms.Count) 
				return _transforms[index];

			return null;
		}

		public void Translate(float x, float y, float z)
		{
			_localPosition.X += x;
			_localPosition.Y += y;
			_localPosition.Z += z;
			_dirty = true;
		}

		public void Translate(Vector3 translation)
		{
			Translate(translation.X, translation.Y, translation.Z);
		}

		public void Rotate(float rx, float ry, float rz)
		{
			_localRotation.X += rx;
			_localRotation.Y += ry;
			_localRotation.Z += rz;
			_dirty = true;
		}

		public void Rotate(Vector3 rotation)
		{
			Rotate(rotation.X, rotation.Y, rotation.Z);
		}

		public void SetLocalPositionAndRotation(Vector3 position, Matrix matrix)
		{
			var quaternion = Quaternion.CreateFromRotationMatrix(matrix);
			
			_localPosition = position;
			_localRotation = quaternion.ToEuler();
			_dirty = true;
		}

		public void SetLocalRotation(Matrix matrix)
		{
			var quaternion = Quaternion.CreateFromRotationMatrix(matrix);
			_localRotation = quaternion.ToEuler();
			_dirty = true;
		}

		public Vector3 TransformVector(Vector3 direction)
		{
			UpdateWorldMatrix();

			return Vector3.Transform(direction, _worldMatrix);
		}

		public override void Update()
		{
			if (_dirty || !_gameObject.IsStatic)
			{
				UpdateWorldMatrix();
				_dirty = false;
			}
		}

		public void UpdateWorldMatrix()
		{
			_worldMatrix = Matrix.Identity;
			_worldMatrix *= Matrix.CreateScale(_localScale);
			_worldMatrix *= Matrix.CreateFromYawPitchRoll(_localRotation.Y, _localRotation.X, _localRotation.Z);
			_worldMatrix *= Matrix.CreateTranslation(_localPosition);

			if (_parent != null)
				_worldMatrix *= _parent._worldMatrix;
		}

		public override object Clone()
		{
			var t = new Transform();
			t._parent = _parent;
			t._root = _root;
			t._gameObject = _gameObject;

			foreach (var child in _transforms)
				t._transforms.Add(child);

			t._localPosition = _localPosition;
			t._localRotation = _localRotation;
			t._localScale = _localScale;
			t._dirty = true;

			return t;
		}
	}
}

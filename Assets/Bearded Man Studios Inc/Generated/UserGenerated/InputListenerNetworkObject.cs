using BeardedManStudios.Forge.Networking.Frame;
using BeardedManStudios.Forge.Networking.Unity;
using System;
using UnityEngine;

namespace BeardedManStudios.Forge.Networking.Generated
{
	[GeneratedInterpol("{\"inter\":[0,0,0,0,0]")]
	public partial class InputListenerNetworkObject : NetworkObject
	{
		public const int IDENTITY = 5;

		private byte[] _dirtyFields = new byte[1];

		#pragma warning disable 0067
		public event FieldChangedEvent fieldAltered;
		#pragma warning restore 0067
		private bool _moveRight;
		public event FieldEvent<bool> moveRightChanged;
		public Interpolated<bool> moveRightInterpolation = new Interpolated<bool>() { LerpT = 0f, Enabled = false };
		public bool moveRight
		{
			get { return _moveRight; }
			set
			{
				// Don't do anything if the value is the same
				if (_moveRight == value)
					return;

				// Mark the field as dirty for the network to transmit
				_dirtyFields[0] |= 0x1;
				_moveRight = value;
				hasDirtyFields = true;
			}
		}

		public void SetmoveRightDirty()
		{
			_dirtyFields[0] |= 0x1;
			hasDirtyFields = true;
		}

		private void RunChange_moveRight(ulong timestep)
		{
			if (moveRightChanged != null) moveRightChanged(_moveRight, timestep);
			if (fieldAltered != null) fieldAltered("moveRight", _moveRight, timestep);
		}
		private bool _moveLeft;
		public event FieldEvent<bool> moveLeftChanged;
		public Interpolated<bool> moveLeftInterpolation = new Interpolated<bool>() { LerpT = 0f, Enabled = false };
		public bool moveLeft
		{
			get { return _moveLeft; }
			set
			{
				// Don't do anything if the value is the same
				if (_moveLeft == value)
					return;

				// Mark the field as dirty for the network to transmit
				_dirtyFields[0] |= 0x2;
				_moveLeft = value;
				hasDirtyFields = true;
			}
		}

		public void SetmoveLeftDirty()
		{
			_dirtyFields[0] |= 0x2;
			hasDirtyFields = true;
		}

		private void RunChange_moveLeft(ulong timestep)
		{
			if (moveLeftChanged != null) moveLeftChanged(_moveLeft, timestep);
			if (fieldAltered != null) fieldAltered("moveLeft", _moveLeft, timestep);
		}
		private bool _moveDown;
		public event FieldEvent<bool> moveDownChanged;
		public Interpolated<bool> moveDownInterpolation = new Interpolated<bool>() { LerpT = 0f, Enabled = false };
		public bool moveDown
		{
			get { return _moveDown; }
			set
			{
				// Don't do anything if the value is the same
				if (_moveDown == value)
					return;

				// Mark the field as dirty for the network to transmit
				_dirtyFields[0] |= 0x4;
				_moveDown = value;
				hasDirtyFields = true;
			}
		}

		public void SetmoveDownDirty()
		{
			_dirtyFields[0] |= 0x4;
			hasDirtyFields = true;
		}

		private void RunChange_moveDown(ulong timestep)
		{
			if (moveDownChanged != null) moveDownChanged(_moveDown, timestep);
			if (fieldAltered != null) fieldAltered("moveDown", _moveDown, timestep);
		}
		private bool _moveUp;
		public event FieldEvent<bool> moveUpChanged;
		public Interpolated<bool> moveUpInterpolation = new Interpolated<bool>() { LerpT = 0f, Enabled = false };
		public bool moveUp
		{
			get { return _moveUp; }
			set
			{
				// Don't do anything if the value is the same
				if (_moveUp == value)
					return;

				// Mark the field as dirty for the network to transmit
				_dirtyFields[0] |= 0x8;
				_moveUp = value;
				hasDirtyFields = true;
			}
		}

		public void SetmoveUpDirty()
		{
			_dirtyFields[0] |= 0x8;
			hasDirtyFields = true;
		}

		private void RunChange_moveUp(ulong timestep)
		{
			if (moveUpChanged != null) moveUpChanged(_moveUp, timestep);
			if (fieldAltered != null) fieldAltered("moveUp", _moveUp, timestep);
		}
		private Vector2 _mousePosition;
		public event FieldEvent<Vector2> mousePositionChanged;
		public InterpolateVector2 mousePositionInterpolation = new InterpolateVector2() { LerpT = 0f, Enabled = false };
		public Vector2 mousePosition
		{
			get { return _mousePosition; }
			set
			{
				// Don't do anything if the value is the same
				if (_mousePosition == value)
					return;

				// Mark the field as dirty for the network to transmit
				_dirtyFields[0] |= 0x10;
				_mousePosition = value;
				hasDirtyFields = true;
			}
		}

		public void SetmousePositionDirty()
		{
			_dirtyFields[0] |= 0x10;
			hasDirtyFields = true;
		}

		private void RunChange_mousePosition(ulong timestep)
		{
			if (mousePositionChanged != null) mousePositionChanged(_mousePosition, timestep);
			if (fieldAltered != null) fieldAltered("mousePosition", _mousePosition, timestep);
		}

		protected override void OwnershipChanged()
		{
			base.OwnershipChanged();
			SnapInterpolations();
		}
		
		public void SnapInterpolations()
		{
			moveRightInterpolation.current = moveRightInterpolation.target;
			moveLeftInterpolation.current = moveLeftInterpolation.target;
			moveDownInterpolation.current = moveDownInterpolation.target;
			moveUpInterpolation.current = moveUpInterpolation.target;
			mousePositionInterpolation.current = mousePositionInterpolation.target;
		}

		public override int UniqueIdentity { get { return IDENTITY; } }

		protected override BMSByte WritePayload(BMSByte data)
		{
			UnityObjectMapper.Instance.MapBytes(data, _moveRight);
			UnityObjectMapper.Instance.MapBytes(data, _moveLeft);
			UnityObjectMapper.Instance.MapBytes(data, _moveDown);
			UnityObjectMapper.Instance.MapBytes(data, _moveUp);
			UnityObjectMapper.Instance.MapBytes(data, _mousePosition);

			return data;
		}

		protected override void ReadPayload(BMSByte payload, ulong timestep)
		{
			_moveRight = UnityObjectMapper.Instance.Map<bool>(payload);
			moveRightInterpolation.current = _moveRight;
			moveRightInterpolation.target = _moveRight;
			RunChange_moveRight(timestep);
			_moveLeft = UnityObjectMapper.Instance.Map<bool>(payload);
			moveLeftInterpolation.current = _moveLeft;
			moveLeftInterpolation.target = _moveLeft;
			RunChange_moveLeft(timestep);
			_moveDown = UnityObjectMapper.Instance.Map<bool>(payload);
			moveDownInterpolation.current = _moveDown;
			moveDownInterpolation.target = _moveDown;
			RunChange_moveDown(timestep);
			_moveUp = UnityObjectMapper.Instance.Map<bool>(payload);
			moveUpInterpolation.current = _moveUp;
			moveUpInterpolation.target = _moveUp;
			RunChange_moveUp(timestep);
			_mousePosition = UnityObjectMapper.Instance.Map<Vector2>(payload);
			mousePositionInterpolation.current = _mousePosition;
			mousePositionInterpolation.target = _mousePosition;
			RunChange_mousePosition(timestep);
		}

		protected override BMSByte SerializeDirtyFields()
		{
			dirtyFieldsData.Clear();
			dirtyFieldsData.Append(_dirtyFields);

			if ((0x1 & _dirtyFields[0]) != 0)
				UnityObjectMapper.Instance.MapBytes(dirtyFieldsData, _moveRight);
			if ((0x2 & _dirtyFields[0]) != 0)
				UnityObjectMapper.Instance.MapBytes(dirtyFieldsData, _moveLeft);
			if ((0x4 & _dirtyFields[0]) != 0)
				UnityObjectMapper.Instance.MapBytes(dirtyFieldsData, _moveDown);
			if ((0x8 & _dirtyFields[0]) != 0)
				UnityObjectMapper.Instance.MapBytes(dirtyFieldsData, _moveUp);
			if ((0x10 & _dirtyFields[0]) != 0)
				UnityObjectMapper.Instance.MapBytes(dirtyFieldsData, _mousePosition);

			// Reset all the dirty fields
			for (int i = 0; i < _dirtyFields.Length; i++)
				_dirtyFields[i] = 0;

			return dirtyFieldsData;
		}

		protected override void ReadDirtyFields(BMSByte data, ulong timestep)
		{
			if (readDirtyFlags == null)
				Initialize();

			Buffer.BlockCopy(data.byteArr, data.StartIndex(), readDirtyFlags, 0, readDirtyFlags.Length);
			data.MoveStartIndex(readDirtyFlags.Length);

			if ((0x1 & readDirtyFlags[0]) != 0)
			{
				if (moveRightInterpolation.Enabled)
				{
					moveRightInterpolation.target = UnityObjectMapper.Instance.Map<bool>(data);
					moveRightInterpolation.Timestep = timestep;
				}
				else
				{
					_moveRight = UnityObjectMapper.Instance.Map<bool>(data);
					RunChange_moveRight(timestep);
				}
			}
			if ((0x2 & readDirtyFlags[0]) != 0)
			{
				if (moveLeftInterpolation.Enabled)
				{
					moveLeftInterpolation.target = UnityObjectMapper.Instance.Map<bool>(data);
					moveLeftInterpolation.Timestep = timestep;
				}
				else
				{
					_moveLeft = UnityObjectMapper.Instance.Map<bool>(data);
					RunChange_moveLeft(timestep);
				}
			}
			if ((0x4 & readDirtyFlags[0]) != 0)
			{
				if (moveDownInterpolation.Enabled)
				{
					moveDownInterpolation.target = UnityObjectMapper.Instance.Map<bool>(data);
					moveDownInterpolation.Timestep = timestep;
				}
				else
				{
					_moveDown = UnityObjectMapper.Instance.Map<bool>(data);
					RunChange_moveDown(timestep);
				}
			}
			if ((0x8 & readDirtyFlags[0]) != 0)
			{
				if (moveUpInterpolation.Enabled)
				{
					moveUpInterpolation.target = UnityObjectMapper.Instance.Map<bool>(data);
					moveUpInterpolation.Timestep = timestep;
				}
				else
				{
					_moveUp = UnityObjectMapper.Instance.Map<bool>(data);
					RunChange_moveUp(timestep);
				}
			}
			if ((0x10 & readDirtyFlags[0]) != 0)
			{
				if (mousePositionInterpolation.Enabled)
				{
					mousePositionInterpolation.target = UnityObjectMapper.Instance.Map<Vector2>(data);
					mousePositionInterpolation.Timestep = timestep;
				}
				else
				{
					_mousePosition = UnityObjectMapper.Instance.Map<Vector2>(data);
					RunChange_mousePosition(timestep);
				}
			}
		}

		public override void InterpolateUpdate()
		{
			if (IsOwner)
				return;

			if (moveRightInterpolation.Enabled && !moveRightInterpolation.current.UnityNear(moveRightInterpolation.target, 0.0015f))
			{
				_moveRight = (bool)moveRightInterpolation.Interpolate();
				//RunChange_moveRight(moveRightInterpolation.Timestep);
			}
			if (moveLeftInterpolation.Enabled && !moveLeftInterpolation.current.UnityNear(moveLeftInterpolation.target, 0.0015f))
			{
				_moveLeft = (bool)moveLeftInterpolation.Interpolate();
				//RunChange_moveLeft(moveLeftInterpolation.Timestep);
			}
			if (moveDownInterpolation.Enabled && !moveDownInterpolation.current.UnityNear(moveDownInterpolation.target, 0.0015f))
			{
				_moveDown = (bool)moveDownInterpolation.Interpolate();
				//RunChange_moveDown(moveDownInterpolation.Timestep);
			}
			if (moveUpInterpolation.Enabled && !moveUpInterpolation.current.UnityNear(moveUpInterpolation.target, 0.0015f))
			{
				_moveUp = (bool)moveUpInterpolation.Interpolate();
				//RunChange_moveUp(moveUpInterpolation.Timestep);
			}
			if (mousePositionInterpolation.Enabled && !mousePositionInterpolation.current.UnityNear(mousePositionInterpolation.target, 0.0015f))
			{
				_mousePosition = (Vector2)mousePositionInterpolation.Interpolate();
				//RunChange_mousePosition(mousePositionInterpolation.Timestep);
			}
		}

		private void Initialize()
		{
			if (readDirtyFlags == null)
				readDirtyFlags = new byte[1];

		}

		public InputListenerNetworkObject() : base() { Initialize(); }
		public InputListenerNetworkObject(NetWorker networker, INetworkBehavior networkBehavior = null, int createCode = 0, byte[] metadata = null) : base(networker, networkBehavior, createCode, metadata) { Initialize(); }
		public InputListenerNetworkObject(NetWorker networker, uint serverId, FrameStream frame) : base(networker, serverId, frame) { Initialize(); }

		// DO NOT TOUCH, THIS GETS GENERATED PLEASE EXTEND THIS CLASS IF YOU WISH TO HAVE CUSTOM CODE ADDITIONS
	}
}

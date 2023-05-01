using Unity.Collections;
using Unity.Netcode;
using UnityEngine;

public class PlayerNetwork : NetworkBehaviour
{
    private const string IS_JUMPING = "IsJumping";
    private const string IS_GROUNDED = "IsGrounded";
    private const string IS_FALLING = "IsFalling";
    private const string IS_MOVING = "IsMoving";
    private const string IS_CROUCHING = "IsCrouching";
    private const string FORWARD = "forward";
    private const string STRAFE = "strafe";

    private Vector3 _vel;
    private float _rotVel;

    private struct PlayerNetworkData : INetworkSerializable
    {
        private float _x, _y, _z;
        private float _modelX, _modelY, _modelZ;
        private float _yRot;
        private bool _isFalling, _isJumping, _isGrounded, _isMoving, _isCrouching;
        private float _forward, _strafe;

        private FixedString64Bytes _playerName;

        internal Vector3 Position
        {
            get => new Vector3(_x, _y, _z);
            set
            {
                _x = value.x;
                _y = value.y;
                _z = value.z;
            }
        }

        internal Vector3 ModelPosition
        {
            get => new Vector3(_modelX, _modelY, _modelZ);
            set
            {
                _modelX = value.x;
                _modelY = value.y;
                _modelZ = value.z;
            }
        }

        internal Vector3 Rotation
        {
            get => new Vector3(0f, _yRot, 0f);
            set => _yRot = value.y;
        }

        internal bool IsJumping
        {
            get => _isJumping;
            set => _isJumping = value;
        }
        internal bool IsFalling
        {
            get => _isFalling;
            set => _isFalling = value;
        }
        internal bool IsGrounded
        {
            get => _isGrounded;
            set => _isGrounded = value;
        }
        internal bool IsMoving
        {
            get => _isMoving;
            set => _isMoving = value;
        }

        internal bool IsCrouching
        {
            get => _isCrouching;
            set => _isCrouching = value;
        }

        internal float Forward
        {
            get => _forward;
            set => _forward = value;
        }

        internal float Strafe
        {
            get => _strafe;
            set => _strafe = value;
        }

        internal string PlayerName
        {
            get => _playerName.ToString();
            set => _playerName = value;
        }

        void INetworkSerializable.NetworkSerialize<T>(BufferSerializer<T> serializer)
        {
            serializer.SerializeValue(ref _x);
            serializer.SerializeValue(ref _y);
            serializer.SerializeValue(ref _z);
            serializer.SerializeValue(ref _modelX);
            serializer.SerializeValue(ref _modelY);
            serializer.SerializeValue(ref _modelZ);

            serializer.SerializeValue(ref _yRot);

            serializer.SerializeValue(ref _isFalling);
            serializer.SerializeValue(ref _isGrounded);
            serializer.SerializeValue(ref _isJumping);
            serializer.SerializeValue(ref _isMoving);
            serializer.SerializeValue(ref _isCrouching);
            serializer.SerializeValue(ref _forward);
            serializer.SerializeValue(ref _strafe);

            serializer.SerializeValue(ref _playerName);
        }
    }

    private NetworkVariable<PlayerNetworkData> _netData = new NetworkVariable<PlayerNetworkData>(writePerm: NetworkVariableWritePermission.Server);

    [SerializeField]
    private Animator animator;
    [SerializeField]
    private Transform model;

    private void Update()
    {
        if (IsOwner)
        {
            var playerData = new PlayerNetworkData()
            {
                Position = transform.position,
                ModelPosition = model.localPosition,
                Rotation = transform.rotation.eulerAngles,
                IsFalling = animator.GetBool(IS_FALLING),
                IsJumping = animator.GetBool(IS_JUMPING),
                IsGrounded = animator.GetBool(IS_GROUNDED),
                IsMoving = animator.GetBool(IS_MOVING),
                IsCrouching = animator.GetBool(IS_CROUCHING),
                Forward = animator.GetFloat(FORWARD),
                Strafe = animator.GetFloat(STRAFE),
                PlayerName = gameObject.name
            };

            if (IsServer) _netData.Value = playerData;
            else
            {
                TransmitDataServerRpc(playerData);
            }
        }
        else
        {
            transform.position = Vector3.SmoothDamp(transform.position, _netData.Value.Position, ref _vel, 0.1f);
            model.localPosition = _netData.Value.ModelPosition;
            transform.rotation = Quaternion.Euler(0, Mathf.SmoothDampAngle(transform.rotation.eulerAngles.y, _netData.Value.Rotation.y, ref _rotVel, 0.1f), 0);
            animator.SetBool(IS_FALLING, _netData.Value.IsFalling);
            animator.SetBool(IS_JUMPING, _netData.Value.IsJumping);
            animator.SetBool(IS_GROUNDED, _netData.Value.IsGrounded);
            animator.SetBool(IS_MOVING, _netData.Value.IsMoving);
            animator.SetBool(IS_CROUCHING, _netData.Value.IsCrouching);
            animator.SetFloat(FORWARD, _netData.Value.Forward);
            animator.SetFloat(STRAFE, _netData.Value.Strafe);
            gameObject.name = _netData.Value.PlayerName;
        }
    }

    [ServerRpc]
    private void TransmitDataServerRpc(PlayerNetworkData playerData)
    {
        _netData.Value = playerData;
    }
}
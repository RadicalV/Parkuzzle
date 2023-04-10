using Unity.Netcode;
using UnityEngine;

public class PlayerNetwork : NetworkBehaviour
{
    private const string IS_JUMPING = "IsJumping";
    private const string IS_GROUNDED = "IsGrounded";
    private const string IS_FALLING = "IsFalling";
    private const string IS_MOVING = "IsMoving";
    private const string FORWARD = "forward";
    private const string STRAFE = "strafe";

    private struct PlayerNetworkData : INetworkSerializable
    {
        private float _x, _y, _z;
        private float _yRot;
        private bool _isFalling, _isJumping, _isGrounded, _isMoving;
        private float _forward, _strafe;

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

        void INetworkSerializable.NetworkSerialize<T>(BufferSerializer<T> serializer)
        {
            serializer.SerializeValue(ref _x);
            serializer.SerializeValue(ref _y);
            serializer.SerializeValue(ref _z);

            serializer.SerializeValue(ref _yRot);

            serializer.SerializeValue(ref _isFalling);
            serializer.SerializeValue(ref _isGrounded);
            serializer.SerializeValue(ref _isJumping);
            serializer.SerializeValue(ref _isMoving);
            serializer.SerializeValue(ref _forward);
            serializer.SerializeValue(ref _strafe);
        }
    }

    private NetworkVariable<PlayerNetworkData> _netData = new NetworkVariable<PlayerNetworkData>(writePerm: NetworkVariableWritePermission.Server);
    private Animator animator;

    private void Start()
    {
        animator = GetComponentInChildren<Animator>();
    }

    private void Update()
    {
        if (IsOwner)
        {
            var playerData = new PlayerNetworkData()
            {
                Position = transform.position,
                Rotation = transform.rotation.eulerAngles,
                IsFalling = animator.GetBool(IS_FALLING),
                IsJumping = animator.GetBool(IS_JUMPING),
                IsGrounded = animator.GetBool(IS_GROUNDED),
                IsMoving = animator.GetBool(IS_MOVING),
                Forward = animator.GetFloat(FORWARD),
                Strafe = animator.GetFloat(STRAFE)
            };

            if (IsServer) _netData.Value = playerData;
            else
            {
                TransmitDataServerRpc(playerData);
            }
        }
        else
        {
            transform.position = _netData.Value.Position;
            transform.rotation = Quaternion.Euler(_netData.Value.Rotation);
            animator.SetBool(IS_FALLING, _netData.Value.IsFalling);
            animator.SetBool(IS_JUMPING, _netData.Value.IsJumping);
            animator.SetBool(IS_GROUNDED, _netData.Value.IsGrounded);
            animator.SetBool(IS_MOVING, _netData.Value.IsMoving);
            animator.SetFloat(FORWARD, _netData.Value.Forward);
            animator.SetFloat(STRAFE, _netData.Value.Strafe);
        }
    }

    [ServerRpc]
    private void TransmitDataServerRpc(PlayerNetworkData playerData)
    {
        _netData.Value = playerData;
    }
}
using System;
using System.Collections.Generic;
using System.Linq;
using ServerRest;
using UnityEngine;

namespace Meta.BaseRedactor
{
    public class ShieldsSetter : MonoBehaviour
    {
        [Header("Links")]
        [SerializeField] private PlayerBaseMenu playerBaseMenu;
        [SerializeField] private ParticleSystem shieldParticleDespawn;
        [SerializeField] private ParticleSystem shieldParticleSpawn;
        [SerializeField] private WallsContainer wallsContainer;
        [SerializeField] private WallsSpawner wallsSpawner;

        [Header("Monitoring")]
        [SerializeField] private bool[] shields;
        [SerializeField] private ParticleSystem[] shieldsParticles;


        public int ShieldsSettedCount => shields.Count(s => s);

        private int _maxShields;
        private Camera _camera;

        public void InitSetter()
        {
            if (shieldsParticles != null)
                foreach (var shieldsParticle in shieldsParticles)
                    Destroy(shieldsParticle.gameObject);

            shields = new bool[wallsContainer.WallsCount];
            shieldsParticles = new ParticleSystem[wallsContainer.WallsCount];
            playerBaseMenu.UpdateShieldsRemaining(ShieldsSettedCount);
            var mask = ServerGameStatus.Instance.playerData.baseMask;
            ServerGameStatus.Instance.modifiedPlayerBase = mask;
            UpdateShieldsStatusFromGameStatus();

        }

        private void UpdateShieldsStatusFromGameStatus()
        {
            var mask = ServerGameStatus.Instance.playerData.baseMask;
            for (int i = shields.Length - 1; i >= 0; i--)
            {
                RemoveShield(i);
            }
            for (int i = shields.Length - 1; i >= 0; i--)
            {
                if (mask % 2 == 1)
                {
                    SetShield(i);
                }
                mask /= 2;
            }
            playerBaseMenu.UpdateShieldsRemaining(ShieldsSettedCount);
            UpdateShieldState();
        }

        private void OnEnable()
        {
            wallsSpawner.WallsSpawned.AddListener(OnWallsSpawned);
            playerBaseMenu.RespawnBaseFromStatusRequestEvent.AddListener(OnShieldsRespawnRequest);
        }

        private void OnShieldsRespawnRequest(RespawnBaseFromStatusRequestArgs arg0)
        {
            UpdateShieldsStatusFromGameStatus();
        }

        private void OnDisable()
        {
            wallsSpawner.WallsSpawned.RemoveListener(OnWallsSpawned);
            playerBaseMenu.RespawnBaseFromStatusRequestEvent.RemoveListener(OnShieldsRespawnRequest);
        }

        private void Awake()
        {
            _camera = Camera.main;
        }


        private void Update()
        {
            if (Input.GetMouseButtonDown(0))
            {
                if (RayCastFindAttackLine(out var wallView))
                {
                    var id = wallsContainer[wallView];
                    if (shields[id]) RemoveShield(id);
                    else SetShield(id);
                    playerBaseMenu.UpdateShieldsRemaining(ShieldsSettedCount);
                }
            }
        }


        private void OnWallsSpawned()
        {
            InitSetter();
        }


        private void SetShield(int id)
        {
            if (shields[id]) return;
            if (ShieldsSettedCount >= ServerGameStatus.Instance.staticGameParams.maxShields) return;

            var ps = Instantiate(shieldParticleSpawn);
            ps.transform.position = wallsContainer[id].ParticlesSpawnPoint.position;
            shields[id] = true;
            shieldsParticles[id] = ps;
            UpdateShieldState();
        }

        private void RemoveShield(int id)
        {
            if (!shields[id]) return;

            shields[id] = false;
            var ps = Instantiate(shieldParticleDespawn);
            ps.transform.position = wallsContainer[id].ParticlesSpawnPoint.position;
            Destroy(shieldsParticles[id].gameObject);
            UpdateShieldState();
        }

        private void UpdateShieldState()
        {
            int mask = 0;
            foreach (var t in shields)
            {
                mask *= 2;
                if (t)
                {
                    mask++;
                }
            }
            ServerGameStatus.Instance.modifiedPlayerBase = mask;
        }
        private bool RayCastFindAttackLine(out WallView wallView)
        {
            wallView = null;
            RaycastHit hit;
            Ray ray = _camera.ScreenPointToRay(Input.mousePosition);

            if (Physics.Raycast(ray, out hit))
                return (hit.transform.gameObject.TryGetComponent(out wallView));

            return false;
        }
    }
}
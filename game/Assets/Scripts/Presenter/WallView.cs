using Presenter;
using UnityEngine;

public class WallView : MonoBehaviour
{
    [SerializeField] private Transform particlesSpawnPoint;
    [SerializeField] private ParticleSystem destroyParticles;
    [SerializeField] private GameObject view;

    public Transform ParticlesSpawnPoint => particlesSpawnPoint;

    public void DestroyViewStart()
    {
        if (destroyParticles)
        {
            var ps = Instantiate(destroyParticles);
            ps.transform.position = particlesSpawnPoint.position;
        }
    }

    public void AttackResultViewStart(AttackResult attackResult)
    {
        if (attackResult.IsAttackSuccess)
        {
            DestroyViewStart();
            view.SetActive(false);
            return;
        }

        var deathType = attackResult.UnitDeathType;
        if (deathType.WallDefendEffect)
        {
            var effect = Instantiate(deathType.WallDefendEffect);
            effect.transform.position = particlesSpawnPoint.position;
        }
    }

    public void ResetView()
    {
        view.SetActive(true);
    }
}
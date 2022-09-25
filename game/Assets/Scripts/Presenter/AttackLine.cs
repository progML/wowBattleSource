using Presenter;
using UnityEngine;

public class AttackLine : MonoBehaviour
{
    [Header("Links")]
    [SerializeField] private Transform attackerCornerPoint;
    [SerializeField] private Transform wallPoint;
    [SerializeField] private Transform attackerSpawnPoint;
    [SerializeField] private MeshRenderer ground;


    [Header("Monitoring")]
    [SerializeField] private WallView wallView;
    [SerializeField] private AttackerCornerView attackerCornerView;
    [SerializeField] private Material groundMaterial;


    public WallView WallView => wallView;

    public void Initialize(WallView wallViewPrefab, AttackerCornerView attackerCornerViewPrefab,
        Material groundMaterial)
    {
        wallView = Instantiate(wallViewPrefab, wallPoint);
        attackerCornerView = Instantiate(attackerCornerViewPrefab, attackerCornerPoint);
        this.groundMaterial = groundMaterial;
        ground.material = this.groundMaterial;
    }

    public void SendUnit(AttackerUnit spawnedUnit)
    {
        var startPoint = attackerSpawnPoint.position;
        spawnedUnit.transform.position = startPoint;

        var endPoint = wallPoint.position;
        endPoint.y = startPoint.y;
        spawnedUnit.StartAttack(startPoint, endPoint);
    }


    public void AttackResultViewStart(AttackResult attackResult)
    {
        wallView.AttackResultViewStart(attackResult);
    }

    public void ResetView()
    {
        wallView.ResetView();
    }
}
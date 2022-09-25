using System;
using System.Collections;
using System.Collections.Generic;
using Presenter;
using Presenter.Misc;
using UnityEngine;


public class GameFieldSpawner : MonoBehaviour
{
    [Header("Links")]
    [SerializeField] private GameFieldPrefabsConfig gameFieldPrefabsConfig;
    [SerializeField] private AttackLine defaultAttackLinePrefab;
    [SerializeField] private AttackLinesContainer attackLinesContainer;


    [Header("Settings")]
    [SerializeField] private float lineSpaceBetween;

    [ContextMenu("SpawnFieldTest10Lines")]
    public void SpawnFieldTest10Lines()
    {
        SpawnField(10);
    }


    public void SpawnField(int lineCount)
    {
        attackLinesContainer.DestroyLines();
        Vector3 spawnPosition = attackLinesContainer.LinesParent.position;
        for (int i = 0; i < lineCount; i++)
        {
            var newLine = SpawnLine();
            newLine.transform.SetParent(attackLinesContainer.LinesParent);
            newLine.transform.position = spawnPosition;
            spawnPosition += attackLinesContainer.LinesParent.right * lineSpaceBetween;
            attackLinesContainer.AddLine(newLine, i);
        }
    }


    private AttackLine SpawnLine()
    {
        var randomWallPrefab = gameFieldPrefabsConfig.wallViewPrefabs.GetRandom();
        var randomCornerView = gameFieldPrefabsConfig.AttackerCornerViewPrefabs.GetRandom();
        var randomGroundMaterial = gameFieldPrefabsConfig.GroundMaterials.GetRandom();

        var attackLine = Instantiate(defaultAttackLinePrefab);
        attackLine.Initialize(randomWallPrefab, randomCornerView, randomGroundMaterial);
        return attackLine;
    }
}
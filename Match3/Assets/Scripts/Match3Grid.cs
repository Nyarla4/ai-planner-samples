﻿using System;
using System.Collections.Generic;
using Generated.AI.Planner.StateRepresentation.Match3Plan;
using Generated.Semantic.Traits;
using Generated.Semantic.Traits.Enums;
using Match3;
using Unity.AI.Planner.Controller;
using Unity.Collections;
using UnityEngine;
using Random = UnityEngine.Random;

public class Match3Grid : MonoBehaviour
{
    const float k_DelayFillNewCells = 0.5f;
    const float k_DelayCellsToCheck = 0.3f;
    const float k_DelayNextTurn = 0.4f;

#pragma warning disable 0649
    [SerializeField]
    GameObject m_CellPrefab;

    [SerializeField]
    Player m_PlayerPrefab;

    [SerializeField]
    int m_GridSize = 8;

    [SerializeField]
    GemObject[] m_GemTypes;
#pragma warning restore 0649

    public Action<int> MoveCountChanged;
    public Action<int> GoalCountChanged;

    GemObject[,] m_GemObjects;
    Cell[,] m_CellsData;

    float m_NextBoardUpdate;
    List<(int, int)> m_CellToCheckNextUpdate = new List<(int, int)>();

    DecisionController m_DecisionController;

    public GameObject CellPrefab => m_CellPrefab;

    Game m_GameTraitData;
    int TotalMoveCount { get; set; }

    protected void Start()
    {
        Application.targetFrameRate = 60;

        InitializeWorldState();
        InitializeVisualGems();

        var player = Instantiate(m_PlayerPrefab);
        player.Grid = this;
        m_DecisionController = player.GetComponent<DecisionController>();

        m_GameTraitData = GetComponent<Game>();
        GoalCountChanged?.Invoke((int)m_GameTraitData.GoalCount);
    }

    public bool ReadyToPlay()
    {
        return Time.realtimeSinceStartup > m_NextBoardUpdate + k_DelayNextTurn;
    }

    void InitializeWorldState()
    {
        m_CellsData = new Cell[m_GridSize, m_GridSize];
        for (int x = 0; x < m_GridSize; x++)
        {
            for (int y = 0; y < m_GridSize; y++)
            {
                m_CellsData[x, y] = transform.Find(x + "_" + y).GetComponent<Cell>();
            }
        }
    }

    void InitializeVisualGems()
    {
        m_GemObjects = new GemObject[m_GridSize, m_GridSize];
        for (int x = 0; x < m_GridSize; x++)
        {
            for (int y = 0; y < m_GridSize; y++)
            {
                var gemType = m_GemTypes[(long)m_CellsData[x, y].Type];
                var gem = Instantiate(gemType, new Vector3(x, 0, y), Quaternion.identity, transform);
                gem.Initialize(x, y);
                m_GemObjects[x, y] = gem;
            }
        }
    }

    public void TryToSwapSelectedCell(GemObject gem, Vector3 targetPos)
    {
        GemObject swapTarget = null;
        var sourcePos = gem.transform.position;
        if (Math.Abs(targetPos.x - sourcePos.x) > 0.5f)
        {
            swapTarget = targetPos.x < sourcePos.x ? GetGem(gem.X - 1, gem.Y) : GetGem(gem.X + 1, gem.Y);
        }
        else if (Math.Abs(targetPos.z - sourcePos.z) > 0.5f)
        {
            swapTarget = targetPos.z < sourcePos.z ? GetGem(gem.X, gem.Y - 1) : GetGem(gem.X, gem.Y + 1);
        }

        if (swapTarget != null)
        {
            SwapGems(gem, swapTarget);
        }
        else
        {
            ResetGemPosition(gem);
        }
    }

    public void ResetGemPosition(GemObject gem)
    {
        var gemPos = gem.transform.position;
        gemPos.y = 0;
        gem.transform.position = gemPos;
    }

    public void SwapGems(int cell1X, int cell1Y, int cell2X, int cell2Y)
    {
        SwapGems(GetGem(cell1X, cell1Y), GetGem(cell2X, cell2Y));
    }

    GemObject GetGem(int x, int y)
    {
        if (x < 0 || y < 0 || x >= m_GemObjects.GetLength(0) || y >= m_GemObjects.GetLength(1))
        {
            return null;
        }

        return m_GemObjects[x, y];
    }

    void SwapGems(GemObject gem1, GemObject gem2)
    {
        TotalMoveCount++;
        MoveCountChanged?.Invoke(TotalMoveCount);

        int oldX = gem1.X;
        int oldY = gem1.Y;
        gem1.SetDestination(gem2.X, gem2.Y, gem2.transform.position);
        gem2.SetDestination(oldX, oldY, gem1.transform.position);

        m_GemObjects[gem1.X, gem1.Y] = gem1;
        m_GemObjects[gem2.X, gem2.Y] = gem2;

        m_CellsData[gem1.X, gem1.Y].Type = gem1.Type;
        m_CellsData[gem2.X, gem2.Y].Type = gem2.Type;

        m_CellToCheckNextUpdate.Add((gem1.X, gem1.Y));
        m_CellToCheckNextUpdate.Add((gem2.X, gem2.Y));

        m_NextBoardUpdate = Time.realtimeSinceStartup + k_DelayCellsToCheck;
    }

    protected void Update()
    {
        if (Time.realtimeSinceStartup > m_NextBoardUpdate)
        {
            UpdateBoard();
        }
    }

    void UpdateBoard()
    {
        if (m_CellToCheckNextUpdate.Count > 0)
        {
            bool matchFound = false;
            foreach (var (x, y) in m_CellToCheckNextUpdate)
            {
                matchFound |= CheckMatch3(x, y);
            }
            m_CellToCheckNextUpdate.Clear();

            if (matchFound)
                m_NextBoardUpdate = Time.realtimeSinceStartup + k_DelayFillNewCells;

            return;
        }

        for (int x = 0; x < m_GridSize; x++)
        {
            int firstEmptyY = -1;
            for (int y = 0; y < m_GridSize; y++)
            {
                var gem = m_GemObjects[x, y];
                if (gem == null) // empty space
                {
                    if (firstEmptyY < 0) // mark first empty space
                        firstEmptyY = y;
                }
                else if (firstEmptyY >= 0)
                {
                    m_GemObjects[x, firstEmptyY] = gem;
                    m_CellsData[x, firstEmptyY].Type = gem.Type;

                    m_GemObjects[x, y] = null;

                    gem.SetDestination(x, firstEmptyY, new Vector3(x, 0, firstEmptyY), 0.4f);

                    m_CellToCheckNextUpdate.Add((x, firstEmptyY));

                    y = firstEmptyY; // reset y position in update loop
                    firstEmptyY = -1;
                }
            }
        }

        for (int x = 0; x < m_GridSize; x++)
        {
            for (int y = 0; y < m_GridSize; y++)
            {
                if (m_GemObjects[x, y] == null)
                {
                    var gemTypeIndex = Random.Range(1, m_GemTypes.Length);
                    m_CellsData[x, y].Type = (CellType)gemTypeIndex;

                    var gemType = m_GemTypes[gemTypeIndex];
                    var gem = Instantiate(gemType, new Vector3(x, 0, y + 10), Quaternion.identity, transform);
                    gem.SetDestination(x, y, new Vector3(x, 0, y), 0.4f);
                    m_GemObjects[x, y] = gem;

                    m_CellToCheckNextUpdate.Add((x, y));
                }
            }
        }

        if (m_CellToCheckNextUpdate.Count > 0)
            m_NextBoardUpdate = Time.realtimeSinceStartup + k_DelayCellsToCheck;
    }

    bool CheckMatch3(int x, int y)
    {
        if (m_GemObjects[x, y] == null)
            return false;

        var sourceType = m_GemObjects[x, y].Type;
        var anyMatch = false;
        var consecutiveHorizontal = ConsecutiveGem(x, y, sourceType, 1, 0);
        if (consecutiveHorizontal.Count >= 2)
        {
            anyMatch = true;
            foreach (var c in consecutiveHorizontal)
            {
                DestroyGem(c);
            }
        }

        var consecutiveVertical = ConsecutiveGem(x, y, sourceType, 0, 1);
        if (consecutiveVertical.Count >= 2)
        {
            anyMatch = true;
            foreach (var c in consecutiveVertical)
            {
                DestroyGem(c);
            }
        }

        if (anyMatch)
            DestroyGem(m_GemObjects[x, y]);

        return anyMatch;
    }

    void DestroyGem(GemObject gemObject)
    {
        var goalType = m_GameTraitData.GoalType;
        if (goalType == gemObject.Type)
        {
            int currentGoal = (int)m_GameTraitData.GoalCount;
            currentGoal = Math.Max(0, currentGoal - 1);

            m_GameTraitData.GoalCount = currentGoal;
            GoalCountChanged?.Invoke(currentGoal);

            if (currentGoal == 0)
                FinishGame();
        }

        m_GemObjects[gemObject.X, gemObject.Y] = null;
        gemObject.Explode();
    }

    void FinishGame()
    {
        m_DecisionController.AutoUpdate = false;
    }

    List<GemObject> ConsecutiveGem(int x, int y, CellType sourceType, int offsetX, int offsetY)
    {
        var consecutive = new List<GemObject>();
        for (var gem = GetGem(x + offsetX, y + offsetY); gem != null && gem.Type == sourceType; gem = GetGem(gem.X + offsetX, gem.Y + offsetY))
        {
            consecutive.Add(gem);
        }

        for (var gem = GetGem(x - offsetX, y - offsetY); gem != null && gem.Type == sourceType; gem = GetGem(gem.X - offsetX, gem.Y - offsetY))
        {
            consecutive.Add(gem);
        }

        return consecutive;
    }

    void OnDrawGizmosSelected()
    {
        if (!m_DecisionController || !m_DecisionController.Initialized || m_DecisionController.CurrentStateData == null)
            return;

        var stateData = (StateData)m_DecisionController.CurrentStateData;

        var cellObjects = new NativeList<int>(64, Allocator.Temp);
        var cellIndices = stateData.GetTraitBasedObjectIndices(cellObjects, typeof(Generated.AI.Planner.StateRepresentation.Cell));
        foreach (var traitBasedObjectIndex in cellIndices)
        {
            var cell = stateData.GetTraitOnObjectAtIndex<Generated.AI.Planner.StateRepresentation.Cell>(traitBasedObjectIndex);

            switch (cell.Type)
            {
                case CellType.Blue:
                    Gizmos.color = Color.blue;
                    break;
                case CellType.Green:
                    Gizmos.color = Color.green;
                    break;
                case CellType.Purple:
                    Gizmos.color = Color.magenta;
                    break;
                case CellType.Red:
                    Gizmos.color = Color.red;
                    break;
                case CellType.Yellow:
                    Gizmos.color = Color.yellow;
                    break;
                case CellType.None:
                    Gizmos.color = Color.grey;
                    break;
            }

            var coordinate = stateData.GetTraitOnObjectAtIndex<Generated.AI.Planner.StateRepresentation.Coordinate>(traitBasedObjectIndex);
            Gizmos.DrawCube(new Vector3(coordinate.X, 0, coordinate.Y), Vector3.one * 0.2f);
        }

        cellObjects.Dispose();
    }
}

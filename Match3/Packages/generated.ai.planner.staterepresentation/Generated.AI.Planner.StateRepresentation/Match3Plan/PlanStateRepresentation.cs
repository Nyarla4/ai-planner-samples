using System;
using System.Text;
using System.Collections.Generic;
using System.Runtime.InteropServices;
using Unity.AI.Planner;
using Unity.AI.Planner.Traits;
using Unity.AI.Planner.Jobs;
using Unity.Collections;
using Unity.Collections.LowLevel.Unsafe;
using Unity.Entities;
using Unity.Jobs;
using PlanningAgent = Unity.AI.Planner.Traits.PlanningAgent;

namespace Generated.AI.Planner.StateRepresentation.Match3Plan
{
    // Plans don't share key types to enforce a specific state representation
    public struct StateEntityKey : IEquatable<StateEntityKey>, IStateKey
    {
        public Entity Entity;
        public int HashCode;

        public bool Equals(StateEntityKey other) => Entity == other.Entity;

        public bool Equals(IStateKey other) => other is StateEntityKey key && Equals(key);

        public override int GetHashCode() => HashCode;

        public override string ToString() => $"StateEntityKey ({Entity} {HashCode})";

        public string Label => $"State{Entity}";
    }

    public static class TraitArrayIndex<T> where T : struct, ITrait
    {
        public static readonly int Index = -1;

        static TraitArrayIndex()
        {
            var typeIndex = TypeManager.GetTypeIndex<T>();
            if (typeIndex == TypeManager.GetTypeIndex<Game>())
                Index = 0;
            else if (typeIndex == TypeManager.GetTypeIndex<Cell>())
                Index = 1;
            else if (typeIndex == TypeManager.GetTypeIndex<Coordinate>())
                Index = 2;
            else if (typeIndex == TypeManager.GetTypeIndex<Blocker>())
                Index = 3;
            else if (typeIndex == TypeManager.GetTypeIndex<PlanningAgent>())
                Index = 4;
        }
    }

    [StructLayout(LayoutKind.Sequential, Size=8)]
    public struct TraitBasedObject : ITraitBasedObject, IEquatable<TraitBasedObject>
    {
        public int Length => 5;

        public byte this[int i]
        {
            get
            {
                switch (i)
                {
                    case 0:
                        return GameIndex;
                    case 1:
                        return CellIndex;
                    case 2:
                        return CoordinateIndex;
                    case 3:
                        return BlockerIndex;
                    case 4:
                        return PlanningAgentIndex;
                }

                return Unset;
            }
            set
            {
                switch (i)
                {
                    case 0:
                        GameIndex = value;
                        break;
                    case 1:
                        CellIndex = value;
                        break;
                    case 2:
                        CoordinateIndex = value;
                        break;
                    case 3:
                        BlockerIndex = value;
                        break;
                    case 4:
                        PlanningAgentIndex = value;
                        break;
                }
            }
        }

        public static readonly byte Unset = Byte.MaxValue;

        public static TraitBasedObject Default => new TraitBasedObject
        {
            GameIndex = Unset,
            CellIndex = Unset,
            CoordinateIndex = Unset,
            BlockerIndex = Unset,
            PlanningAgentIndex = Unset,
        };


        public byte GameIndex;
        public byte CellIndex;
        public byte CoordinateIndex;
        public byte BlockerIndex;
        public byte PlanningAgentIndex;


        static readonly int s_GameTypeIndex = TypeManager.GetTypeIndex<Game>();
        static readonly int s_CellTypeIndex = TypeManager.GetTypeIndex<Cell>();
        static readonly int s_CoordinateTypeIndex = TypeManager.GetTypeIndex<Coordinate>();
        static readonly int s_BlockerTypeIndex = TypeManager.GetTypeIndex<Blocker>();
        static readonly int s_PlanningAgentTypeIndex = TypeManager.GetTypeIndex<PlanningAgent>();

        public bool HasSameTraits(TraitBasedObject other)
        {
            for (var i = 0; i < Length; i++)
            {
                var traitIndex = this[i];
                var otherTraitIndex = other[i];
                if (traitIndex == Unset && otherTraitIndex != Unset || traitIndex != Unset && otherTraitIndex == Unset)
                    return false;
            }
            return true;
        }

        public bool HasTraitSubset(TraitBasedObject traitSubset)
        {
            for (var i = 0; i < Length; i++)
            {
                var requiredTrait = traitSubset[i];
                if (requiredTrait != Unset && this[i] == Unset)
                    return false;
            }
            return true;
        }

        // todo - replace with more efficient subset check
        public bool MatchesTraitFilter(NativeArray<ComponentType> componentTypes)
        {
            for (int i = 0; i < componentTypes.Length; i++)
            {
                var t = componentTypes[i];
                if (t == default || t.TypeIndex == 0)
                {
                    // This seems to be necessary for Burst compilation; Doesn't happen with non-Burst compilation
                }
                else if (t.TypeIndex == s_GameTypeIndex)
                {
                    if (t.AccessModeType == ComponentType.AccessMode.Exclude ^ GameIndex == Unset)
                        return false;
                }
                else if (t.TypeIndex == s_CellTypeIndex)
                {
                    if (t.AccessModeType == ComponentType.AccessMode.Exclude ^ CellIndex == Unset)
                        return false;
                }
                else if (t.TypeIndex == s_CoordinateTypeIndex)
                {
                    if (t.AccessModeType == ComponentType.AccessMode.Exclude ^ CoordinateIndex == Unset)
                        return false;
                }
                else if (t.TypeIndex == s_BlockerTypeIndex)
                {
                    if (t.AccessModeType == ComponentType.AccessMode.Exclude ^ BlockerIndex == Unset)
                        return false;
                }
                else if (t.TypeIndex == s_PlanningAgentTypeIndex)
                {
                    if (t.AccessModeType == ComponentType.AccessMode.Exclude ^ PlanningAgentIndex == Unset)
                        return false;
                }
                else
                    throw new ArgumentException($"Incorrect trait type used in object query: {t}");
            }

            return true;
        }

        public bool MatchesTraitFilter(ComponentType[] componentTypes)
        {
            for (int i = 0; i < componentTypes.Length; i++)
            {
                var t = componentTypes[i];
                if (t == default || t == null || t.TypeIndex == 0)
                {
                    // This seems to be necessary for Burst compilation; Doesn't happen with non-Burst compilation
                }
                else if (t.TypeIndex == s_GameTypeIndex)
                {
                    if (t.AccessModeType == ComponentType.AccessMode.Exclude ^ GameIndex == Unset)
                        return false;
                }
                else if (t.TypeIndex == s_CellTypeIndex)
                {
                    if (t.AccessModeType == ComponentType.AccessMode.Exclude ^ CellIndex == Unset)
                        return false;
                }
                else if (t.TypeIndex == s_CoordinateTypeIndex)
                {
                    if (t.AccessModeType == ComponentType.AccessMode.Exclude ^ CoordinateIndex == Unset)
                        return false;
                }
                else if (t.TypeIndex == s_BlockerTypeIndex)
                {
                    if (t.AccessModeType == ComponentType.AccessMode.Exclude ^ BlockerIndex == Unset)
                        return false;
                }
                else if (t.TypeIndex == s_PlanningAgentTypeIndex)
                {
                    if (t.AccessModeType == ComponentType.AccessMode.Exclude ^ PlanningAgentIndex == Unset)
                        return false;
                }
                else
                    throw new ArgumentException($"Incorrect trait type used in object query: {t}");
            }

            return true;
        }

        public bool Equals(TraitBasedObject other)
        {

                return GameIndex == other.GameIndex && CellIndex == other.CellIndex && CoordinateIndex == other.CoordinateIndex && BlockerIndex == other.BlockerIndex && PlanningAgentIndex == other.PlanningAgentIndex;
        }

        public override bool Equals(object obj)
        {
            return obj is TraitBasedObject other && Equals(other);
        }

        public override int GetHashCode()
        {
            unchecked
            {

                    var hashCode = GameIndex.GetHashCode();
                    
                     hashCode = (hashCode * 397) ^ CellIndex.GetHashCode();
                     hashCode = (hashCode * 397) ^ CoordinateIndex.GetHashCode();
                     hashCode = (hashCode * 397) ^ BlockerIndex.GetHashCode();
                     hashCode = (hashCode * 397) ^ PlanningAgentIndex.GetHashCode();
                return hashCode;
            }
        }
    }

    public struct StateData : ITraitBasedStateData<TraitBasedObject, StateData>
    {
        public Entity StateEntity;
        public DynamicBuffer<TraitBasedObject> TraitBasedObjects;
        public DynamicBuffer<TraitBasedObjectId> TraitBasedObjectIds;

        public DynamicBuffer<Game> GameBuffer;
        public DynamicBuffer<Cell> CellBuffer;
        public DynamicBuffer<Coordinate> CoordinateBuffer;
        public DynamicBuffer<Blocker> BlockerBuffer;
        public DynamicBuffer<PlanningAgent> PlanningAgentBuffer;

        static readonly int s_GameTypeIndex = TypeManager.GetTypeIndex<Game>();
        static readonly int s_CellTypeIndex = TypeManager.GetTypeIndex<Cell>();
        static readonly int s_CoordinateTypeIndex = TypeManager.GetTypeIndex<Coordinate>();
        static readonly int s_BlockerTypeIndex = TypeManager.GetTypeIndex<Blocker>();
        static readonly int s_PlanningAgentTypeIndex = TypeManager.GetTypeIndex<PlanningAgent>();

        public StateData(ExclusiveEntityTransaction transaction, Entity stateEntity)
        {
            StateEntity = stateEntity;
            TraitBasedObjects = transaction.GetBuffer<TraitBasedObject>(stateEntity);
            TraitBasedObjectIds = transaction.GetBuffer<TraitBasedObjectId>(stateEntity);

            GameBuffer = transaction.GetBuffer<Game>(stateEntity);
            CellBuffer = transaction.GetBuffer<Cell>(stateEntity);
            CoordinateBuffer = transaction.GetBuffer<Coordinate>(stateEntity);
            BlockerBuffer = transaction.GetBuffer<Blocker>(stateEntity);
            PlanningAgentBuffer = transaction.GetBuffer<PlanningAgent>(stateEntity);
        }

        public StateData(int jobIndex, EntityCommandBuffer.ParallelWriter entityCommandBuffer, Entity stateEntity)
        {
            StateEntity = stateEntity;
            TraitBasedObjects = entityCommandBuffer.AddBuffer<TraitBasedObject>(jobIndex, stateEntity);
            TraitBasedObjectIds = entityCommandBuffer.AddBuffer<TraitBasedObjectId>(jobIndex, stateEntity);

            GameBuffer = entityCommandBuffer.AddBuffer<Game>(jobIndex, stateEntity);
            CellBuffer = entityCommandBuffer.AddBuffer<Cell>(jobIndex, stateEntity);
            CoordinateBuffer = entityCommandBuffer.AddBuffer<Coordinate>(jobIndex, stateEntity);
            BlockerBuffer = entityCommandBuffer.AddBuffer<Blocker>(jobIndex, stateEntity);
            PlanningAgentBuffer = entityCommandBuffer.AddBuffer<PlanningAgent>(jobIndex, stateEntity);
        }

        public StateData Copy(int jobIndex, EntityCommandBuffer.ParallelWriter entityCommandBuffer)
        {
            var stateEntity = entityCommandBuffer.Instantiate(jobIndex, StateEntity);
            var traitBasedObjects = entityCommandBuffer.SetBuffer<TraitBasedObject>(jobIndex, stateEntity);
            traitBasedObjects.CopyFrom(TraitBasedObjects.AsNativeArray());
            var traitBasedObjectIds = entityCommandBuffer.SetBuffer<TraitBasedObjectId>(jobIndex, stateEntity);
            traitBasedObjectIds.CopyFrom(TraitBasedObjectIds.AsNativeArray());

            var Games = entityCommandBuffer.SetBuffer<Game>(jobIndex, stateEntity);
            Games.CopyFrom(GameBuffer.AsNativeArray());
            var Cells = entityCommandBuffer.SetBuffer<Cell>(jobIndex, stateEntity);
            Cells.CopyFrom(CellBuffer.AsNativeArray());
            var Coordinates = entityCommandBuffer.SetBuffer<Coordinate>(jobIndex, stateEntity);
            Coordinates.CopyFrom(CoordinateBuffer.AsNativeArray());
            var Blockers = entityCommandBuffer.SetBuffer<Blocker>(jobIndex, stateEntity);
            Blockers.CopyFrom(BlockerBuffer.AsNativeArray());
            var PlanningAgents = entityCommandBuffer.SetBuffer<PlanningAgent>(jobIndex, stateEntity);
            PlanningAgents.CopyFrom(PlanningAgentBuffer.AsNativeArray());

            return new StateData
            {
                StateEntity = stateEntity,
                TraitBasedObjects = traitBasedObjects,
                TraitBasedObjectIds = traitBasedObjectIds,

                GameBuffer = Games,
                CellBuffer = Cells,
                CoordinateBuffer = Coordinates,
                BlockerBuffer = Blockers,
                PlanningAgentBuffer = PlanningAgents,
            };
        }

        public void AddObject(NativeArray<ComponentType> types, out TraitBasedObject traitBasedObject, TraitBasedObjectId objectId, FixedString64 name = default)
        {
            traitBasedObject = TraitBasedObject.Default;
#if DEBUG
            objectId.Name.CopyFrom(name);
#endif

            for (int i = 0; i < types.Length; i++)
            {
                var t = types[i];
                if (t.TypeIndex == s_GameTypeIndex)
                {
                    GameBuffer.Add(default);
                    traitBasedObject.GameIndex = (byte) (GameBuffer.Length - 1);
                }
                else if (t.TypeIndex == s_CellTypeIndex)
                {
                    CellBuffer.Add(default);
                    traitBasedObject.CellIndex = (byte) (CellBuffer.Length - 1);
                }
                else if (t.TypeIndex == s_CoordinateTypeIndex)
                {
                    CoordinateBuffer.Add(default);
                    traitBasedObject.CoordinateIndex = (byte) (CoordinateBuffer.Length - 1);
                }
                else if (t.TypeIndex == s_BlockerTypeIndex)
                {
                    BlockerBuffer.Add(default);
                    traitBasedObject.BlockerIndex = (byte) (BlockerBuffer.Length - 1);
                }
                else if (t.TypeIndex == s_PlanningAgentTypeIndex)
                {
                    PlanningAgentBuffer.Add(default);
                    traitBasedObject.PlanningAgentIndex = (byte) (PlanningAgentBuffer.Length - 1);
                }
            }

            TraitBasedObjectIds.Add(objectId);
            TraitBasedObjects.Add(traitBasedObject);
        }

        public void AddObject(NativeArray<ComponentType> types, out TraitBasedObject traitBasedObject, out TraitBasedObjectId objectId, FixedString64 name = default)
        {
            objectId = new TraitBasedObjectId() { Id = ObjectId.GetNext() };
            AddObject(types, out traitBasedObject, objectId, name);
        }

        public void ConvertAndSetPlannerTrait(Entity sourceEntity, EntityManager sourceEntityManager,
            NativeArray<ComponentType> sourceTraitTypes, IDictionary<Entity, TraitBasedObjectId> entityToObjectId,
            ref TraitBasedObject traitBasedObject)
        {
            unsafe
            {
                foreach (var type in sourceTraitTypes)
                {
                    if (type == typeof(Generated.Semantic.Traits.GameData))
                    {
                        var traitData = sourceEntityManager.GetComponentData<Generated.Semantic.Traits.GameData>(sourceEntity);
                        var plannerTraitData = new Game();
                        UnsafeUtility.CopyStructureToPtr(ref traitData, UnsafeUtility.AddressOf(ref plannerTraitData));
                        SetTraitOnObject(plannerTraitData, ref traitBasedObject);
                    }
                    if (type == typeof(Generated.Semantic.Traits.CellData))
                    {
                        var traitData = sourceEntityManager.GetComponentData<Generated.Semantic.Traits.CellData>(sourceEntity);
                        var plannerTraitData = new Cell();
                        plannerTraitData.Type = traitData.Type;
                        if (entityToObjectId.TryGetValue(traitData.Left, out var Left))
                            plannerTraitData.Left = Left;
                        if (entityToObjectId.TryGetValue(traitData.Right, out var Right))
                            plannerTraitData.Right = Right;
                        if (entityToObjectId.TryGetValue(traitData.Top, out var Top))
                            plannerTraitData.Top = Top;
                        if (entityToObjectId.TryGetValue(traitData.Bottom, out var Bottom))
                            plannerTraitData.Bottom = Bottom;
                        SetTraitOnObject(plannerTraitData, ref traitBasedObject);
                    }
                    if (type == typeof(Generated.Semantic.Traits.CoordinateData))
                    {
                        var traitData = sourceEntityManager.GetComponentData<Generated.Semantic.Traits.CoordinateData>(sourceEntity);
                        var plannerTraitData = new Coordinate();
                        UnsafeUtility.CopyStructureToPtr(ref traitData, UnsafeUtility.AddressOf(ref plannerTraitData));
                        SetTraitOnObject(plannerTraitData, ref traitBasedObject);
                    }
                    if (type == typeof(Generated.Semantic.Traits.BlockerData))
                    {
                        var traitData = sourceEntityManager.GetComponentData<Generated.Semantic.Traits.BlockerData>(sourceEntity);
                        var plannerTraitData = new Blocker();
                        UnsafeUtility.CopyStructureToPtr(ref traitData, UnsafeUtility.AddressOf(ref plannerTraitData));
                        SetTraitOnObject(plannerTraitData, ref traitBasedObject);
                    }
                }
            }
        }

        public void SetTraitOnObject(ITrait trait, ref TraitBasedObject traitBasedObject)
        {
            if (trait is Game GameTrait)
                SetTraitOnObject(GameTrait, ref traitBasedObject);
            else if (trait is Cell CellTrait)
                SetTraitOnObject(CellTrait, ref traitBasedObject);
            else if (trait is Coordinate CoordinateTrait)
                SetTraitOnObject(CoordinateTrait, ref traitBasedObject);
            else if (trait is Blocker BlockerTrait)
                SetTraitOnObject(BlockerTrait, ref traitBasedObject);
            else if (trait is PlanningAgent PlanningAgentTrait)
                SetTraitOnObject(PlanningAgentTrait, ref traitBasedObject);
            else 
                throw new ArgumentException($"Trait {trait} of type {trait.GetType()} is not supported in this state representation.");
        }

        public void SetTraitOnObjectAtIndex(ITrait trait, int traitBasedObjectIndex)
        {
            if (trait is Game GameTrait)
                SetTraitOnObjectAtIndex(GameTrait, traitBasedObjectIndex);
            else if (trait is Cell CellTrait)
                SetTraitOnObjectAtIndex(CellTrait, traitBasedObjectIndex);
            else if (trait is Coordinate CoordinateTrait)
                SetTraitOnObjectAtIndex(CoordinateTrait, traitBasedObjectIndex);
            else if (trait is Blocker BlockerTrait)
                SetTraitOnObjectAtIndex(BlockerTrait, traitBasedObjectIndex);
            else if (trait is PlanningAgent PlanningAgentTrait)
                SetTraitOnObjectAtIndex(PlanningAgentTrait, traitBasedObjectIndex);
            else 
                throw new ArgumentException($"Trait {trait} of type {trait.GetType()} is not supported in this state representation.");
        }


        public TTrait GetTraitOnObject<TTrait>(TraitBasedObject traitBasedObject) where TTrait : struct, ITrait
        {
            var traitBasedObjectTraitIndex = TraitArrayIndex<TTrait>.Index;
            if (traitBasedObjectTraitIndex == -1)
                throw new ArgumentException($"Trait {typeof(TTrait)} not supported in this plan");

            var traitBufferIndex = traitBasedObject[traitBasedObjectTraitIndex];
            if (traitBufferIndex == TraitBasedObject.Unset)
                throw new ArgumentException($"Trait of type {typeof(TTrait)} does not exist on object {traitBasedObject}.");

            return GetBuffer<TTrait>()[traitBufferIndex];
        }

        public bool HasTraitOnObject<TTrait>(TraitBasedObject traitBasedObject) where TTrait : struct, ITrait
        {
            var traitBasedObjectTraitIndex = TraitArrayIndex<TTrait>.Index;
            if (traitBasedObjectTraitIndex == -1)
                throw new ArgumentException($"Trait {typeof(TTrait)} not supported in this plan");

            var traitBufferIndex = traitBasedObject[traitBasedObjectTraitIndex];
            return traitBufferIndex != TraitBasedObject.Unset;
        }

        public void SetTraitOnObject<TTrait>(TTrait trait, ref TraitBasedObject traitBasedObject) where TTrait : struct, ITrait
        {
            var objectIndex = GetTraitBasedObjectIndex(traitBasedObject);
            if (objectIndex == -1)
                throw new ArgumentException($"Object {traitBasedObject} does not exist within the state data {this}.");

            var traitIndex = TraitArrayIndex<TTrait>.Index;
            var traitBuffer = GetBuffer<TTrait>();

            var bufferIndex = traitBasedObject[traitIndex];
            if (bufferIndex == TraitBasedObject.Unset)
            {
                traitBuffer.Add(trait);
                traitBasedObject[traitIndex] = (byte) (traitBuffer.Length - 1);

                TraitBasedObjects[objectIndex] = traitBasedObject;
            }
            else
            {
                traitBuffer[bufferIndex] = trait;
            }
        }

        public bool RemoveTraitOnObject<TTrait>(ref TraitBasedObject traitBasedObject) where TTrait : struct, ITrait
        {
            var objectTraitIndex = TraitArrayIndex<TTrait>.Index;
            var traitBuffer = GetBuffer<TTrait>();

            var traitBufferIndex = traitBasedObject[objectTraitIndex];
            if (traitBufferIndex == TraitBasedObject.Unset)
                return false;

            // last index
            var lastBufferIndex = traitBuffer.Length - 1;

            // Swap back and remove
            var lastTrait = traitBuffer[lastBufferIndex];
            traitBuffer[lastBufferIndex] = traitBuffer[traitBufferIndex];
            traitBuffer[traitBufferIndex] = lastTrait;
            traitBuffer.RemoveAt(lastBufferIndex);

            // Update index for object with last trait in buffer
            var numObjects = TraitBasedObjects.Length;
            for (int i = 0; i < numObjects; i++)
            {
                var otherTraitBasedObject = TraitBasedObjects[i];
                if (otherTraitBasedObject[objectTraitIndex] == lastBufferIndex)
                {
                    otherTraitBasedObject[objectTraitIndex] = traitBufferIndex;
                    TraitBasedObjects[i] = otherTraitBasedObject;
                    break;
                }
            }

            // Update traitBasedObject in buffer (ref is to a copy)
            for (int i = 0; i < numObjects; i++)
            {
                if (traitBasedObject.Equals(TraitBasedObjects[i]))
                {
                    traitBasedObject[objectTraitIndex] = TraitBasedObject.Unset;
                    TraitBasedObjects[i] = traitBasedObject;
                    return true;
                }
            }

            throw new ArgumentException($"TraitBasedObject {traitBasedObject} does not exist in the state container {this}.");
        }

        public bool RemoveObject(TraitBasedObject traitBasedObject)
        {
            var objectIndex = GetTraitBasedObjectIndex(traitBasedObject);
            if (objectIndex == -1)
                return false;


            RemoveTraitOnObject<Game>(ref traitBasedObject);
            RemoveTraitOnObject<Cell>(ref traitBasedObject);
            RemoveTraitOnObject<Coordinate>(ref traitBasedObject);
            RemoveTraitOnObject<Blocker>(ref traitBasedObject);
            RemoveTraitOnObject<PlanningAgent>(ref traitBasedObject);

            TraitBasedObjects.RemoveAt(objectIndex);
            TraitBasedObjectIds.RemoveAt(objectIndex);

            return true;
        }


        public TTrait GetTraitOnObjectAtIndex<TTrait>(int traitBasedObjectIndex) where TTrait : struct, ITrait
        {
            var traitBasedObjectTraitIndex = TraitArrayIndex<TTrait>.Index;
            if (traitBasedObjectTraitIndex == -1)
                throw new ArgumentException($"Trait {typeof(TTrait)} not supported in this state representation");

            var traitBasedObject = TraitBasedObjects[traitBasedObjectIndex];
            var traitBufferIndex = traitBasedObject[traitBasedObjectTraitIndex];
            if (traitBufferIndex == TraitBasedObject.Unset)
                throw new Exception($"Trait index for {typeof(TTrait)} is not set for object {traitBasedObject}");

            return GetBuffer<TTrait>()[traitBufferIndex];
        }

        public void SetTraitOnObjectAtIndex<T>(T trait, int traitBasedObjectIndex) where T : struct, ITrait
        {
            var traitBasedObjectTraitIndex = TraitArrayIndex<T>.Index;
            if (traitBasedObjectTraitIndex == -1)
                throw new ArgumentException($"Trait {typeof(T)} not supported in this state representation");

            var traitBasedObject = TraitBasedObjects[traitBasedObjectIndex];
            var traitBufferIndex = traitBasedObject[traitBasedObjectTraitIndex];
            var traitBuffer = GetBuffer<T>();
            if (traitBufferIndex == TraitBasedObject.Unset)
            {
                traitBuffer.Add(trait);
                traitBufferIndex = (byte)(traitBuffer.Length - 1);
                traitBasedObject[traitBasedObjectTraitIndex] = traitBufferIndex;
                TraitBasedObjects[traitBasedObjectIndex] = traitBasedObject;
            }
            else
            {
                traitBuffer[traitBufferIndex] = trait;
            }
        }

        public bool RemoveTraitOnObjectAtIndex<TTrait>(int traitBasedObjectIndex) where TTrait : struct, ITrait
        {
            var objectTraitIndex = TraitArrayIndex<TTrait>.Index;
            var traitBuffer = GetBuffer<TTrait>();

            var traitBasedObject = TraitBasedObjects[traitBasedObjectIndex];
            var traitBufferIndex = traitBasedObject[objectTraitIndex];
            if (traitBufferIndex == TraitBasedObject.Unset)
                return false;

            // last index
            var lastBufferIndex = traitBuffer.Length - 1;

            // Swap back and remove
            var lastTrait = traitBuffer[lastBufferIndex];
            traitBuffer[lastBufferIndex] = traitBuffer[traitBufferIndex];
            traitBuffer[traitBufferIndex] = lastTrait;
            traitBuffer.RemoveAt(lastBufferIndex);

            // Update index for object with last trait in buffer
            var numObjects = TraitBasedObjects.Length;
            for (int i = 0; i < numObjects; i++)
            {
                var otherTraitBasedObject = TraitBasedObjects[i];
                if (otherTraitBasedObject[objectTraitIndex] == lastBufferIndex)
                {
                    otherTraitBasedObject[objectTraitIndex] = traitBufferIndex;
                    TraitBasedObjects[i] = otherTraitBasedObject;
                    break;
                }
            }

            traitBasedObject[objectTraitIndex] = TraitBasedObject.Unset;
            TraitBasedObjects[traitBasedObjectIndex] = traitBasedObject;

            return true;
        }

        public bool RemoveTraitBasedObjectAtIndex(int traitBasedObjectIndex)
        {
            RemoveTraitOnObjectAtIndex<Game>(traitBasedObjectIndex);
            RemoveTraitOnObjectAtIndex<Cell>(traitBasedObjectIndex);
            RemoveTraitOnObjectAtIndex<Coordinate>(traitBasedObjectIndex);
            RemoveTraitOnObjectAtIndex<Blocker>(traitBasedObjectIndex);
            RemoveTraitOnObjectAtIndex<PlanningAgent>(traitBasedObjectIndex);

            TraitBasedObjects.RemoveAt(traitBasedObjectIndex);
            TraitBasedObjectIds.RemoveAt(traitBasedObjectIndex);

            return true;
        }


        public NativeArray<int> GetTraitBasedObjectIndices(NativeList<int> traitBasedObjectIndices, NativeArray<ComponentType> traitFilter)
        {
            var numObjects = TraitBasedObjects.Length;
            for (var i = 0; i < numObjects; i++)
            {
                var traitBasedObject = TraitBasedObjects[i];
                if (traitBasedObject.MatchesTraitFilter(traitFilter))
                    traitBasedObjectIndices.Add(i);
            }

            return traitBasedObjectIndices.AsArray();
        }

        public NativeArray<int> GetTraitBasedObjectIndices(NativeList<int> traitBasedObjectIndices, params ComponentType[] traitFilter)
        {
            var numObjects = TraitBasedObjects.Length;
            for (var i = 0; i < numObjects; i++)
            {
                var traitBasedObject = TraitBasedObjects[i];
                if (traitBasedObject.MatchesTraitFilter(traitFilter))
                    traitBasedObjectIndices.Add(i);
            }

            return traitBasedObjectIndices.AsArray();
        }

        public int GetTraitBasedObjectIndex(TraitBasedObject traitBasedObject)
        {
            for (int objectIndex = TraitBasedObjects.Length - 1; objectIndex >= 0; objectIndex--)
            {
                bool match = true;
                var other = TraitBasedObjects[objectIndex];
                for (int i = 0; i < traitBasedObject.Length && match; i++)
                {
                    match &= traitBasedObject[i] == other[i];
                }

                if (match)
                    return objectIndex;
            }

            return -1;
        }

        public int GetTraitBasedObjectIndex(TraitBasedObjectId traitBasedObjectId)
        {
            var objectIndex = -1;
            for (int i = TraitBasedObjectIds.Length - 1; i >= 0; i--)
            {
                if (TraitBasedObjectIds[i].Equals(traitBasedObjectId))
                {
                    objectIndex = i;
                    break;
                }
            }

            return objectIndex;
        }

        public TraitBasedObjectId GetTraitBasedObjectId(TraitBasedObject traitBasedObject)
        {
            var index = GetTraitBasedObjectIndex(traitBasedObject);
            return TraitBasedObjectIds[index];
        }

        public TraitBasedObjectId GetTraitBasedObjectId(int traitBasedObjectIndex)
        {
            return TraitBasedObjectIds[traitBasedObjectIndex];
        }

        public TraitBasedObject GetTraitBasedObject(TraitBasedObjectId traitBasedObject)
        {
            var index = GetTraitBasedObjectIndex(traitBasedObject);
            return TraitBasedObjects[index];
        }


        DynamicBuffer<T> GetBuffer<T>() where T : struct, ITrait
        {
            var index = TraitArrayIndex<T>.Index;
            switch (index)
            {
                case 0:
                    return GameBuffer.Reinterpret<T>();
                case 1:
                    return CellBuffer.Reinterpret<T>();
                case 2:
                    return CoordinateBuffer.Reinterpret<T>();
                case 3:
                    return BlockerBuffer.Reinterpret<T>();
                case 4:
                    return PlanningAgentBuffer.Reinterpret<T>();
            }

            return default;
        }

        public bool Equals(IStateData other) => other is StateData otherData && Equals(otherData);

        public bool Equals(StateData rhsState)
        {
            if (StateEntity == rhsState.StateEntity)
                return true;

            // Easy check is to make sure each state has the same number of trait-based objects
            if (TraitBasedObjects.Length != rhsState.TraitBasedObjects.Length
                || GameBuffer.Length != rhsState.GameBuffer.Length
                || CellBuffer.Length != rhsState.CellBuffer.Length
                || CoordinateBuffer.Length != rhsState.CoordinateBuffer.Length
                || BlockerBuffer.Length != rhsState.BlockerBuffer.Length
                || PlanningAgentBuffer.Length != rhsState.PlanningAgentBuffer.Length)
                return false;

            var objectMap = new ObjectCorrespondence(TraitBasedObjectIds.Length, Allocator.Temp);
            bool statesEqual = TryGetObjectMapping(rhsState, objectMap);
            objectMap.Dispose();

            return statesEqual;
        }

        bool ITraitBasedStateData<TraitBasedObject, StateData>.TryGetObjectMapping(StateData rhsState, ObjectCorrespondence objectMap)
        {
            // Easy check is to make sure each state has the same number of domain objects
            if (TraitBasedObjects.Length != rhsState.TraitBasedObjects.Length
                || GameBuffer.Length != rhsState.GameBuffer.Length
                || CellBuffer.Length != rhsState.CellBuffer.Length
                || CoordinateBuffer.Length != rhsState.CoordinateBuffer.Length
                || BlockerBuffer.Length != rhsState.BlockerBuffer.Length
                || PlanningAgentBuffer.Length != rhsState.PlanningAgentBuffer.Length)
                return false;

            return TryGetObjectMapping(rhsState, objectMap);
        }

        internal bool TryGetObjectMapping(StateData rhsState, ObjectCorrespondence objectMap)
        {
            objectMap.Initialize(TraitBasedObjectIds, rhsState.TraitBasedObjectIds);

            bool statesEqual = true;
            var numObjects = TraitBasedObjects.Length;
            for (int lhsIndex = 0; lhsIndex < numObjects; lhsIndex++)
            {
                var lhsId = TraitBasedObjectIds[lhsIndex].Id;
                if (objectMap.TryGetValue(lhsId, out _)) // already matched
                    continue;

                // todo lhsIndex to start? would require swapping rhs on assignments, though
                bool matchFound = true;

                for (var rhsIndex = 0; rhsIndex < numObjects; rhsIndex++)
                {
                    var rhsId = rhsState.TraitBasedObjectIds[rhsIndex].Id;
                    if (objectMap.ContainsRHS(rhsId)) // skip if already assigned todo optimize this
                        continue;

                    objectMap.BeginNewTraversal();
                    objectMap.Add(lhsId, rhsId);

                    // Traversal comparing all reachable objects
                    matchFound = true;
                    while (objectMap.Next(out var lhsIdToEvaluate, out var rhsIdToEvaluate))
                    {
                        // match objects, queueing as needed
                        var lhsTraitBasedObject = TraitBasedObjects[objectMap.GetLHSIndex(lhsIdToEvaluate)];
                        var rhsTraitBasedObject = rhsState.TraitBasedObjects[objectMap.GetRHSIndex(rhsIdToEvaluate)];

                        if (!ObjectsMatchAttributes(lhsTraitBasedObject, rhsTraitBasedObject, rhsState) ||
                            !CheckRelationsAndQueueObjects(lhsTraitBasedObject, rhsTraitBasedObject, rhsState, objectMap))
                        {
                            objectMap.RevertTraversalChanges();

                            matchFound = false;
                            break;
                        }
                    }

                    if (matchFound)
                        break;
                }

                if (!matchFound)
                {
                    statesEqual = false;
                    break;
                }
            }

            return statesEqual;
        }

        bool ObjectsMatchAttributes(TraitBasedObject traitBasedObjectLHS, TraitBasedObject traitBasedObjectRHS, StateData rhsState)
        {
            if (!traitBasedObjectLHS.HasSameTraits(traitBasedObjectRHS))
                return false;

            if (traitBasedObjectLHS.GameIndex != TraitBasedObject.Unset
                && !GameTraitAttributesEqual(GameBuffer[traitBasedObjectLHS.GameIndex], rhsState.GameBuffer[traitBasedObjectRHS.GameIndex]))
                return false;


            if (traitBasedObjectLHS.CellIndex != TraitBasedObject.Unset
                && !CellTraitAttributesEqual(CellBuffer[traitBasedObjectLHS.CellIndex], rhsState.CellBuffer[traitBasedObjectRHS.CellIndex]))
                return false;


            if (traitBasedObjectLHS.CoordinateIndex != TraitBasedObject.Unset
                && !CoordinateTraitAttributesEqual(CoordinateBuffer[traitBasedObjectLHS.CoordinateIndex], rhsState.CoordinateBuffer[traitBasedObjectRHS.CoordinateIndex]))
                return false;


            if (traitBasedObjectLHS.BlockerIndex != TraitBasedObject.Unset
                && !BlockerTraitAttributesEqual(BlockerBuffer[traitBasedObjectLHS.BlockerIndex], rhsState.BlockerBuffer[traitBasedObjectRHS.BlockerIndex]))
                return false;



            return true;
        }
        
        bool GameTraitAttributesEqual(Game one, Game two)
        {
            return
                    one.MoveCount == two.MoveCount && 
                    one.Score == two.Score && 
                    one.GoalType == two.GoalType && 
                    one.GoalCount == two.GoalCount;
        }
        
        bool CellTraitAttributesEqual(Cell one, Cell two)
        {
            return
                    one.Type == two.Type;
        }
        
        bool CoordinateTraitAttributesEqual(Coordinate one, Coordinate two)
        {
            return
                    one.X == two.X && 
                    one.Y == two.Y;
        }
        
        bool BlockerTraitAttributesEqual(Blocker one, Blocker two)
        {
            return
                    one.Life == two.Life;
        }
        
        bool CheckRelationsAndQueueObjects(TraitBasedObject traitBasedObjectLHS, TraitBasedObject traitBasedObjectRHS, StateData rhsState, ObjectCorrespondence objectMap)
        {
            // edge walking - for relation properties
            ObjectId lhsRelationId;
            ObjectId rhsRelationId;
            ObjectId rhsAssignedId;
            if (traitBasedObjectLHS.CellIndex != TraitBasedObject.Unset)
            {
                // The Ids to match for Cell.Left
                lhsRelationId = CellBuffer[traitBasedObjectLHS.CellIndex].Left.Id;
                rhsRelationId = rhsState.CellBuffer[traitBasedObjectRHS.CellIndex].Left.Id;

                if (lhsRelationId.Equals(ObjectId.None) ^ rhsRelationId.Equals(ObjectId.None))
                    return false;

                if (objectMap.TryGetValue(lhsRelationId, out rhsAssignedId))
                {
                    if (!rhsRelationId.Equals(rhsAssignedId))
                        return false;
                }
                else
                {
                    objectMap.Add(lhsRelationId, rhsRelationId);
                }

                // The Ids to match for Cell.Right
                lhsRelationId = CellBuffer[traitBasedObjectLHS.CellIndex].Right.Id;
                rhsRelationId = rhsState.CellBuffer[traitBasedObjectRHS.CellIndex].Right.Id;

                if (lhsRelationId.Equals(ObjectId.None) ^ rhsRelationId.Equals(ObjectId.None))
                    return false;

                if (objectMap.TryGetValue(lhsRelationId, out rhsAssignedId))
                {
                    if (!rhsRelationId.Equals(rhsAssignedId))
                        return false;
                }
                else
                {
                    objectMap.Add(lhsRelationId, rhsRelationId);
                }

                // The Ids to match for Cell.Top
                lhsRelationId = CellBuffer[traitBasedObjectLHS.CellIndex].Top.Id;
                rhsRelationId = rhsState.CellBuffer[traitBasedObjectRHS.CellIndex].Top.Id;

                if (lhsRelationId.Equals(ObjectId.None) ^ rhsRelationId.Equals(ObjectId.None))
                    return false;

                if (objectMap.TryGetValue(lhsRelationId, out rhsAssignedId))
                {
                    if (!rhsRelationId.Equals(rhsAssignedId))
                        return false;
                }
                else
                {
                    objectMap.Add(lhsRelationId, rhsRelationId);
                }

                // The Ids to match for Cell.Bottom
                lhsRelationId = CellBuffer[traitBasedObjectLHS.CellIndex].Bottom.Id;
                rhsRelationId = rhsState.CellBuffer[traitBasedObjectRHS.CellIndex].Bottom.Id;

                if (lhsRelationId.Equals(ObjectId.None) ^ rhsRelationId.Equals(ObjectId.None))
                    return false;

                if (objectMap.TryGetValue(lhsRelationId, out rhsAssignedId))
                {
                    if (!rhsRelationId.Equals(rhsAssignedId))
                        return false;
                }
                else
                {
                    objectMap.Add(lhsRelationId, rhsRelationId);
                }
            }


            return true;
        }

        public override int GetHashCode()
        {
            // h = 3860031 + (h+y)*2779 + (h*y*2)   // from How to Hash a Set by Richard O’Keefe
            var stateHashValue = 3860031 + (397 + TraitBasedObjectIds.Length) * 2779 + (397 * TraitBasedObjectIds.Length * 2);

            int bufferLength;

            bufferLength = GameBuffer.Length;
            for (int i = 0; i < bufferLength; i++)
            {
                var element = GameBuffer[i];
                var value = 397
                    ^ element.MoveCount.GetHashCode()
                    ^ element.Score.GetHashCode()
                    ^ (int) element.GoalType
                    ^ element.GoalCount.GetHashCode();
                stateHashValue = 3860031 + (stateHashValue + value) * 2779 + (stateHashValue * value * 2);
            }
            bufferLength = CellBuffer.Length;
            for (int i = 0; i < bufferLength; i++)
            {
                var element = CellBuffer[i];
                var value = 397
                    ^ (int) element.Type;
                stateHashValue = 3860031 + (stateHashValue + value) * 2779 + (stateHashValue * value * 2);
            }
            bufferLength = CoordinateBuffer.Length;
            for (int i = 0; i < bufferLength; i++)
            {
                var element = CoordinateBuffer[i];
                var value = 397
                    ^ element.X.GetHashCode()
                    ^ element.Y.GetHashCode();
                stateHashValue = 3860031 + (stateHashValue + value) * 2779 + (stateHashValue * value * 2);
            }
            bufferLength = BlockerBuffer.Length;
            for (int i = 0; i < bufferLength; i++)
            {
                var element = BlockerBuffer[i];
                var value = 397
                    ^ element.Life.GetHashCode();
                stateHashValue = 3860031 + (stateHashValue + value) * 2779 + (stateHashValue * value * 2);
            }
            bufferLength = PlanningAgentBuffer.Length;
            for (int i = 0; i < bufferLength; i++)
            {
                var value = 397;
                stateHashValue = 3860031 + (stateHashValue + value) * 2779 + (stateHashValue * value * 2);
            }

            return stateHashValue;
        }

        public override string ToString()
        {
            if (StateEntity.Equals(default))
                return string.Empty;

            var sb = new StringBuilder();
            var numObjects = TraitBasedObjects.Length;
            for (var traitBasedObjectIndex = 0; traitBasedObjectIndex < numObjects; traitBasedObjectIndex++)
            {
                var traitBasedObject = TraitBasedObjects[traitBasedObjectIndex];
                sb.AppendLine(TraitBasedObjectIds[traitBasedObjectIndex].ToString());

                var i = 0;

                var traitIndex = traitBasedObject[i++];
                if (traitIndex != TraitBasedObject.Unset)
                    sb.AppendLine(GameBuffer[traitIndex].ToString());

                traitIndex = traitBasedObject[i++];
                if (traitIndex != TraitBasedObject.Unset)
                    sb.AppendLine(CellBuffer[traitIndex].ToString());

                traitIndex = traitBasedObject[i++];
                if (traitIndex != TraitBasedObject.Unset)
                    sb.AppendLine(CoordinateBuffer[traitIndex].ToString());

                traitIndex = traitBasedObject[i++];
                if (traitIndex != TraitBasedObject.Unset)
                    sb.AppendLine(BlockerBuffer[traitIndex].ToString());

                traitIndex = traitBasedObject[i++];
                if (traitIndex != TraitBasedObject.Unset)
                    sb.AppendLine(PlanningAgentBuffer[traitIndex].ToString());

                sb.AppendLine();
            }

            return sb.ToString();
        }
    }

    public struct StateDataContext : ITraitBasedStateDataContext<TraitBasedObject, StateEntityKey, StateData>
    {
        public bool IsCreated;
        internal EntityCommandBuffer.ParallelWriter EntityCommandBuffer;
        internal EntityArchetype m_StateArchetype;
        internal int JobIndex;

        [ReadOnly,NativeDisableContainerSafetyRestriction] public BufferFromEntity<TraitBasedObject> TraitBasedObjects;
        [ReadOnly,NativeDisableContainerSafetyRestriction] public BufferFromEntity<TraitBasedObjectId> TraitBasedObjectIds;

        [ReadOnly,NativeDisableContainerSafetyRestriction] public BufferFromEntity<Game> GameData;
        [ReadOnly,NativeDisableContainerSafetyRestriction] public BufferFromEntity<Cell> CellData;
        [ReadOnly,NativeDisableContainerSafetyRestriction] public BufferFromEntity<Coordinate> CoordinateData;
        [ReadOnly,NativeDisableContainerSafetyRestriction] public BufferFromEntity<Blocker> BlockerData;
        [ReadOnly,NativeDisableContainerSafetyRestriction] public BufferFromEntity<PlanningAgent> PlanningAgentData;

        [NativeDisableContainerSafetyRestriction,ReadOnly] ObjectCorrespondence m_ObjectCorrespondence;

        public StateDataContext(JobComponentSystem system, EntityArchetype stateArchetype)
        {
            EntityCommandBuffer = default;
            TraitBasedObjects = system.GetBufferFromEntity<TraitBasedObject>(true);
            TraitBasedObjectIds = system.GetBufferFromEntity<TraitBasedObjectId>(true);

            GameData = system.GetBufferFromEntity<Game>(true);
            CellData = system.GetBufferFromEntity<Cell>(true);
            CoordinateData = system.GetBufferFromEntity<Coordinate>(true);
            BlockerData = system.GetBufferFromEntity<Blocker>(true);
            PlanningAgentData = system.GetBufferFromEntity<PlanningAgent>(true);

            m_StateArchetype = stateArchetype;
            JobIndex = 0;
            m_ObjectCorrespondence = default;
            IsCreated = true;
        }

        public StateData GetStateData(StateEntityKey stateKey)
        {
            var stateEntity = stateKey.Entity;

            return new StateData
            {
                StateEntity = stateEntity,
                TraitBasedObjects = TraitBasedObjects[stateEntity],
                TraitBasedObjectIds = TraitBasedObjectIds[stateEntity],

                GameBuffer = GameData[stateEntity],
                CellBuffer = CellData[stateEntity],
                CoordinateBuffer = CoordinateData[stateEntity],
                BlockerBuffer = BlockerData[stateEntity],
                PlanningAgentBuffer = PlanningAgentData[stateEntity],
            };
        }

        public StateData CopyStateData(StateData stateData)
        {
            return stateData.Copy(JobIndex, EntityCommandBuffer);
        }

        public StateEntityKey GetStateDataKey(StateData stateData)
        {
            return new StateEntityKey { Entity = stateData.StateEntity, HashCode = stateData.GetHashCode()};
        }

        public void DestroyState(StateEntityKey stateKey)
        {
            EntityCommandBuffer.DestroyEntity(JobIndex, stateKey.Entity);
        }

        public StateData CreateStateData()
        {
            return new StateData(JobIndex, EntityCommandBuffer, EntityCommandBuffer.CreateEntity(JobIndex, m_StateArchetype));
        }

        public bool Equals(StateData x, StateData y)
        {
            if (x.TraitBasedObjectIds.Length != y.TraitBasedObjectIds.Length)
                return false;

            if (!m_ObjectCorrespondence.IsCreated)
                m_ObjectCorrespondence = new ObjectCorrespondence(x.TraitBasedObjectIds.Length, Allocator.Temp);

            return x.TryGetObjectMapping(y, m_ObjectCorrespondence);
        }

        public int GetHashCode(StateData obj)
        {
            return obj.GetHashCode();
        }
    }

    [DisableAutoCreation, AlwaysUpdateSystem]
    public class StateManager : JobComponentSystem, ITraitBasedStateManager<TraitBasedObject, StateEntityKey, StateData, StateDataContext>
    {
        public new EntityManager EntityManager
        {
            get
            {
                if (!m_EntityTransactionActive)
                    BeginEntityExclusivity();

                return ExclusiveEntityTransaction.EntityManager;
            }
        }

        ExclusiveEntityTransaction m_ExclusiveEntityTransaction;
        public ExclusiveEntityTransaction ExclusiveEntityTransaction
        {
            get
            {
                if (!m_EntityTransactionActive)
                    BeginEntityExclusivity();

                return m_ExclusiveEntityTransaction;
            }
        }

        StateDataContext m_StateDataContext;
        public StateDataContext StateDataContext
        {
            get
            {
                if (m_StateDataContext.IsCreated)
                    return m_StateDataContext;

                m_StateDataContext = new StateDataContext(this, m_StateArchetype);
                return m_StateDataContext;
            }
        }

        public event Action Destroying;

        List<EntityCommandBuffer> m_EntityCommandBuffers;
        EntityArchetype m_StateArchetype;
        bool m_EntityTransactionActive = false;

        protected override void OnCreate()
        {
            m_StateArchetype = base.EntityManager.CreateArchetype(typeof(State), typeof(TraitBasedObject), typeof(TraitBasedObjectId), typeof(HashCode),
                typeof(Game),
                typeof(Cell),
                typeof(Coordinate),
                typeof(Blocker),
                typeof(PlanningAgent));

            m_EntityCommandBuffers = new List<EntityCommandBuffer>();
        }

        protected override void OnDestroy()
        {
            Destroying?.Invoke();
            EndEntityExclusivity();
            ClearECBs();
            base.OnDestroy();
        }

        public EntityCommandBuffer GetEntityCommandBuffer()
        {
            var ecb = new EntityCommandBuffer(Allocator.Persistent);
            m_EntityCommandBuffers.Add(ecb);
            return ecb;
        }

        public StateData CreateStateData()
        {
            var stateEntity = ExclusiveEntityTransaction.CreateEntity(m_StateArchetype);
            return new StateData(ExclusiveEntityTransaction, stateEntity);;
        }

        public StateData GetStateData(StateEntityKey stateKey, bool readWrite = false)
        {
            return !Enabled || !ExclusiveEntityTransaction.Exists(stateKey.Entity) ?
                default : new StateData(ExclusiveEntityTransaction, stateKey.Entity);
        }

        public void DestroyState(StateEntityKey stateKey)
        {
            var stateEntity = stateKey.Entity;
            if (ExclusiveEntityTransaction.Exists(stateEntity))
                ExclusiveEntityTransaction.DestroyEntity(stateEntity);
        }

        public StateEntityKey GetStateDataKey(StateData stateData)
        {
            return new StateEntityKey { Entity = stateData.StateEntity, HashCode = stateData.GetHashCode()};
        }

        public StateData CopyStateData(StateData stateData)
        {
            var copyStateEntity = ExclusiveEntityTransaction.Instantiate(stateData.StateEntity);
            return new StateData(ExclusiveEntityTransaction, copyStateEntity);
        }

        public StateEntityKey CopyState(StateEntityKey stateKey)
        {
            var copyStateEntity = ExclusiveEntityTransaction.Instantiate(stateKey.Entity);
            var stateData = new StateData(ExclusiveEntityTransaction, copyStateEntity);
            return new StateEntityKey { Entity = copyStateEntity, HashCode = stateData.GetHashCode()};
        }

        protected override JobHandle OnUpdate(JobHandle inputDeps)
        {
            var jobDependencyHandle = ExclusiveEntityTransaction.EntityManager.ExclusiveEntityTransactionDependency;
            if (jobDependencyHandle.IsCompleted)
            {
                jobDependencyHandle.Complete();
                ClearECBs();
            }

            return inputDeps;
        }

        void ClearECBs()
        {
            foreach (var ecb in m_EntityCommandBuffers)
            {
                ecb.Dispose();
            }
            m_EntityCommandBuffers.Clear();
        }

        public bool Equals(StateData x, StateData y)
        {
            return x.Equals(y);
        }

        public int GetHashCode(StateData obj)
        {
            return obj.GetHashCode();
        }

        void BeginEntityExclusivity()
        {
            m_StateDataContext = new StateDataContext(this, m_StateArchetype);
            m_ExclusiveEntityTransaction = base.EntityManager.BeginExclusiveEntityTransaction();
            m_EntityTransactionActive = true;
        }

        void EndEntityExclusivity()
        {
            base.EntityManager.EndExclusiveEntityTransaction();
            m_EntityTransactionActive = false;
            m_StateDataContext = default;
        }
    }

    struct DestroyStatesJobScheduler : IDestroyStatesScheduler<StateEntityKey, StateData, StateDataContext, StateManager>
    {
        public StateManager StateManager { private get; set; }
        public NativeQueue<StateEntityKey> StatesToDestroy { private get; set; }

        public JobHandle Schedule(JobHandle inputDeps)
        {
            var stateDataContext = StateManager.StateDataContext;
            var ecb = StateManager.GetEntityCommandBuffer();
            stateDataContext.EntityCommandBuffer = ecb.AsParallelWriter();
            var destroyStatesJobHandle = new DestroyStatesJob<StateEntityKey, StateData, StateDataContext>()
            {
                StateDataContext = stateDataContext,
                StatesToDestroy = StatesToDestroy
            }.Schedule(inputDeps);

            var playbackECBJobHandle = new PlaybackSingleECBJob()
            {
                ExclusiveEntityTransaction = StateManager.ExclusiveEntityTransaction,
                EntityCommandBuffer = ecb
            }.Schedule(destroyStatesJobHandle);

            var entityManager = StateManager.ExclusiveEntityTransaction.EntityManager;
            entityManager.ExclusiveEntityTransactionDependency = playbackECBJobHandle;
            return playbackECBJobHandle;
        }
    }
}

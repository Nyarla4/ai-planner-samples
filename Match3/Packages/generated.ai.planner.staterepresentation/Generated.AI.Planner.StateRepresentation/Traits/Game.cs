using System;
using System.Collections.Generic;
using Unity.Collections;
using Unity.Entities;
using Unity.AI.Planner.Traits;
using Generated.Semantic.Traits.Enums;

namespace Generated.AI.Planner.StateRepresentation
{
    [Serializable]
    public struct Game : ITrait, IBufferElementData, IEquatable<Game>
    {
        public const string FieldMoveCount = "MoveCount";
        public const string FieldScore = "Score";
        public const string FieldGoalType = "GoalType";
        public const string FieldGoalCount = "GoalCount";
        public System.Int32 MoveCount;
        public System.Int32 Score;
        public Generated.Semantic.Traits.Enums.CellType GoalType;
        public System.Int32 GoalCount;

        public void SetField(string fieldName, object value)
        {
            switch (fieldName)
            {
                case nameof(MoveCount):
                    MoveCount = (System.Int32)value;
                    break;
                case nameof(Score):
                    Score = (System.Int32)value;
                    break;
                case nameof(GoalType):
                    GoalType = (Generated.Semantic.Traits.Enums.CellType)Enum.ToObject(typeof(Generated.Semantic.Traits.Enums.CellType), value);
                    break;
                case nameof(GoalCount):
                    GoalCount = (System.Int32)value;
                    break;
                default:
                    throw new ArgumentException($"Field \"{fieldName}\" does not exist on trait Game.");
            }
        }

        public object GetField(string fieldName)
        {
            switch (fieldName)
            {
                case nameof(MoveCount):
                    return MoveCount;
                case nameof(Score):
                    return Score;
                case nameof(GoalType):
                    return GoalType;
                case nameof(GoalCount):
                    return GoalCount;
                default:
                    throw new ArgumentException($"Field \"{fieldName}\" does not exist on trait Game.");
            }
        }

        public bool Equals(Game other)
        {
            return MoveCount == other.MoveCount && Score == other.Score && GoalType == other.GoalType && GoalCount == other.GoalCount;
        }

        public override string ToString()
        {
            return $"Game\n  MoveCount: {MoveCount}\n  Score: {Score}\n  GoalType: {GoalType}\n  GoalCount: {GoalCount}";
        }
    }
}

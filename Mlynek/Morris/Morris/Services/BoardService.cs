using System.Collections.Generic;
using System.Linq;
using Morris.Models;

namespace Morris.Services
{
    public static class BoardService
    {
        public static void InitializeFields(this Board board)
        {
            board.OuterFields = new List<Field>()
            {
                new Field() { State = FieldState.Empty, GridCol = 0, GridRow = 0},
                new Field() { State = FieldState.Empty, GridCol = 3, GridRow = 0},
                new Field() { State = FieldState.Empty, GridCol = 6, GridRow = 0},
                new Field() { State = FieldState.Empty, GridCol = 6, GridRow = 3},
                new Field() { State = FieldState.Empty, GridCol = 6, GridRow = 6},
                new Field() { State = FieldState.Empty, GridCol = 3, GridRow = 6},
                new Field() { State = FieldState.Empty, GridCol = 0, GridRow = 6},
                new Field() { State = FieldState.Empty, GridCol = 0, GridRow = 3},
            };

            board.MiddleFields = new List<Field>()
            {
                new Field() { State = FieldState.Empty, GridCol = 1, GridRow = 1},
                new Field() { State = FieldState.Empty, GridCol = 3, GridRow = 1},
                new Field() { State = FieldState.Empty, GridCol = 5, GridRow = 1},
                new Field() { State = FieldState.Empty, GridCol = 5, GridRow = 3},
                new Field() { State = FieldState.Empty, GridCol = 5, GridRow = 5},
                new Field() { State = FieldState.Empty, GridCol = 3, GridRow = 5},
                new Field() { State = FieldState.Empty, GridCol = 1, GridRow = 5},
                new Field() { State = FieldState.Empty, GridCol = 1, GridRow = 3},
            };

            board.InnerFields = new List<Field>()
            {
                new Field() { State = FieldState.Empty, GridCol = 2, GridRow = 2},
                new Field() { State = FieldState.Empty, GridCol = 3, GridRow = 2},
                new Field() { State = FieldState.Empty, GridCol = 4, GridRow = 2},
                new Field() { State = FieldState.Empty, GridCol = 4, GridRow = 3},
                new Field() { State = FieldState.Empty, GridCol = 4, GridRow = 4},
                new Field() { State = FieldState.Empty, GridCol = 3, GridRow = 4},
                new Field() { State = FieldState.Empty, GridCol = 2, GridRow = 4},
                new Field() { State = FieldState.Empty, GridCol = 2, GridRow = 3},
            };
            foreach (var field in board.OuterFields)
            {
                field.UpdateCords();
            }
            foreach (var field in board.MiddleFields)
            {
                field.UpdateCords();
            }

            foreach (var field in board.InnerFields)
            {
                field.UpdateCords();
            }

            if (board.LastP1Moves == null)
                board.LastP1Moves = new Move() { Start = string.Empty, Stop = string.Empty, Player = FieldState.P1 };
            if (board.LastP2Moves == null)
                board.LastP2Moves = new Move() { Start = string.Empty, Stop = string.Empty, Player = FieldState.P2 };
        }

        public static List<Field> GetFields(this Board board)
        {
            var fields = board.OuterFields.Concat(board.MiddleFields).Concat(board.InnerFields).ToList();
            return fields;
        }

        public static IEnumerable<Field> GetNeighbors(this Board board, string cords)
        {
            var neighbors = new List<Field>();
            var field = Get(board, cords);
            if (board.OuterFields.Contains(field))
            {
                var index = board.OuterFields.IndexOf(field);
                neighbors.Add(board.OuterFields[(index+1)%8]);
                if (index != 0)
                    neighbors.Add(board.OuterFields[(index - 1) % 8]);
                else
                    neighbors.Add(board.OuterFields.LastOrDefault());

                if (index%2 == 1)
                {
                    neighbors.Add(board.MiddleFields[index]);
                }
            }
            else if (board.MiddleFields.Contains(field))
            {
                var index = board.MiddleFields.IndexOf(field);
                neighbors.Add(board.MiddleFields[(index + 1) % 8]);
                if (index != 0)
                    neighbors.Add(board.MiddleFields[(index - 1) % 8]);
                else
                    neighbors.Add(board.MiddleFields.LastOrDefault());
                if (index % 2 == 1)
                {
                    neighbors.Add(board.OuterFields[index]);
                    neighbors.Add(board.InnerFields[index]);
                }
            }
            else if (board.InnerFields.Contains(field))
            {
                var index = board.InnerFields.IndexOf(field);
                neighbors.Add(board.InnerFields[(index + 1) % 8]);
                if (index != 0)
                    neighbors.Add(board.InnerFields[(index - 1) % 8]);
                else
                    neighbors.Add(board.InnerFields.LastOrDefault());
                if (index % 2 == 1)
                {
                    neighbors.Add(board.MiddleFields[index]);
                }
            }

            return neighbors;
        }

        public static IEnumerable<Field> GetAvailableMoves(this Board board, string cords)
        {
            var field = Get(board, cords);
            if (field.State.Equals(FieldState.Empty))
            {
                return null;
            }
            

            var lastMove = field.State.Equals(FieldState.P1) ? board.LastP1Moves : board.LastP2Moves;
            List<Field> list;
            if (GetFields(board).Count(f => f.State == field.State) == 3)
                list = GetFields(board).Where(f => f.State == FieldState.Empty).ToList();
            else
            {
                list = GetNeighbors(board, cords).Where(x => x.State == FieldState.Empty).ToList();
            }
            for (int i = 0; i < list.Count; i++)
                if (list[i].Cords.Equals(lastMove.Start) && cords.Equals(lastMove.Stop))
                {
                    list.RemoveAt(i);
                }
            return list;
        }

        public static void UpdateLastMove(this Board board, string start, string stop, FieldState state)
        {
            var move = new Move {Player = state, Start = start, Stop = stop};
            if (state.Equals(FieldState.P1))
            {
                board.LastP1Moves = move;
            }
            else if (state.Equals(FieldState.P2))
            {
                board.LastP2Moves = move;
            }
        }

        public static Field Get(this Board board, string cords)
        {
            var field = board.OuterFields.Concat(board.MiddleFields).Concat(board.InnerFields)
                .FirstOrDefault(f => f.Cords == cords);

            return field;
        }

        public static IEnumerable<Mill> GetMills(this Board board)
        {
            var list = new List<Mill>();

            for (int i = 0; i < 8; i = i + 2)
            {
                if (IsAMill(board.OuterFields[i], board.OuterFields[i + 1], board.OuterFields[(i + 2) % 8]))
                    list.Add(new Mill() { Field1 = board.OuterFields[i], Field2 = board.OuterFields[i + 1], Field3 = board.OuterFields[(i + 2) % 8] });
            }

            for (int i = 0; i < 8; i = i + 2)
            {
                if (IsAMill(board.MiddleFields[i], board.MiddleFields[i + 1], board.MiddleFields[(i + 2) % 8]))
                    list.Add(new Mill() { Field1 = board.MiddleFields[i], Field2 = board.MiddleFields[i + 1], Field3 = board.MiddleFields[(i + 2) % 8] });
            }

            for (int i = 0; i < 8; i = i + 2)
            {
                if (IsAMill(board.InnerFields[i], board.InnerFields[i + 1], board.InnerFields[(i + 2) % 8]))
                    list.Add(new Mill() { Field1 = board.InnerFields[i], Field2 = board.InnerFields[i + 1], Field3 = board.InnerFields[(i + 2) % 8] });
            }

            for (int i = 1; i < 8; i = i + 2)
            {
                if (IsAMill(board.OuterFields[i], board.MiddleFields[i], board.InnerFields[i]))
                    list.Add(new Mill() { Field1 = board.OuterFields[i], Field2 = board.MiddleFields[i], Field3 = board.InnerFields[i] });
            }

            return list;
        }

        public static int GetDoubles(this Board board, FieldState state)
        {
            int sum = 0;

            for (int i = 0; i < 8; i = i + 2)
            {
                if (IsADouble(board, board.OuterFields[i], board.OuterFields[i + 1], board.OuterFields[(i + 2) % 8], state))
                    sum++;
            }

            for (int i = 0; i< 8; i = i + 2)
            {
                if (IsADouble(board, board.MiddleFields[i], board.MiddleFields[i + 1], board.MiddleFields[(i + 2) % 8], state))
                    sum++;
            }

            for (int i = 0; i< 8; i = i + 2)
            {
                if (IsADouble(board, board.InnerFields[i], board.InnerFields[i + 1], board.InnerFields[(i + 2) % 8], state))
                    sum++;
            }

            for (int i = 1; i< 8; i = i + 2)
            {
                if (IsADouble(board, board.OuterFields[i], board.MiddleFields[i], board.InnerFields[i], state))
                    sum++;
            }

            return sum;
        }

        private static bool IsADouble(this Board board, Field field1, Field field2, Field field3, FieldState state)
        {
            var fields = new List<Field>() { field1, field2, field3 };
            return fields.Count(x => x.State == state) == 2;
        }

        public static int CountPlayerFields(this Board board, FieldState state)
        {
            return board.OuterFields.Concat(board.MiddleFields).Concat(board.InnerFields)
                .Count(x => x.State == state);
        }

        private static bool IsAMill(Field field1, Field field2, Field field3)
        {
            if (field1.State != FieldState.Empty)
            {
                if (field1.State == field2.State && field2.State == field3.State)
                {
                    return true;
                }
            }

            return false;
        }

        public static Board Copy(this Board board)
        {
            Board b = new Board
            {
                OuterFields = board.OuterFields.Select(x => x.Copy()).ToList(),
                InnerFields = board.InnerFields.Select(x => x.Copy()).ToList(),
                MiddleFields = board.MiddleFields.Select(x => x.Copy()).ToList(),
                LastP1Moves = new Move() { Start = board.LastP1Moves.Start, Stop = board.LastP1Moves.Stop, Player = board.LastP1Moves.Player},
                LastP2Moves = new Move() { Start = board.LastP2Moves.Start, Stop = board.LastP2Moves.Stop, Player = board.LastP2Moves.Player}
            };
            return b;
        }

        public static bool IsGameOver(this Board board, int placedStones, FieldState pState)
        {
            if (placedStones < 18)
            {
                return false;
            }

            var fields = board.GetFields().Where(x => x.State.Equals(pState));
            if (board.GetFields().Count() == 2)
            {
                return true;
            }

            bool isOver = true;
            foreach (var field in fields)
            {
                if (GetAvailableMoves(board, field.Cords).Any())
                {
                    isOver = false;
                }
            }

            return isOver;
        }
    }
}
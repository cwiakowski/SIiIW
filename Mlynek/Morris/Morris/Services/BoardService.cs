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
        }

        public static void UpdateField(this Board board, int state, int list, int index)
        {
            if (0 <= index && index < 8)
            {
                switch (list)
                {
                    case 0:
                        board.OuterFields[index].UpdateState(state);
                        break;
                    case 1:
                        board.MiddleFields[index].UpdateState(state);
                        break;
                    case 2:
                        board.InnerFields[index].UpdateState(state);
                        break;
                }
            }
        }

        public static IEnumerable<Field> GetNeighbors(this Board board, string cords)
        {
            var neighbors = new List<Field>();
            //biore se chlopa
            //biore sasiadow z listy
            //patrze czy jest na krzyzowce
            //biore z innych siema
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

        public static void UpdateField(this Board board, int state, string cords)
        {
            var field = Get(board, cords);
            field?.UpdateState(state);
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
    }
}
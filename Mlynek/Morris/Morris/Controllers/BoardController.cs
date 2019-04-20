using System.Collections.Generic;
using System.Linq;
using Morris.Models;
using Morris.Services;

namespace Morris.Controllers
{
    public class BoardController
    {
        public IEnumerable<Mill> Mills { get; set; }
        private Board _board;

        public BoardController()
        {
            _board = new Board();
            Mills = new List<Mill>();
        }

        public bool Move(string start, string stop)
        {
            var field1 = _board.Get(start);
            var field2 = _board.Get(stop);
            if (field1 == null || field2 == null)
            {
                return false;
            }

            if (_board.CountPlayerFields(field1.State) == 3)
            {
                if (field2.State == FieldState.Empty)
                {
                    var temp = field1.State;
                    field1.State = field2.State;
                    field2.State = temp;
                    Mills = _board.GetMills();
                    return true;
                }
            }

            if (AvailableMoves(start).Contains(field2))
            {
                var temp = field1.State;
                field1.State = field2.State;
                field2.State = temp;
                Mills = _board.GetMills();
                return true;
            }

            
            return false;
        }

        public IEnumerable<Field> GetNeighbors(string cords)
        {
            return _board.GetNeighbors(cords);
        }

        public IEnumerable<Field> AvailableMoves(string cords)
        {
            var list = _board.GetNeighbors(cords);
            var field = _board.Get(cords);
            return list.Where(x => x.State == FieldState.Empty);
        }


        public void NewGame()
        {
            _board.InitializeFields();
        }

        public bool IsMill()
        {
            Mills = _board.GetMills();
            if (Mills.Any())
            {
                return false;
            }
            return true;
        }

        public void ChangeValue(int state, int list, int ind)
        {
            _board.UpdateField(state, list, ind);
            Mills = _board.GetMills();
        }

        public void ChangeValue(int state, string cords)
        {
            _board.UpdateField(state, cords);
            Mills = _board.GetMills();
        }

        public string GameState()
        {
            return _board.ToString();
        }

        public void GenerateExample()
        {
            _board.OuterFields[0].State = FieldState.P1;
            _board.OuterFields[2].State = FieldState.P1;
            _board.OuterFields[3].State = FieldState.P1;
            _board.OuterFields[6].State = FieldState.P1;
            _board.InnerFields[7].State = FieldState.P1;
            _board.InnerFields[0].State = FieldState.P1;
            _board.InnerFields[1].State = FieldState.P1;
            _board.MiddleFields[5].State = FieldState.P1;
            _board.MiddleFields[2].State = FieldState.P1;

            _board.OuterFields[1].State = FieldState.P2;
            _board.OuterFields[4].State = FieldState.P2;
            _board.OuterFields[5].State = FieldState.P2;
            _board.MiddleFields[1].State = FieldState.P2;
            _board.MiddleFields[0].State = FieldState.P2;
            _board.MiddleFields[4].State = FieldState.P2;
            _board.MiddleFields[6].State = FieldState.P2;
            _board.InnerFields[2].State = FieldState.P2;
            _board.InnerFields[3].State = FieldState.P2;
        }
    }
}
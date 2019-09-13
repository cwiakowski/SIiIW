using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Morris.Models
{
    public class Board : IDisposable
    {
        public List<Field> OuterFields { get; set; }
        public List<Field> MiddleFields { get; set; }
        public List<Field> InnerFields { get; set; }
        public Move LastP1Moves { get; set; }
        public Move LastP2Moves { get; set; }


        public override string ToString()
        {
            var s = new StringBuilder();
            for (int row = 0; row < 7; row++)
            {
                for (int col = 0; col < 7; col++)
                {
                    var temp = "-";
                    if (OuterFields.FirstOrDefault(x => x.GridRow == row && x.GridCol == col) != null)
                    {
                        temp = OuterFields.FirstOrDefault(x => x.GridRow == row && x.GridCol == col).ToString();
                    }
                    else if (MiddleFields.FirstOrDefault(x => x.GridRow == row && x.GridCol == col) != null)
                    {
                        temp = MiddleFields.FirstOrDefault(x => x.GridRow == row && x.GridCol == col).ToString();
                    }
                    else if (InnerFields.FirstOrDefault(x => x.GridRow == row && x.GridCol == col) != null)
                    {
                        temp = InnerFields.FirstOrDefault(x => x.GridRow == row && x.GridCol == col).ToString();
                    }

                    if (temp == "-" && (col == 0 || col == 6))
                    {
                        temp = " |";
                    }

                    if (temp == "-" && (row == 2 || row == 4))
                    {
                        temp = "|";
                    }
                    
                    s.Append($"{temp} ");
                }
                s.Append("\n");
            }

            return s.ToString();
        }


        public void Dispose()
        {
            OuterFields = null;
            MiddleFields = null;
            InnerFields = null;
            LastP1Moves = null;
            LastP2Moves = null;
            GC.SuppressFinalize(this);
        }
    }
}
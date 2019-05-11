namespace Morris.Models
{
    public class Field
    {
        public FieldState State { get; set; }
        public int GridRow { get; set; }
        public int GridCol { get; set; }

        public string Cords { get; set; }

        public override bool Equals(object obj)
        {
            Field f = obj as Field;
            return (GridRow.Equals(f.GridRow) && GridCol.Equals(f.GridCol));
        }

        public override string ToString()
        {
            switch (State)
            {
                case FieldState.Empty:
                    return "0";
                case FieldState.P1:
                    return "1";
                case FieldState.P2:
                    return "2";
            }

            return string.Empty;
        }
    }
}
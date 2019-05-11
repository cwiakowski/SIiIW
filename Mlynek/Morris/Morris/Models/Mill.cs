using System.Text;

namespace Morris.Models
{
    public class Mill
    {
        public Field Field1 { get; set; }
        public Field Field2 { get; set; }
        public Field Field3 { get; set; }

        public override bool Equals(object obj)
        {
//            var m = obj as Mill;
//            return (Field1.Equals(m.Field1) && Field2.Equals(m.Field2) && Field3.Equals(m.Field3));
            return GetHashCode() == obj.GetHashCode();
        }

        public override int GetHashCode()
        {
            int s = 0;
            s += Field1.GridCol;
            s += Field2.GridCol*100;
            s += Field3.GridCol*10000;
            s += Field1.GridRow*10;
            s += Field2.GridRow*1000;
            s += Field3.GridRow*100000;
            return s;
        }

        public override string ToString()
        {
            var s = new StringBuilder();
            s.Append($"[({Field1.Cords}), ");
            s.Append($"({Field2.Cords}), ");
            s.Append($"({Field3.Cords})]");
            return s.ToString();
        }
    }
}
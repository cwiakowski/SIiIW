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
            var ob = obj as Mill;
            if (Field1.GridCol == ob.Field1.GridCol && Field1.GridRow == ob.Field1.GridRow
                                                    && Field2.GridCol == ob.Field2.GridCol &&
                                                    Field2.GridRow == ob.Field2.GridRow
                                                    && Field3.GridCol == ob.Field3.GridCol &&
                                                    Field3.GridRow == ob.Field3.GridRow)
                return true;
            return false;
        }

        public override string ToString()
        {
            var s = new StringBuilder();
            s.Append($"[({Field1.GridRow}, {Field1.GridCol}), ");
            s.Append($"[({Field2.GridRow}, {Field2.GridCol}), ");
            s.Append($"[({Field3.GridRow}, {Field3.GridCol})]");
            return s.ToString();
        }
    }
}
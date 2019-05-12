using System;
using System.Text;
using Morris.Models;

namespace Morris.Services
{
    public static class FieldService
    {
        public static void UpdateState(this Field field, int state)
        {
            if (state == 0)
            {
                field.State = FieldState.Empty;
            }
            else if (state == 1)
            {
                field.State = FieldState.P1;
            }
            else if (state == 2)
            {
                field.State = FieldState.P2;
            }
        }

        public static void UpdateCords(this Field field)
        {
            var s = new StringBuilder();
            s.Append(Convert.ToChar(field.GridCol + 97));
            s.Append(7 - field.GridRow);
            field.Cords = s.ToString();
        }

        public static Field Copy(this Field field)
        {
            Field f = new Field()
            {
                State = field.State,
                Cords = field.Cords,
                GridRow = field.GridRow,
                GridCol = field.GridCol
            };
            return f;
        }
    }
}
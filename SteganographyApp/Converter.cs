using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SteganographyApp
{
    class Converter
    {
        public ArrayList bits = new ArrayList();
        public int num;

        public Converter(int value)
        {
            num = value;
            IntToBits();
        }

        private void IntToBits()
        {
            int temp = num;
            for (int i = 0; i < 8; i++)
            {
                bits.Add(temp % 2);
                temp = (int)Math.Floor((double)temp / 2);
            }
            bits.Reverse();
        }

        override public string ToString()
        {
            string str = "";
            for (int i = 0; i < bits.Count; i++)
            {
                str += bits[i];
            }
            return str;
        }
    }
}

using System;
using System.Collections.Generic;
using System.Text;

namespace AdventCode_3a {
    public class Claim {
        public int Id { get; set; }

        public int Left { get; set; }

        public int Top { get; set; }

        public int HeightInInches { get; set; }

        public int WidthInInches { get; set; }

        public int Right
        {
            get
            {
                return Left + WidthInInches;
            }
        }

        public int Bottom
        {
            get
            {
                return Top + HeightInInches;
            }
        }

        public int X1 { get; set; }
        public int X2 { get; set; }

        public int Y1 { get; set; }

        public int Y2 { get; set; }

    }
}

﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Tetris
{
    public class Position
    {
        public int Row {  get; set; }
        public int Column { get; set; }

        public Position(int Row, int Column)
        {
            this.Row = Row;
            this.Column = Column;
        }
    }
}

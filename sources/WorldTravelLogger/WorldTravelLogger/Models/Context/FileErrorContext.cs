using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace WorldTravelLogger.Models.Context
{
    public class FileErrorContext
    {
        public int X { get; set; }
        public int Y { get; set; }
        public string Context { get; set; }

        public FileErrorContext(int x, int y, string context)
        {
            X = x;
            Y = y;
            Context = context;
        }
    }
}

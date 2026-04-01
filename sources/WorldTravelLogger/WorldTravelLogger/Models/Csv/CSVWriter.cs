using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace WorldTravelLogger.Models.Csv
{
    class CSVWriter
    {
        public static bool WriteCSV(string filePath, object[] data)
        {
            try
            {
                using (StreamWriter writer = new StreamWriter(filePath))
                {
                    foreach (var row in (string[][])data)
                    {
                        for(var i = 0; i< row.Length; i++)
                        {
                            writer.Write(row[i]);
                            if (i == row.Length - 1)
                            {
                                writer.Write("\n");
                            }
                            else
                            {
                                writer.Write(",");
                            }
                        }
                    }
                }
                return true;

            }
            catch
            {
                return false;
            }
        }
    }
}

using SQLite;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RedrockAssess.Model
{
     public class HistoryModel
    {
        [PrimaryKey, AutoIncrement]
        public int _ID { get; set; }
        public string path { get; set; }
        public string name { get; set; }
    }
}

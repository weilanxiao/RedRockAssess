using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RedrockAssess.Model
{
    class Cache: INotifyPropertyChanged
    {
        public string name { get; set; }
        public string path { get; set; }

        public event PropertyChangedEventHandler PropertyChanged;
    }
}

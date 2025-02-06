using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CommonAPIBusinessLayer.Services.Interface
{
    public interface ISoundexService
    {
        bool NameComparison(string name1, string name2);
        string NameCheck(string name, int length);
    }
}

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CosmosKernel4
{
    class Variable
    {
        string var;
        int varVal;

        public Variable(string name, int vbl)
        {
            var = name;
            varVal = vbl;
        }
        public void setVar(string vbl)
        {
            var = vbl;
        }
        public void setVal(int val)
        {
            varVal = val;
        }

        public string getVar()
        {
            return var;
        }

        public int getVal()
        {
            return varVal;
        }
    }
}

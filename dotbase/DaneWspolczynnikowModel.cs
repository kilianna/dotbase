using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace DotBase
{
    class DaneWspolczynnikowModel
    {
        public string wykonal;
        public string sprawdzil;

        public DaneWspolczynnikowModel(string wykonal, string sprawdzil)
        {
            this.wykonal = wykonal;
            this.sprawdzil = sprawdzil;
        }
    }
}

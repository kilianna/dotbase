using System;
using System.Collections.Generic;
using System.Text;
using System.IO;
using System.Diagnostics;

namespace DotBase
{
    public abstract class AbcProtokol
    {
        private StreamReader _Fin;
        private StreamWriter _Fout;

        protected String _Szablon;
        
        //*******************************************
        public void PobierzSzablon(string sciezka)
        //*******************************************
        {
            _Fin = new System.IO.StreamReader(sciezka, Encoding.UTF8);
            _Szablon = _Fin.ReadToEnd();
            _Fin.Close();
        }

        //*******************************************
        abstract public void UtworzProtokol();
        //*******************************************

        //********************************************************************************************
        // Jako, że w klasy dziedziczące mogą wymagać większej liczby szablonów ostatecznie zapisanym/wypełnionym
        // szablonem nie musi być szablon podstawowy. Dlatego istnieje możliwość nadpisania tej metody.
        virtual protected void ZapiszPlikWynikowy(string sciezka)
        //********************************************************************************************
        {
            _Fout = new StreamWriter(sciezka, false, Encoding.UTF8);
            _Fout.Write(_Szablon.ToString());
            _Fout.Close();
        }
    }
}

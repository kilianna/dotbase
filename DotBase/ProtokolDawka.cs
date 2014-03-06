using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Narzedzia;
using System.Windows.Forms;

namespace DotBase
{
    namespace Dokumenty
    {

        class ProtokolDawka : WydrukiCez
        {
            //***************************************************
            public ProtokolDawka(StaleWzorcowan.stale typ)
                : base(typ)
            //***************************************************
            {
                _BazaDanych = new BazaDanychWrapper();
                WczytajSzablon(typ);
            }

            //***************************************************
            public bool PobierzDaneWzorcoweIPomiarowe(string zrodlo, string odleglosc, string wspolczynnik, string niepewnosc, DataGridViewRowCollection tabela)
            //***************************************************
            {
                _SzablonPodstawowy = _SzablonPodstawowy.Replace("<!zrodlo>", zrodlo);
                _SzablonPodstawowy = _SzablonPodstawowy.Replace("<!odleglosc>", odleglosc);

                String tabelaDoWpisania = "";

                try
                {
                    for (int i = 0; tabela[i].Cells[0].Value != null; ++i)
                    {
                        tabelaDoWpisania += String.Format("<tr align=\"center\" valign=\"middle\"><td><span>{0}</span></td>", i + 1)
                                          + String.Format("<td><span>{0}</span></td> <td><span>{1}</span></td></tr>",
                                                          tabela[i].Cells["WartoscWzorcowa"].Value.ToString(),
                                                          tabela[i].Cells["Wskazanie"].Value.ToString());
                    }
                }
                catch (Exception)
                {
                    return false;
                }


                double tempWspolczynnik, tempNiepewnosc;

                if (Double.TryParse(wspolczynnik, out tempWspolczynnik) && Double.TryParse(niepewnosc, out tempNiepewnosc))
                {
                    _SzablonPodstawowy = _SzablonPodstawowy.Replace("<!wspolczynnik>", String.Format("{0:0.000}", tempWspolczynnik));
                    _SzablonPodstawowy = _SzablonPodstawowy.Replace("<!niepewnosc>", String.Format("{0:0.000}", tempNiepewnosc));
                }
                else
                {
                    return false;
                }

                _SzablonPodstawowy.Replace("<!tabela>", tabelaDoWpisania);

                return true;
            }
        }
    }
}

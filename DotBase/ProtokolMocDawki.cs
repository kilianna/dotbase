using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Data;
using System.Windows.Forms;
using Narzedzia;

namespace DotBase
{
    namespace Dokumenty
    {
        class ProtokolMocDawki : WydrukiCez
        {
            //***************************************************
            public ProtokolMocDawki(StaleWzorcowan.stale typ)
                : base(typ)
            //***************************************************
            {
                _BazaDanych = new BazaDanychWrapper();
                WczytajSzablon(typ);
            }

            //***************************************************
            public bool PobierzDaneWzorcoweIPomiarowe(string jednostka, string wielkoscFizyczna, string tlo,
                                                      DataGridViewRowCollection tabela)
            //***************************************************
            {


                _SzablonPodstawowy = _SzablonPodstawowy.Replace("<!jednostka>", jednostka);
                _SzablonPodstawowy = _SzablonPodstawowy.Replace("<!wielk_fiz>", wielkoscFizyczna);
                _SzablonPodstawowy = _SzablonPodstawowy.Replace("<!tlo>", tlo);

                String tabelaDoWpisania = "";

                int licznik = 1;

                double wartoscWzorcowa;

                try
                {
                    for (int i = 0; tabela[i].Cells[0].Value != null; ++i)
                    {
                        wartoscWzorcowa = ((double)tabela[i].Cells[5].Value);

                        tabelaDoWpisania
                        += String.Format("<tr align=\"center\" valign=\"middle\"><td><span>{0}</span></td>", licznik)
                        + String.Format("<td><span>{0}</span></td> <td><span>{1}</span></td> <td><span>{2}</span></td> <td><span>{3}</span></td>",
                                        tabela[i].Cells[0].Value.ToString(),
                                        tabela[i].Cells[1].Value.ToString(),
                                        wartoscWzorcowa.ToString(Precyzja.Ustaw(wartoscWzorcowa)),
                                        tabela[i].Cells[2].Value.ToString())
                        + String.Format("<td><span>{0}</span></td> <td><span>{1}</span></td> </tr>",
                                        tabela[i].Cells[3].Value.ToString(),
                                        tabela[i].Cells[4].Value.ToString());
                        ++licznik;
                    }
                }
                catch (Exception)
                {
                    return false;
                }

                _SzablonPodstawowy = _SzablonPodstawowy.Replace("<!tabela>", tabelaDoWpisania);

                return true;
            }
        }
    }
}

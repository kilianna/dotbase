using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using System.Data;
using Narzedzia;

namespace DotBase
{
    namespace Dokumenty
    {
        class ProtokolSygnalizacjaDawki : WydrukiCez
        {
            //***************************************************
            public ProtokolSygnalizacjaDawki(StaleWzorcowan.stale stale)
                : base(stale)
            //***************************************************
            {
                _BazaDanych = new BazaDanychWrapper();
            }

            //***************************************************
            public bool PobierzDaneWzorcoweIPomiarowe(string odleglosc, string zrodlo, string jednostka, ref DataGridView tabela)
            //***************************************************
            {
                _SzablonPodstawowy = _SzablonPodstawowy.Replace("<!odleglosc>", odleglosc);
                _SzablonPodstawowy = _SzablonPodstawowy.Replace("<!zrodlo>", zrodlo);
                _SzablonPodstawowy = _SzablonPodstawowy.Replace("<!jednostka>", jednostka.Replace("/h", "").Replace("/s", ""));

                String tabelaDoWpisania = "";


                for (int i = 0; i < tabela.Rows.Count - 1; ++i)
                {
                    tabelaDoWpisania +=
                    String.Format("</tr><tr align=\"center\" valign=\"middle\"><td><span>{0}</span></td><td><span>{1}</span></td><td><span>{2}</span></td></tr>",
                                  tabela.Rows[i].Cells[0].Value.ToString(), tabela.Rows[i].Cells[3].Value.ToString(), tabela.Rows[i].Cells[4].Value.ToString());
                }

                _SzablonPodstawowy = _SzablonPodstawowy.Replace("<!tabela>", tabelaDoWpisania);

                return true;
            }
        }
    }
}

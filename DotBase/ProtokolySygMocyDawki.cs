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
        class ProtokolSygMocyDawki : WydrukiCez
        {
            bool _bSzablonOpisowy;

            //***************************************************
            public ProtokolSygMocyDawki(StaleWzorcowan.stale typ, bool szablonOpisowy)
                : base(typ)
            //***************************************************
            {
                _BazaDanych = new BazaDanychWrapper();
                _bSzablonOpisowy = szablonOpisowy;
            }

            //***************************************************
            public bool PobierzDaneWzorcoweIPomiarowe(string uwagi)
            //***************************************************
            {
                _SzablonPodstawowy = _SzablonPodstawowy.Replace("<!uwagiOpis>", uwagi);

                return true;
            }

            //***************************************************
            public bool PobierzDaneWzorcoweIPomiarowe(string jednostka, ref DataGridView tabela)
            //***************************************************
            {
                _SzablonPodstawowy = _SzablonPodstawowy.Replace("<!jednostka>", jednostka);

                String tabelaDoWpisania = "";

                for (int i = 0; i < tabela.Rows.Count - 1; ++i)
                {
                    tabelaDoWpisania +=
                    String.Format("</tr><tr align=\"center\" valign=\"middle\"><td><span>{0}</span></td><td><span>{1} &plusmn; {2}</span></td></tr>",
                                 tabela.Rows[i].Cells[0].Value.ToString(), tabela.Rows[i].Cells[5].Value.ToString(), tabela.Rows[i].Cells[6].Value.ToString());
                }

                _SzablonPodstawowy = _SzablonPodstawowy.Replace("<!tabela>", tabelaDoWpisania);

                return true;
            }
        }
    }
}

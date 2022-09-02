using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;

namespace DotBase
{
    /*
     * Design Pattern = Factory Method + Singleton
     */
    class DocumentsTemplatesFactory
    {
        public enum TemplateType
        {
            SCIEZKA_PLIK_POMOCNICZY_DAWKA,
            SCIEZKA_PLIK_POMOCNICZY_MOC_DAWKI,
            SCIEZKA_PLIK_POMOCNICZY_SKAZENIA,
            SCIEZKA_PLIK_POMOCNICZY_SYGNALIZACJA_DAWKI,
            SCIEZKA_PLIK_POMOCNICZY_SYGNALIZACJA_MOCY_DAWKI,
            SCIEZKA_PLIK_POMOCNICZY_SYGNALIZACJA_MOCY_DAWKI_OPIS,

            SCIEZKA_KARTA_PRZYJECIA,
            SCIEZKA_MELDUNEK,
            SCIEZKA_PISMO_PRZEWODNIE,
            SCIEZKA_SWIADECTWO,

            SCIEZKA_PROTOKOL_DAWKA,
            SCIEZKA_PROTOKOL_MOC_DAWKI,
            SCIEZKA_PROTOKOL_SKAZENIA,
            SCIEZKA_PROTOKOL_SYGNALIZACJA_DAWKI,
            SCIEZKA_PROTOKOL_SYGNALIZACJA_MOCY_DAWKI_OPIS,
            SCIEZKA_PROTOKOL_SYGNALIZACJA_MOCY_DAWKI_TABELA
        }

        private IDictionary<TemplateType, String> m_templatePaths;
        private static DocumentsTemplatesFactory m_instance_pl = new DocumentsTemplatesFactory(Jezyk.PL);
        private static DocumentsTemplatesFactory m_instance_en = new DocumentsTemplatesFactory(Jezyk.EN);

        public DocumentsTemplatesFactory(Jezyk jezyk) 
        {
            m_templatePaths = new Dictionary<TemplateType, String>();
            DocumentationPathsLoader dpl = new DocumentationPathsLoader();

            m_templatePaths.Add(TemplateType.SCIEZKA_PLIK_POMOCNICZY_DAWKA, dpl.GetPath("SwiadectwoSzablonPlikiPomocnicze", jezyk) + "Dawka.txt");
            m_templatePaths.Add(TemplateType.SCIEZKA_PLIK_POMOCNICZY_MOC_DAWKI, dpl.GetPath("SwiadectwoSzablonPlikiPomocnicze", jezyk) + "MocDawki.txt");
            m_templatePaths.Add(TemplateType.SCIEZKA_PLIK_POMOCNICZY_SKAZENIA, dpl.GetPath("SwiadectwoSzablonPlikiPomocnicze", jezyk) + "Skazenia.txt");
            m_templatePaths.Add(TemplateType.SCIEZKA_PLIK_POMOCNICZY_SYGNALIZACJA_DAWKI, dpl.GetPath("SwiadectwoSzablonPlikiPomocnicze", jezyk) + "SygDawki.txt");
            m_templatePaths.Add(TemplateType.SCIEZKA_PLIK_POMOCNICZY_SYGNALIZACJA_MOCY_DAWKI, dpl.GetPath("SwiadectwoSzablonPlikiPomocnicze", jezyk) + "SygMocyDawki.txt");
            m_templatePaths.Add(TemplateType.SCIEZKA_PLIK_POMOCNICZY_SYGNALIZACJA_MOCY_DAWKI_OPIS, dpl.GetPath("SwiadectwoSzablonPlikiPomocnicze", jezyk) + "SygMocyDawkiOpis.txt");

            m_templatePaths.Add(TemplateType.SCIEZKA_KARTA_PRZYJECIA, dpl.GetPath("KartaPrzyjeciaSzablon", jezyk) + "KartaPrzyjecia.html");
            m_templatePaths.Add(TemplateType.SCIEZKA_MELDUNEK, dpl.GetPath("MeldunekSzablon", jezyk) + "Meldunek.html");
            m_templatePaths.Add(TemplateType.SCIEZKA_PISMO_PRZEWODNIE, dpl.GetPath("PismoPrzewodnieSzablon", jezyk) + "PismoPrzewodnie.html");
            m_templatePaths.Add(TemplateType.SCIEZKA_SWIADECTWO, dpl.GetPath("SwiadectwoSzablon", jezyk) + "Swiadectwo.html");

            m_templatePaths.Add(TemplateType.SCIEZKA_PROTOKOL_DAWKA, dpl.GetPath("ProtokolyDawkaSzablon", jezyk) + "Dawka.html");
            m_templatePaths.Add(TemplateType.SCIEZKA_PROTOKOL_MOC_DAWKI, dpl.GetPath("ProtokolyMocDawkiSzablon", jezyk) + "MocDawki.html");
            m_templatePaths.Add(TemplateType.SCIEZKA_PROTOKOL_SKAZENIA, dpl.GetPath("ProtokolySkazeniaSzablon", jezyk) + "Skazenia.html");
            m_templatePaths.Add(TemplateType.SCIEZKA_PROTOKOL_SYGNALIZACJA_DAWKI, dpl.GetPath("ProtokolySygnalizacjaDawkiSzablon", jezyk) + "SygDawki.html");
            m_templatePaths.Add(TemplateType.SCIEZKA_PROTOKOL_SYGNALIZACJA_MOCY_DAWKI_OPIS, dpl.GetPath("ProtokolySygnalizacjaMocyDawkiSzablon", jezyk) + "SygMocyDawkiOpis.html");
            m_templatePaths.Add(TemplateType.SCIEZKA_PROTOKOL_SYGNALIZACJA_MOCY_DAWKI_TABELA, dpl.GetPath("ProtokolySygnalizacjaMocyDawkiSzablon", jezyk) + "SygMocyDawkiTabela.html");
        }

        public StringBuilder create(TemplateType templateType)
        {
            using (StreamReader sr = new StreamReader(m_templatePaths[templateType]))
            {
                return new StringBuilder(sr.ReadToEnd());
            }
        }

        static public DocumentsTemplatesFactory getInstance(Jezyk jezyk = Jezyk.PL)
        {
            return jezyk == Jezyk.PL ? m_instance_pl : m_instance_en;
        }
    }
}

using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using ZedGraph;
using DotBase;

namespace Wykres
{
    public partial class WykresForm : Form
    {
        bool        _WykresMocyDawki;
        bool        _PrzedluzonaWaznosc;
        Zakres      _DaneWejsciowe;
        LineItem    _DataLine;
        GraphPane   _ObszarRysowniczy;
        bool        _Poprawa;
        Jezyk       jezyk;

        private string tr(string pl)
        {
            if (jezyk == Jezyk.EN)
            {
                switch (pl)
                {
                    case "Wykres kalibracyjny dotyczący Świadectwa Wzorcowania nr ":
                        return "Calibration plot for certificate: ";
                    case "Wzorcowanie w zakresie dawki":
                        return "Calibration in terms of dose";
                    case "1 mR/h = 8,74 \u00B5Gy/h ":
                        return "1 mR/h = 8.74 \u00B5Gy/h ";
                    case "1 nA/kg = 121,9 \u00B5Gy/h":
                        return "1 nA/kg = 121.9 \u00B5Gy/h";
                    case "Data zalecanej kalibracji: ":
                        return null;
                    case " r.":
                        return "";
                    case "Hp (0,07) ":
                        return "Hp (0.07) ";
                    case "Wartości wzorcowe ":
                        return "Reference values ";
                    case "Wartości zmierzone ":
                        return "Measured values ";
                    case "Zastosowano przelicznik: ":
                        return "Conversion factor used: ";
                    case "Sonda: ":
                        return "Probe: ";
                    case "zakres ":
                        return "range ";
                    default:
                        MessageBox.Show("Nie ma tłumaczenie wyrażenia:\r\n" + pl, "Błąd krytyczny", MessageBoxButtons.OK, MessageBoxIcon.Error);
                        break;
                };
            }
            return pl;
        }

        //-------------------------------------------------------------------------
        public WykresForm(bool mocDawki, bool przedluzonaWaznosc, bool poprawa, Jezyk jezyk)
        //-------------------------------------------------------------------------
        {
            _WykresMocyDawki = mocDawki;
            InitializeComponent();
            _DaneWejsciowe = new Zakres();
            _PrzedluzonaWaznosc = przedluzonaWaznosc;
            _Poprawa = poprawa;
            this.jezyk = jezyk;
        }

        //-------------------------------------------------------------------------
        public void CzyscDane()
        //-------------------------------------------------------------------------
        {
            _DaneWejsciowe.CzyscDane();
        }

        //-------------------------------------------------------------------------
        public void DodajPunkt(double x, double y, double niepewnosc, double zakres, bool dolaczyc)
        //-------------------------------------------------------------------------
        {
            _DaneWejsciowe.DodajPunkt(x, y, niepewnosc, zakres, dolaczyc);
        }


        //-------------------------------------------------------------------------
        public bool Rysuj(string nrKarty, DateTime data, string jednostka, string sonda, string wielkoscFizycznaZOkna, int rownowaznik_dawki = -1)
        //-------------------------------------------------------------------------
        {
            try
            {
                Program.zmienJezyk(jezyk);

                _ObszarRysowniczy = zedGraphControl1.GraphPane;

                UstawieniaTytułu(nrKarty, data.Year.ToString());
                UstawieniaDaty(data);
                UstawieniaOsi(UstawienieJednostki(jednostka), rownowaznik_dawki, wielkoscFizycznaZOkna);

                if (SprawdzJednostke(jednostka) != "")
                    UstawieniaPrzelicznika(SprawdzJednostke(jednostka));

                UstawieniaSondy(sonda);

                foreach (Punkty zakres in _DaneWejsciowe.Zakresy)
                {
                    RysujPunktyZakresu(KonwertujPunkty(zakres));
                }

                Scale s = _ObszarRysowniczy.XAxis.Scale;

                RysujNiepewnosci();

                foreach (Punkty p in _DaneWejsciowe.Zakresy)
                {
                    RysujLinie(p);
                }

                _ObszarRysowniczy.XAxis.Scale.Max = s.Max;
                _ObszarRysowniczy.XAxis.Scale.Min = s.Min;

                zedGraphControl1.Invalidate();
            }
            finally
            {
                Program.zmienJezyk(Jezyk.PL);
            }
            return true;
        }

        struct PunktyTemp
        {
            public double[] x;
            public double[] y;
        };

        //-------------------------------------------------------------------------
        private void RysujLinie(Punkty zakres)
        //-------------------------------------------------------------------------
        {
            List<double> w = new List<double>();

            double s, sxy, sx, sy, sxx;

            s = sxy = sx = sy = sxx = 0;

            // wszystko dobrze się liczy więc zostawiliśmy to w świętym spokoju... Na wypadek gdyby trzeba było kiedyś uwzględnić niepewności
            // na razie wszysktie przyjmują neutralną wartość jedynki
            for (int i = 0; i < zakres.PobierzRozmiarDanych(); ++i)
            {
                w.Add(1.0);

                if (false == zakres.Dolaczyc[i])
                    continue;

                s += w[i];

                sx += Math.Log10(zakres.X[i]);
                sy += Math.Log10(zakres.Y[i]);
                sxy += Math.Log10(zakres.Y[i]) * Math.Log10(zakres.X[i]);
                sxx += Math.Log10(zakres.X[i]) * Math.Log10(zakres.X[i]);
            }

            double a, b, delta;

            delta = s * sxx - sx * sx;

            a = (s * sxy - sx * sy) / delta;
            b = (sxx * sy - sx * sxy) / delta;

            PunktyTemp punktyTemp = new PunktyTemp();
            punktyTemp.x = new double[2];
            punktyTemp.y = new double[2];

            // ustawiamy dowolnie dużą liczbę, która będzie na pewno większa od minimalnej wartości
            double min_x = Double.MaxValue;

            for (int i = 0; i < zakres.PobierzRozmiarDanych(); ++i)
            {
                if (false == zakres.Dolaczyc[i])
                    continue;

                if (zakres.X[i] < min_x)
                    min_x = zakres.X[i];
            }

            punktyTemp.x[0] = min_x;
            punktyTemp.y[0] = Math.Pow(10.0, a * Math.Log10(punktyTemp.x[0]) + b);


            // ustawiamy dowolnie małą liczbę, która będzie na pewno mniejsza od maksymalnej wartości
            double max_x = Double.MinValue;

            for (int i = 0; i < zakres.PobierzRozmiarDanych(); ++i)
            {
                if (false == zakres.Dolaczyc[i])
                    continue;

                if (zakres.X[i] > max_x)
                    max_x = zakres.X[i];
            }

            punktyTemp.x[1] = max_x;
            punktyTemp.y[1] = Math.Pow(10.0, a * Math.Log10(punktyTemp.x[1]) + b);

            _ObszarRysowniczy.AddCurve("", punktyTemp.x, punktyTemp.y, Color.Black, SymbolType.None);

            if (_DaneWejsciowe.Zakresy.Count > 1)
                UstawNazweZakresu();

            zedGraphControl1.Invalidate();
            /*
                if(false==jeden_zakres)
                {
                    // ustawienie odpowiedniej czcionki
                    wxFont czcionka = wxFont(10, wxFONTFAMILY_DEFAULT, wxFONTSTYLE_NORMAL, wxFONTWEIGHT_BOLD);
                    rysujacy.SetFont(czcionka);
                    //--------------------------------

                    zapytanie.Printf(wxT("zakres %g"), zakres);
                    rysujacy.DrawText(zapytanie, PX(temp.x)-40, PY(temp.y)-40);
                }
            */
        }

        //-------------------------------------------------------------------------
        private PointPairList KonwertujPunkty(Punkty zakres)
        //-------------------------------------------------------------------------
        {
            PointPairList punkty = new PointPairList();

            for (int i = 0; i < zakres.X.Count; i++)
                punkty.Add(zakres.X[i], zakres.Y[i]);

            return punkty;
        }

        //-------------------------------------------------------------------------
        public void UstawieniaTytułu(String numer_karty, String Rok)
        //-------------------------------------------------------------------------
        {
            _ObszarRysowniczy.Title.FontSpec.Family = "Arial";
            _ObszarRysowniczy.TitleGap = 1.1f;
            _ObszarRysowniczy.Title.FontSpec.Size = 11;
            _ObszarRysowniczy.Title.FontSpec.IsBold = true;
            _ObszarRysowniczy.Title.Text = tr("Wykres kalibracyjny dotyczący Świadectwa Wzorcowania nr ") + numer_karty + (_Poprawa ? "P" : "") + (jezyk != Jezyk.PL ? "/" + jezyk.ToString() : "") + "/" + Rok;
            if (false == _WykresMocyDawki)
            {
                _ObszarRysowniczy.Title.Text += "\n" + tr("Wzorcowanie w zakresie dawki");
            }
        }

        //-------------------------------------------------------------------------
        private String SprawdzJednostke(String jednostka)
        //-------------------------------------------------------------------------
        {
            if (jednostka.Contains('R'))
                return tr("1 mR/h = 8,74 \u00B5Gy/h ");
            else if (jednostka.Contains('A'))
                return tr("1 nA/kg = 121,9 \u00B5Gy/h");
            return "";
        }

        //-------------------------------------------------------------------------
        private void UstawieniaDaty(DateTime data)
        //-------------------------------------------------------------------------
        {
            String wpisData;

            if(_PrzedluzonaWaznosc)
                wpisData = String.Format("{0:dd.MM.yyyy}", data.AddYears(2));
            else
                wpisData = String.Format("{0:dd.MM.yyyy}", data.AddYears(1));

            TextObj myText;
            var tytul = tr("Data zalecanej kalibracji: ");
            if (tytul == null)
                myText = new TextObj("", 0.83, 0.085, CoordType.PaneFraction);
            else if (_WykresMocyDawki)
                myText = new TextObj(tytul + wpisData + tr(" r."), 0.83, 0.085, CoordType.PaneFraction);
            else
                myText = new TextObj(tytul + wpisData + tr(" r."), 0.83, 0.12, CoordType.PaneFraction);

            myText.FontSpec.FontColor = Color.Black;
            myText.FontSpec.Family = "Arial";
            myText.FontSpec.Size = 9;
            myText.FontSpec.Border.IsVisible = false;
            myText.FontSpec.IsBold = true;
            myText.IsClippedToChartRect = false;
            _ObszarRysowniczy.GraphObjList.Add(myText);
            _ObszarRysowniczy.GraphObjList[0].ZOrder = ZOrder.H_BehindAll;
        }

        //-------------------------------------------------------------------------
        private string UstawienieJednostki(String jednostka)
        //-------------------------------------------------------------------------
        {
            if (jednostka.Contains('u'))
                jednostka = jednostka.Replace('u', '\u03BC');
            return jednostka;
        }

        //-------------------------------------------------------------------------
        private void UstawieniaOsi(String jednostka, int rownowaznik_dawki, string wielkoscFizycznaZOkna)
        //-------------------------------------------------------------------------
        {
            String wielkoscFizyczna = "";

            //typ osi
            _ObszarRysowniczy.XAxis.Type = AxisType.Log;
            _ObszarRysowniczy.YAxis.Type = AxisType.Log;

            _ObszarRysowniczy.XAxis.MinorTic.IsAllTics = false;
            _ObszarRysowniczy.YAxis.MinorTic.IsAllTics = false;

            //siatka
            _ObszarRysowniczy.XAxis.MajorGrid.IsVisible = true;
            _ObszarRysowniczy.YAxis.MajorGrid.IsVisible = true;

            _ObszarRysowniczy.XAxis.MinorGrid.IsVisible = true;
            _ObszarRysowniczy.YAxis.MinorGrid.IsVisible = true;

            _ObszarRysowniczy.XAxis.MajorGrid.DashOn = 0.0f;
            _ObszarRysowniczy.YAxis.MajorGrid.DashOn = 0.0f;

            _ObszarRysowniczy.XAxis.MajorGrid.PenWidth = 0.5f;
            _ObszarRysowniczy.YAxis.MajorGrid.PenWidth = 0.5f;

            _ObszarRysowniczy.XAxis.MinorGrid.PenWidth = 0.5f;
            _ObszarRysowniczy.YAxis.MinorGrid.PenWidth = 0.5f;

            _ObszarRysowniczy.XAxis.MinorGrid.DashOn = 0.0f;
            _ObszarRysowniczy.YAxis.MinorGrid.DashOn = 0.0f;

            _ObszarRysowniczy.XAxis.MinorGrid.Color = Color.Black;
            _ObszarRysowniczy.YAxis.MinorGrid.Color = Color.Black;

            //podpisy osi
            _ObszarRysowniczy.XAxis.Title.FontSpec.IsBold = true;
            _ObszarRysowniczy.YAxis.Title.FontSpec.IsBold = true;

            _ObszarRysowniczy.XAxis.Title.FontSpec.Size = 9;
            _ObszarRysowniczy.YAxis.Title.FontSpec.Size = 9;

            _ObszarRysowniczy.XAxis.Title.FontSpec.Family = "Arial";
            _ObszarRysowniczy.YAxis.Title.FontSpec.Family = "Arial";

            if (jednostka.Contains("Sv"))
            {
                if (_WykresMocyDawki)
                {
                    if (wielkoscFizycznaZOkna != null && (
                        wielkoscFizycznaZOkna.IndexOf("Hp(10)") >= 0 ||
                        wielkoscFizycznaZOkna.IndexOf("Hp (10)") >= 0))
                    {
                        wielkoscFizyczna = "\u1E22p (10) ";
                    }
                    else
                    {
                        wielkoscFizyczna = "\u1E22* ";
                    }
                }
                else
                {
                    if (rownowaznik_dawki == 0)
                    {
                        wielkoscFizyczna = "Hp (10) ";
                       // JestemIdiotycznaFunkcja();
                    }
                    else if (rownowaznik_dawki == 1)
                    {
                        wielkoscFizyczna = tr("Hp (0,07) ");
                        //JestemIdiotycznaFunkcja();
                    }
                    else if (rownowaznik_dawki == 2)
                    {
                        wielkoscFizyczna = "H* ";
                        //JestemIdiotycznaFunkcja();
                    }
                }
            }

            if (jednostka == "mGy" && !_WykresMocyDawki)
            {
                wielkoscFizyczna = "Ka ";
            }

            if (jednostka.Contains("cps") || jednostka.Contains("cpm") ||
                jednostka.Contains("s-1") || jednostka.Contains("1/s") ||
                jednostka.Contains("imp/min") || jednostka.Contains("imp/s") ||
                jednostka.Contains("1/min") || jednostka.Contains("Bq/cm2"))
            {
                _ObszarRysowniczy.XAxis.Title.Text = tr("Wartości wzorcowe ") + "\u00B5Gy/h";
            }
            else
            {
                _ObszarRysowniczy.XAxis.Title.Text = tr("Wartości wzorcowe ") + wielkoscFizyczna + jednostka;
            }

            _ObszarRysowniczy.YAxis.Title.Text = tr("Wartości zmierzone ") + jednostka;

            //ustawienia liczb
            _ObszarRysowniczy.XAxis.Scale.FontSpec.Size = 9;
            _ObszarRysowniczy.YAxis.Scale.FontSpec.Size = 9;
            _ObszarRysowniczy.XAxis.Scale.IsUseTenPower = false;
            _ObszarRysowniczy.YAxis.Scale.IsUseTenPower = false;
            _ObszarRysowniczy.XAxis.Scale.Format = "g";
            _ObszarRysowniczy.YAxis.Scale.Format = "g";
        }

        //-------------------------------------------------------------------------
        /* Dlaczego jestem idiotyczną? C# czy Windows czy kto wie co nie wspiera litery p jako dolnego indeksu
         * a przynajmniej tej p która jest zapisana w dokumentacji UTF-8. Wszystkie inne "dziwne" litery/znaki działają
         * ale litery w formie dolnego indeksu NIE. Dlatego ja oszukuję i rysuję p jako dolny indeks w określonym miejscu.
         * Nawet nie rysuję dolnego indeksu. Po prosu rysuję p w odpowiednim miejscu pomniejszoną czcionką.
         * Jeśli przeglądasz ten kod i wiesz jak to zrobić napisz => Ziemowit.Stolarczyk@gmail.com
         * Moje życie psychiczne osiągnie równowagę po tej inforamacji!
         * Dziękuję za uwagę.
         * Zirytowany programista - Ziemowit Stolarczyk
         */
        private void JestemIdiotycznaFunkcja()
        //-------------------------------------------------------------------------
        {
            TextObj myText = new TextObj("p", 0.562, 1.065, CoordType.ChartFraction);
            myText.FontSpec.FontColor = Color.Black;
            myText.FontSpec.Family = "Arial";
            myText.FontSpec.Size = 6;
            myText.FontSpec.Border.IsVisible = false;
            myText.FontSpec.IsBold = true;
            myText.IsClippedToChartRect = false;
            myText.FontSpec.StringAlignment = StringAlignment.Near;
            _ObszarRysowniczy.GraphObjList.Add(myText);
            _ObszarRysowniczy.GraphObjList[1].ZOrder = ZOrder.A_InFront;
        }

        //-------------------------------------------------------------------------
        private void RysujNiepewnosci()
        //-------------------------------------------------------------------------
        {
            PointPairList punktyDoRysowania = new PointPairList();

            foreach (Punkty punkty in _DaneWejsciowe.Zakresy)
            {
                for (int i = 0; i < punkty.PobierzRozmiarDanych(); ++i)
                {
                    punktyDoRysowania.Add(punkty.X[i], punkty.Y[i] + punkty.Niepewnosc[i], punkty.Y[i] - punkty.Niepewnosc[i]);
                }
            }

            
            ErrorBarItem temp = _ObszarRysowniczy.AddErrorBar("", punktyDoRysowania, Color.Black);
            temp.Bar.Symbol.Size = 4;
            
        }

        //-------------------------------------------------------------------------
        private void RysujPunktyZakresu(PointPairList dane)
        //-------------------------------------------------------------------------
        {
            //rysuj punkty
            _DataLine = _ObszarRysowniczy.AddCurve("", dane, Color.Transparent, SymbolType.Diamond);

            //ustawienie symboli
            _DataLine.Symbol.Fill = new Fill(Color.Black);
            _DataLine.Symbol.Size = 5;

            //odświeżenie
            zedGraphControl1.AxisChange();
        }

        //-------------------------------------------------------------------------
        private void UstawieniaPrzelicznika(String przelicznik)
        //-------------------------------------------------------------------------
        {
            TextObj myText = new TextObj(tr("Zastosowano przelicznik: ") + przelicznik, 0.26, 0.085, CoordType.PaneFraction);
            myText.FontSpec.FontColor = Color.Black;
            myText.FontSpec.Family = "Arial";
            myText.FontSpec.Size = 9;
            myText.FontSpec.Border.IsVisible = false;
            myText.FontSpec.IsBold = true;
            myText.IsClippedToChartRect = false;
            myText.FontSpec.StringAlignment = StringAlignment.Near;
            _ObszarRysowniczy.GraphObjList.Add(myText);
            _ObszarRysowniczy.GraphObjList[1].ZOrder = ZOrder.H_BehindAll;
        }

        //-------------------------------------------------------------------------
        private void UstawieniaSondy(String sonda)
        //-------------------------------------------------------------------------
        {
            TextObj myText = new TextObj(tr("Sonda: ") + TranslacjaForm.Tlumacz(sonda, jezyk), 0.097, 0.96, CoordType.PaneFraction, AlignH.Left, AlignV.Center);
            myText.FontSpec.FontColor = Color.Black;
            myText.FontSpec.Family = "Arial";
            myText.FontSpec.Size = 9;
            myText.FontSpec.Border.IsVisible = false;
            myText.FontSpec.IsBold = true;
            myText.IsClippedToChartRect = false;
            myText.FontSpec.StringAlignment = StringAlignment.Near;
            _ObszarRysowniczy.GraphObjList.Add(myText);
            _ObszarRysowniczy.GraphObjList[1].ZOrder = ZOrder.H_BehindAll;
        }

        //-------------------------------------------------------------------------
        private void UstawNazweZakresu()
        //-------------------------------------------------------------------------
        {
            TextObj podpis;

            for (int i = 0; i < _DaneWejsciowe.Zakresy.Count; ++i)
            {
                Punkty punkty = _DaneWejsciowe.Zakresy[i];
                double x_srodek = (punkty.ZnajdzMaxX() + punkty.ZnajdzMinX()) / 2;
                double y_max = punkty.ZnajdzMaxY() * 1.5;
                podpis = new TextObj(tr("zakres ") + punkty.Zakres, x_srodek, y_max);
                podpis.FontSpec.FontColor = Color.Black;
                podpis.FontSpec.Family = "Arial";
                podpis.FontSpec.Size = 6;
                podpis.FontSpec.Border.IsVisible = false;
                podpis.FontSpec.IsBold = true;
                podpis.IsClippedToChartRect = false;
                podpis.ZOrder = ZOrder.F_BehindGrid;

                _ObszarRysowniczy.GraphObjList.Add(podpis);
            }
        }
    }

    public class Punkty
    {
        public double Zakres { get; private set; }
        public List<double> X { get; private set; }
        public List<double> Y { get; private set; }
        public List<double> Niepewnosc { get; private set; }
        public List<bool> Dolaczyc { get; private set; }

        //***************************************************************
        public Punkty(double x, double y, double niepewnosc, double zakres, bool dolaczyc)
        //***************************************************************
        {
            Zakres = zakres;
            
            X = new List<double>();
            X.Add(x);
            
            Y = new List<double>();
            Y.Add(y);

            Dolaczyc = new List<bool>();
            Dolaczyc.Add(dolaczyc);

            Niepewnosc = new List<double>();
            Niepewnosc.Add(niepewnosc);
        }

        //***************************************************************
        public void CzyscDane()
        //***************************************************************
        {
            Zakres = 0.0;
            X.Clear();
            Y.Clear();
            Dolaczyc.Clear();
            Niepewnosc.Clear();
        }

        //***************************************************************
        public void DodajDane(double x, double y, double niepewnosc, bool dolaczyc)
        //***************************************************************
        {
            X.Add(x);
            Y.Add(y);
            Niepewnosc.Add(niepewnosc);
            Dolaczyc.Add(dolaczyc);
        }

        //***************************************************************
        public int PobierzRozmiarDanych()
        //***************************************************************
        {
            return X.Count;
        }

        //***************************************************************
        public double ZnajdzMaxX()
        //***************************************************************
        {
            double max = Double.MinValue;
            for (int i = 1; i < X.Count; ++i)
            {
                if (Dolaczyc[i] && max < X[i])
                {
                    max = X[i];
                }
            }

            return max;
        }

        //***************************************************************
        public double ZnajdzMinX()
        //***************************************************************
        {
            double min = Double.MaxValue;
            for (int i = 1; i < X.Count; ++i)
            {
                if (Dolaczyc[i] && min > X[i])
                {
                    min = X[i];
                }
            }

            return min;
        }

        //***************************************************************
        public int ZnajdzLiczbeUzytychPunktowX()
        //***************************************************************
        {
            return Dolaczyc.Count(n => n == true);
        }

        //***************************************************************
        public double ZnajdzMaxY()
        //***************************************************************
        {
            double max = Double.MinValue;
            for (int i = 1; i < Y.Count; ++i)
            {
                if (Dolaczyc[i] && max < Y[i])
                {
                    max = Y[i];
                }
            }

            return max;
        }


        //***************************************************************
        public double ZnajdzMinY()
        //***************************************************************
        {
            double min = Double.MaxValue;
            for (int i = 1; i < Y.Count; ++i)
            {
                if (Dolaczyc[i] && min > Y[i])
                {
                    min = Y[i];
                }
            }

            return min;
        }
    }

    public class Zakres
    {
        public List<Punkty> Zakresy { get; private set; }

        //***************************************************************
        public Zakres()
        //***************************************************************
        {
            Zakresy = new List<Punkty>();
        }

        //***************************************************************
        public void DodajPunkt(double x, double y, double niepewnosc, double zakres, bool dolaczyc)
        //***************************************************************
        {
            for (int i = 0; i < Zakresy.Count; ++i)
            {
                if (Zakresy[i].Zakres == zakres)
                {
                    Zakresy[i].DodajDane(x, y, niepewnosc, dolaczyc);
                    return;
                }
            }

            Zakresy.Add(new Punkty(x, y, niepewnosc, zakres, dolaczyc));
        }

        //***************************************************************
        public void CzyscDane()
        //***************************************************************
        {
            Zakresy.Clear();
        }


    }
}

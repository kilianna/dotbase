using System.Data;
using DotBase;

abstract class WspolneDaneWzorcowan
{
    public int IdArkusza { get; protected set; }
    public int IdKarty { get; protected set; }
    public int IdWzorcowania { get; protected set; }
    public int IdZrodla { get; protected set; }

    protected string _Zapytanie;
    protected BazaDanychWrapper _BazaDanych;
    protected DataTable _OdpowiedzBazy;
    
    public DataTable OdpowiedzBazy
    {
        get
        {
            return _OdpowiedzBazy;
        }
    }

    public abstract bool Inicjalizuj();
    public abstract bool StworzNowyArkusz();
    public abstract bool ZnajdzMaksymalnyArkusz();
    public abstract bool ZnajdzMinimalnyArkusz();
    public abstract bool ZnajdzMniejszyArkusz();
    public abstract bool ZnajdzWiekszyArkusz();
}
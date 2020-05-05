using AccountManager.Domain.SeedWork;

namespace AccountManager.Domain.Aggregates.ContaCorrenteAggregate
{
    public class TipoLancamento 
        : Enumeration
    {
        public static TipoLancamento Debito = new TipoLancamento(1, "Débito");
        public static TipoLancamento Credito = new TipoLancamento(2, "Crédito");

        public TipoLancamento(
            int id, 
            string nome) : base(id, nome)
        {
        }
    }
}

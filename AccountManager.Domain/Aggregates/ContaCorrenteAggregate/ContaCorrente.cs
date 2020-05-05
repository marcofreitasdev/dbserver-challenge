using AccountManager.Domain.Aggregates.ContaCorrente;
using AccountManager.Domain.Exceptions;
using System;
using System.Linq;
using System.Collections.Generic;
using AccountManager.Domain.SeedWork;

namespace AccountManager.Domain.Aggregates.ContaCorrenteAggregate
{
    public class ContaCorrente 
        : Entity, IAggregateRoot
    {
        private List<Lancamento> lancamentos = new List<Lancamento>();

        public ContaCorrente(
            Guid id,
            Guid correntistaId,
            IEnumerable<Lancamento> lancamentos = null)
        {
            if (id == null || id == Guid.Empty)
            {
                throw new AccountManagerDomainException("Id da conta corrente é obrigatório");
            }

            if (correntistaId == null || correntistaId == Guid.Empty)
            {
                throw new AccountManagerDomainException("CorrentistaId da conta corrente é obrigatório");
            }

            Id = id;
            CorrentistaId = correntistaId;

            if (lancamentos != null)
            {
                this.lancamentos.AddRange(lancamentos);
            }
        }

        public Guid CorrentistaId { get; private set; }

        public void AdicionarLancamento(
            TipoLancamento tipo,
            decimal valor,
            DateTime data)
        {
            lancamentos.Add(new Lancamento(
                tipo,
                data,
                valor));
        }

        public decimal Saldo => lancamentos.Sum(l =>
        {
            if (l.TipoLancamento == TipoLancamento.Debito)
            {
                return (l.Valor * -1);
            }
            else
            {
                return l.Valor;
            }
        });
    }
}

using AccountManager.Domain.Aggregates.ContaCorrenteAggregate;
using AccountManager.Domain.Exceptions;
using AccountManager.Domain.SeedWork;
using System;
using System.Collections.Generic;

namespace AccountManager.Domain.Aggregates.ContaCorrente
{
    public class Lancamento : ValueObject
    {
        public Lancamento(
            TipoLancamento tipoLancamento,
            DateTime data,
            decimal valor)
        {
            if (valor <= 0)
            {
                throw new AccountManagerDomainException("O valor do lançamento deve ser maior que zero");
            }

            TipoLancamento = tipoLancamento;
            Valor = valor;
            Data = data;
        }

        public TipoLancamento TipoLancamento { get; private set; }
        public decimal Valor { get; private set; }
        public DateTime Data { get; private set; }

        protected override IEnumerable<object> GetAtomicValues()
        {
            yield return TipoLancamento;
            yield return Data;
            yield return Valor;
        }
    }
}

using AccountManager.Domain.Aggregates.ContaCorrenteAggregate;
using AccountManager.Domain.Exceptions;
using System;

namespace AccountManager.Domain.Services
{
    public class OperacaoDomainService
    {
        public void EfetuarTransacao(
            ContaCorrente contaOrigem, 
            ContaCorrente contaDestino, 
            DateTime data,
            decimal valorOperacao)
        {
            ValidarContas(contaOrigem, contaDestino);

            ChecarSaldoSuficiente(
                contaOrigem.Saldo, 
                valorOperacao);

            contaOrigem.AdicionarLancamento(
                TipoLancamento.Debito,
                valorOperacao,
                data);

            contaDestino.AdicionarLancamento(
                TipoLancamento.Credito,
                valorOperacao,
                data);
        }

        private void ValidarContas(ContaCorrente contaOrigem, ContaCorrente contaDestino)
        {
            if (contaDestino == contaOrigem)
            {
                throw new AccountManagerDomainException("Conta de origem e destino devem ser diferentes para realizar esta operação");
            }
        }

        private void ChecarSaldoSuficiente(
            decimal saldoContaOrigem,
            decimal valorOperacao)
        {
            if (saldoContaOrigem < valorOperacao)
            {
                throw new AccountManagerDomainException("O saldo da conta não é suficiente para realizar esta operação");
            }
        }
    }
}

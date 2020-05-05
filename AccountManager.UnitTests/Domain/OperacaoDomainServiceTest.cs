using AccountManager.Domain.Aggregates.ContaCorrente;
using AccountManager.Domain.Aggregates.ContaCorrenteAggregate;
using AccountManager.Domain.Exceptions;
using AccountManager.Domain.Services;
using System;
using System.Collections.Generic;
using Xunit;

namespace AccountManager.UnitTests.Domain
{
    public class OperacaoDomainServiceTest
    {
        private List<Lancamento> LancamentosBuilder()
        {
            return new List<Lancamento>
            {
                new Lancamento(TipoLancamento.Credito, DateTime.Now.AddDays(-3), 1000),
                new Lancamento(TipoLancamento.Debito, DateTime.Now.AddDays(-2), 200.56m),
                new Lancamento(TipoLancamento.Debito, DateTime.Now.AddDays(-2), 99.44m)
            };
        }

        [Fact]
        public void Operacao_Credito_Suficiente_Sucesso()
        {
            //Arrange
            var contaOrigem = new ContaCorrente(
                Guid.NewGuid(),
                Guid.NewGuid(),
                LancamentosBuilder());

            var contaDestino = new ContaCorrente(
                Guid.NewGuid(),
                Guid.NewGuid(),
                LancamentosBuilder());

            var valorOperacao = 400;
            var dataOperacao = DateTime.Now;

            var contaOrigemSaldoEsperado = 300;
            var contaDestinoSaldoEsperado = 1100;
            var operacaoService = new OperacaoDomainService();

            //Act
            operacaoService.EfetuarTransacao(
                contaOrigem,
                contaDestino,
                dataOperacao,
                valorOperacao);

            //Assert
            Assert.Equal(contaOrigemSaldoEsperado, contaOrigem.Saldo);
            Assert.Equal(contaDestinoSaldoEsperado, contaDestino.Saldo);
        }

        [Fact]
        public void Operacao_Credito_Insuficiente_Falha()
        {
            //Arrange
            var contaOrigem = new ContaCorrente(
                Guid.NewGuid(),
                Guid.NewGuid(),
                LancamentosBuilder());

            var contaDestino = new ContaCorrente(
                Guid.NewGuid(),
                Guid.NewGuid(),
                LancamentosBuilder());

            var valorOperacao = 800;
            var dataOperacao = DateTime.Now;
            var operacaoService = new OperacaoDomainService();

            //Act - Assert
            Assert.Throws<AccountManagerDomainException>(() =>
            {
                operacaoService.EfetuarTransacao(
                    contaOrigem,
                    contaDestino,
                    dataOperacao,
                    valorOperacao);
            });
        }

        [Fact]
        public void Operacao_Contas_Mesma_Origem_Falha()
        {
            //Arrange
            var contaCorrenteId = Guid.NewGuid();
            var contaOrigem = new ContaCorrente(
                contaCorrenteId,
                Guid.NewGuid(),
                LancamentosBuilder());

            var contaDestino = new ContaCorrente(
                contaCorrenteId,
                Guid.NewGuid(),
                LancamentosBuilder());

            var valorOperacao = 800;
            var dataOperacao = DateTime.Now;
            var operacaoService = new OperacaoDomainService();

            //Act - Assert
            Assert.Throws<AccountManagerDomainException>(() =>
            {
                operacaoService.EfetuarTransacao(
                    contaOrigem,
                    contaDestino,
                    dataOperacao,
                    valorOperacao);
            });
        }
    }
}

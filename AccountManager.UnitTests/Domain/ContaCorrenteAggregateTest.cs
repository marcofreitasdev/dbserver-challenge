using AccountManager.Domain.Aggregates.ContaCorrente;
using AccountManager.Domain.Aggregates.ContaCorrenteAggregate;
using AccountManager.Domain.Exceptions;
using System;
using Xunit;

namespace AccountManager.UnitTests.Domain
{
    public class ContaCorrenteAggregateTest
    {
        [Fact]
        public void Criar_Conta_Corrente_Com_Id_Invalido_Falha()
        {
            //Arrange
            var contaCorrenteIdInvalido = Guid.Empty;
            var correntistaId = Guid.NewGuid();

            //Act - Assert
            Assert.Throws<AccountManagerDomainException>(() => {
                new ContaCorrente(
                contaCorrenteIdInvalido,
                correntistaId);
            });
        }

        [Fact]
        public void Criar_Conta_Corrente_Com_CorrentistaId_Invalido_Falha()
        {
            //Arrange
            var contaCorrenteId = Guid.NewGuid();
            var correntistaIdInvalido = Guid.Empty;

            //Act - Assert
            Assert.Throws<AccountManagerDomainException>(() => {
                new ContaCorrente(
                contaCorrenteId,
                correntistaIdInvalido);
            });
        }

        [Fact]
        public void Criar_Conta_Corrente_Sem_Lancamentos_Saldo_Inicial_Zero_Sucesso()
        {
            //Arrange
            var contaCorrenteId = Guid.NewGuid();
            var correntistaId = Guid.NewGuid();

            //Act
            var contaCorrente = new ContaCorrente(
                contaCorrenteId,
                correntistaId);

            //Assert
            Assert.Equal(0, contaCorrente.Saldo);
        }

        [Fact]
        public void Criar_Conta_Corrente_Saldo_Inicial_Positivo_Sucesso()
        {
            //Arrange
            var contaCorrenteId = Guid.NewGuid();
            var correntistaId = Guid.NewGuid();
            var valorInicial = 1000m;
            var saldoInicial = new Lancamento[] {
                new Lancamento(
                    TipoLancamento.Credito,
                    DateTime.Now,
                    valorInicial)
            };

            //Act
            var contaCorrente = new ContaCorrente(
                contaCorrenteId,
                correntistaId,
                saldoInicial);

            //Assert
            Assert.Equal(1000m, contaCorrente.Saldo);
        }

        [Fact]
        public void Criar_Conta_Corrente_Saldo_Inicial_Negativo_Sucesso()
        {
            //Arrange
            var contaCorrenteId = Guid.NewGuid();
            var correntistaId = Guid.NewGuid();
            var valorInicial = 1000m;
            var saldoInicial = new Lancamento[] {
                new Lancamento(
                    TipoLancamento.Debito,
                    DateTime.Now,
                    valorInicial)
            };

            //Act
            var contaCorrente = new ContaCorrente(
                contaCorrenteId,
                correntistaId,
                saldoInicial);

            //Assert
            Assert.Equal(-1000m, contaCorrente.Saldo);
        }

        [Fact]
        public void Criar_Lancamento_Valor_Negativo_Falha()
        {
            //Arrange
            var valorInvalido = -1000m;

            //Act - Assert
            Assert.Throws<AccountManagerDomainException>(() =>
            {
                new Lancamento(
                    TipoLancamento.Credito,
                    DateTime.Now,
                    valorInvalido);
            });
        }

        [Fact]
        public void Criar_Lancamento_Valor_Positivo_Sucesso()
        {
            //Arrange
            var valorValido = 1000m;

            //Act
            var lancamento = new Lancamento(
                TipoLancamento.Debito,
                DateTime.Now,
                valorValido);

            //Assert
            Assert.Equal(1000m, lancamento.Valor);
        }

        [Fact]
        public void Criar_Lancamento_Valor_Zero_Falha()
        {
            //Arrange
            var valorInvalido = 0m;

            //Act - Assert
            Assert.Throws<AccountManagerDomainException>(() =>
            {
                new Lancamento(
                    TipoLancamento.Credito,
                    DateTime.Now,
                    valorInvalido);
            });
        }
    }
}

using AccountManager.Domain.Aggregates.ContaCorrenteAggregate;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace AccountManager.InfraStructure
{
    public class ContaCorrenteRepositoryInMemory
        : IContaCorrenteRepository
    {
        private readonly Dictionary<Guid, ContaCorrente> contasCorrentes;

        public ContaCorrenteRepositoryInMemory()
        {
            contasCorrentes = new Dictionary<Guid, ContaCorrente>();
        }

        public void Add(ContaCorrente contaCorrente)
        {
            if (!contasCorrentes.ContainsKey(contaCorrente.Id))
            {
                contasCorrentes.Add(contaCorrente.Id, contaCorrente);
            }
            else
            {
                throw new InvalidOperationException($"Erro ao incluir conta corrente: '{contaCorrente.Id}' já existe");
            }
        }

        public void Delete(ContaCorrente aggregate)
        {
            if (contasCorrentes.ContainsKey(aggregate.Id))
            {
                contasCorrentes.Remove(aggregate.Id);
            }
        }

        public Task<ContaCorrente> FindByIdAsync(Guid id)
        {
            ContaCorrente result = null;

            if (contasCorrentes.ContainsKey(id))
            {
                result = contasCorrentes[id];
            }

            return Task.FromResult(result);
        }

        public Task<IEnumerable<ContaCorrente>> GetAll()
        {
            var result = contasCorrentes.Values.AsEnumerable();
            return Task.FromResult(result);
        }

        public void Update(ContaCorrente contaCorrente)
        {
            if (contasCorrentes.ContainsKey(contaCorrente.Id))
            {
                contasCorrentes[contaCorrente.Id] = contaCorrente;
            }
            else
            {
                throw new InvalidOperationException($"Erro ao atualizar conta corrente: '{contaCorrente.Id}' não encontrada");
            }
        }
    }
}

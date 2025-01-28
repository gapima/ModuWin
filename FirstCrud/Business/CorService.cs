using FirstCrud.Data;
using FirstCrud.Entities;
using System.Collections.Generic;

namespace FirstCrud.Business
{
    public class CorService
    {
        private readonly CorRepository _repository;

        public CorService()
        {
            _repository = new CorRepository();
        }

        public IEnumerable<Cor> ObterTodasCores()
        {
            return _repository.GetAll();
        }

        public void AdicionarCor(Cor cor)
        {
            if (string.IsNullOrWhiteSpace(cor.Desc_Cor))
                throw new ArgumentException("O nome da cor não pode ser vazio.");

            _repository.Add(cor);
        }

        public void AtualizarCor(Cor cor)
        {
            if (cor.Id_Cor <= 0)
                throw new ArgumentException("ID inválido para atualização.");

            _repository.Update(cor);
        }

        public void ExcluirCor(int id)
        {
            if (id <= 0)
                throw new ArgumentException("ID inválido para exclusão.");

            _repository.Delete(id);
        }
    }
}

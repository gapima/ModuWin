using Dapper;
using FirstCrud.Data;
using FirstCrud.Entities;
using System.Collections.Generic;
using System.Data;

namespace FirstCrud.Data
{
    public class CorRepository
    {
        public IEnumerable<Cor> GetAll()
        {
            using (var connection = DbConnectionHelper.GetConnection())
            {
                var table = connection.Query<Cor>("Sc_Ger_S_Cor", commandType: CommandType.StoredProcedure);
                return table;
            }
        }

        public int Add(Cor cor)
        {
            using (var connection = DbConnectionHelper.GetConnection())
            {
                var parameters = new DynamicParameters();
                parameters.Add("@Desc_Cor", cor.Desc_Cor);
                parameters.Add("@Id", dbType: DbType.Int32, direction: ParameterDirection.Output);
                connection.Execute("sc_Ger_I_cor", parameters, commandType: CommandType.StoredProcedure);
                return parameters.Get<int>("@Id");
            }
        }

        public void Update(Cor cor)
        {
            using (var connection = DbConnectionHelper.GetConnection())
            {
                var parameters = new { cor.Id_Cor, cor.Desc_Cor };
                connection.Execute("sc_Ger_u_cor", parameters, commandType: CommandType.StoredProcedure);
            }
        }

        public void Delete(int id)
        {
            using (var connection = DbConnectionHelper.GetConnection())
            {
                var parameters = new { V_Id_Cor = id };
                connection.Execute("sc_Ger_d_cor", parameters, commandType: CommandType.StoredProcedure);
            }
        }
    }
}

using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using MBR.Models;
using System.Linq.Expressions;
using System.Data;
using System.Data.Objects.SqlClient;
using System.Data.Objects;
using System.Threading.Tasks;
using System.Data.SqlClient;
using MySql.Data.MySqlClient;

namespace MBR.Web.Services
{
    public class RunningInfoService : BaseService<MBR.Models.MBR_RunningInfo>
    {
        public RunningInfoService() { }
        public RunningInfoService(MBREntities db) { this.db = db; }

        public IEnumerable<MBR_RunningInfo> GetRunningInfoByDayList(DateTime StartDate, DateTime EndDate, int PoolID = 0)
        {
            string whereClause = string.Empty;
            if (PoolID > 0)
            {
                whereClause += string.Format(" AND PoolID = {0} ", PoolID);
            }

            string sql = string.Format(@"SELECT
                    0 AS RunningInfoID,
                    0 AS PoolID, 
                    DATE_FORMAT(UpdateTime,'%Y-%m-%d') AS UpdateTime,
                    AVG(IFNULL(ChanShuiLL,0)) AS ChanShuiLL,
                    0.0 AS ChanShuiYL,
                    0.0 AS DanChiMCXQL, 
                    0.0 AS WuNiND,
                    0.0 AS RongJieY, 
                    0.0 AS KuaMoYC, 
                    AVG(IFNULL(TouShuiL,0)) AS TouShuiL,
                    0.0 AS QiShuiB,
                    0.0 AS ShuiWen, 
                    0.0 AS WenDuJZTSL
                FROM MBR_RunningInfo 
                WHERE UpdateTime >= @StartDate AND UpdateTime <= @EndDate {0} 
                GROUP BY DATE_FORMAT(UpdateTime,'%Y-%m-%d')", whereClause);
            var list = this.db.Database.SqlQuery<MBR_RunningInfo>(sql,
                new MySqlParameter("@StartDate", StartDate),
                new MySqlParameter("@EndDate", EndDate));

            return list.ToList();

        }
    }
}
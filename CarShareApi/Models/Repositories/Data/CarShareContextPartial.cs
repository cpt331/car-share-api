using System.Data.Entity;

namespace CarShareApi.Models.Repositories.Data
{
    public partial class CarShareContext : DbContext
    {
        public CarShareContext(EwebahConfig config) : base(
            $@"metadata=res://*/Models.Repositories.Data.CarShareContext.csdl|res://*/Models.Repositories.Data.CarShareContext.ssdl|res://*/Models.Repositories.Data.CarShareContext.msl;provider=System.Data.SqlClient;provider connection string=';data source={
                    config.DbServer
                };initial catalog={config.DbName};persist security info=True;user id={config.DbUsername};password={
                    config.DbPassword
                };multipleactiveresultsets=True;application name=EntityFramework';")
        {
        }
    }
}
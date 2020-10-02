create table ChamomileAndPartners.dbo.Companies (
   [Id]             int       identity(1,1) not null
  ,[Name]           varchar(25)
  ,[ContractStatus] varchar(25)
  ,primary key ([Id])
)
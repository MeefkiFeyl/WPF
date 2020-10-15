create table ChamomileAndPartners.dbo.Companies (
   [Id]             int       identity(1,1) not null
  ,[Name]           varchar(25)             not null
  ,[ContractStatus] int
  ,primary key ([Id])
  ,constraint FK_Companies_ContractStatus foreign key ([ContractStatus]) references [ChamomileAndPartners].[dbo].[ContractStatus] ([Id])
)
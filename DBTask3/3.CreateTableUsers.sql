create table ChamomileAndPartners.dbo.Users (
   [Id]        int     identity(1,1) not null
  ,[CompanyId] int                   not null
  ,[Name]      varchar(25)           not null
  ,[Login]     varchar(25)
  ,[Password]  varchar(25)
  ,primary key ([Id])
  ,constraint FK_Users_Companies foreign key ([CompanyId]) references ChamomileAndPartners.dbo.Companies ([Id])
)

create index [IX_CompanyId] on ChamomileAndPartners.dbo.Users([CompanyId])
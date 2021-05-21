use master
if (exists (select top 1 * from master.dbo.sysdatabases where name = 'LinkingPaymentsToTheOrder'))
  drop database LinkingPaymentsToTheOrder
go

create database LinkingPaymentsToTheOrder
go

use LinkingPaymentsToTheOrder

create table Orders (
  Number            int         primary key identity,
  DateOrder         datetime    not null,
  Summ              money       not null check(Summ > 0),
  SummPay           money       default 0)

create table MoneyComings (
  Number            int         primary key identity,
  DatePayment       datetime    not null,
  Summ              money       not null check(Summ > 0),
  Balance           money       not null)

create table Payments (
  Id                int         primary key identity,
  NumberOrder       int         not null,
  NumberMoneyComing int         not null,
  SummPay           money       not null,
  constraint FK_Payement_To_Orders       foreign key (NumberOrder)       references Orders       (Number),
  constraint FK_Payement_To_MoneyComings foreign key (NumberMoneyComing) references MoneyComings (Number))
go

create trigger Payments_INSERT
on Payments
after insert
as
declare
    @id                 int,
    @NumberOrder        int,
    @NumberMoneyComing  int,
    @SummPay            money
begin

  declare
    @q      int

  select 
    @id                 = (select top 1 Id from inserted)

  select 
     @NumberOrder       = (select top 1 NumberOrder from Payments where Id = @Id)
    ,@NumberMoneyComing = (select top 1 NumberMoneyComing from Payments where Id = @Id)
    ,@SummPay           = (select top 1 SummPay from Payments where Id = @Id)
      
  if ((select Balance from MoneyComings where Number = @NumberMoneyComing) < @SummPay
   or (select SummPay from Orders where Number = @NumberOrder) + @SummPay > (select Summ from Orders where Number = @NumberOrder))
  begin
  
  raiserror (N'Сумма выплат в заказе не может привышать его сумму или остаток прихода денег не может быть отрицательным', 15, 1)
  end
  else
  begin
    update Orders
      set SummPay = SummPay + @SummPay
      where Number = @NumberOrder

    update MoneyComings
      set Balance = Balance - @SummPay
      where Number = @NumberMoneyComing
  end
end
go

create trigger Payments_DELETE
on Payments
after delete
as
declare 
  @Id                 int,
  @NumberOrder        int,
  @NumberMoneyComing  int,
  @SummPay            money
begin
  
  select
    @Id                 = (select top 1 Id from deleted),
    @NumberOrder        = (select top 1 NumberOrder from deleted),
    @NumberMoneyComing  = (select top 1 NumberMoneyComing from deleted),
    @SummPay            = (select top 1 SummPay from deleted)
  
  if ((select SummPay from Orders where Number = @NumberOrder) < @SummPay
   or (select Balance from MoneyComings where Number = @NumberMoneyComing) + @SummPay > (select Summ from MoneyComings where Number = @NumberMoneyComing))
  raiserror (N'Сумма выплаты в заказе не может быть отрицательной или остаток прихода денег не может быть больше его суммы', 15, 2)
  else
  begin
    update Orders
      set SummPay = SummPay - @SummPay
      where Number = @NumberOrder

    update MoneyComings
      set Balance = Balance + @SummPay
      where Number = @NumberMoneyComing
  end  
end
go

create procedure Test1
as
begin

  insert into Orders (
    DateOrder,
    Summ,
    SummPay)
  values (getdate(), 10, 0),  (getdate(), 50, 0),  (getdate(), 100, 0), (getdate(), 300, 0),
         (getdate(), 400, 0), (getdate(), 350, 0), (getdate(), 150, 0), (getdate(), 740, 0)

  insert into MoneyComings (
    DatePayment,
    Summ,
    Balance)
  values (getdate(), 500, 500), (getdate(), 750, 750), (getdate(), 350, 350), (getdate(), 5000, 5000)

  insert into Payments (
    NumberOrder,
    NumberMoneyComing,
    SummPay)
  values (1, 4, 5)

  insert into Payments (
    NumberOrder,
    NumberMoneyComing,
    SummPay)
  values (8, 1, 40)

  insert into Payments (
    NumberOrder,
    NumberMoneyComing,
    SummPay)
  values (1, 3, 3)

end
go

exec Test1
go
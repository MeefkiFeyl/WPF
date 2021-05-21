use LinkingPaymentsToTheOrder
/*select * from Orders
select * from MoneyComings
select * from Payments*/
declare
  @NumberOrder int = 1

select 
     o.Number   as [����� ������] 
    ,o.Summ     as [����� ������]
    ,o.SummPay  as [������ �� ������]
    ,p.SummPay  as [����� �������]
    ,mc.Number  as [����� �������]
    ,mc.Summ    as [����� �������]
    ,mc.Balance as [�������]
  from Orders             as o
  left join Payments      as p on p.NumberOrder = o.Number
  left join MoneyComings  as mc on mc.Number = p.NumberMoneyComing
  where o.Number = @NumberOrder

  select sum(SummPay)
    from Payments
    where NumberOrder = @NumberOrder

  select count(Number) as [���������� �����]
    from MoneyComings
go

use master
go

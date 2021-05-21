use LinkingPaymentsToTheOrder
/*select * from Orders
select * from MoneyComings
select * from Payments*/
declare
  @NumberOrder int = 1

select 
     o.Number   as [Номер заказа] 
    ,o.Summ     as [Сумма заказа]
    ,o.SummPay  as [Оплата по заказу]
    ,p.SummPay  as [Сумма платежа]
    ,mc.Number  as [Номер прихода]
    ,mc.Summ    as [Сумма прихода]
    ,mc.Balance as [Остаток]
  from Orders             as o
  left join Payments      as p on p.NumberOrder = o.Number
  left join MoneyComings  as mc on mc.Number = p.NumberMoneyComing
  where o.Number = @NumberOrder

  select sum(SummPay)
    from Payments
    where NumberOrder = @NumberOrder

  select count(Number) as [Количество строк]
    from MoneyComings
go

use master
go

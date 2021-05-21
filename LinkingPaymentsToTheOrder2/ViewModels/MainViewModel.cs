using LinkingPaymentsToTheOrder2.Commands;
using LinkingPaymentsToTheOrder2.Services;
using LinkingPaymentsToTheOrder2.Models;
using LinkingPaymentsToTheOrder2.Context;
using System.Collections.ObjectModel;
using Microsoft.EntityFrameworkCore;
using Microsoft.Data.SqlClient;
using System.Threading.Tasks;
using System.Windows.Input;
using System.Linq;
using System;
using System.Windows;

namespace LinkingPaymentsToTheOrder2.ViewModels
{
    class MainViewModel : BaseViewModel
    {

        #region Properties

        private Payment _selectedPayment { get; set; }
        public Payment SelectedPayment
        {
            get => _selectedPayment;
            set
            {
                _selectedPayment = value;
                OnPropertyChanged(nameof(SelectedPayment));
            }
        }

        private ObservableCollection<Order> _orders;
        public ObservableCollection<Order> Orders
        {
            get => _orders;
            set
            {
                _orders = value;
                OnPropertyChanged(nameof(Orders));
            }
        }

        private decimal? _orderSumm;
        public decimal? OrderSumm 
        { 
            get => _orderSumm;
            set
            {
                _orderSumm = value;
                OnPropertyChanged(nameof(OrderSumm));
            }
        }

        private Order _selectedOrder;
        public Order SelectedOrder
        {
            get => _selectedOrder;
            set
            {
                _selectedOrder = value;
                OnPropertyChanged(nameof(SelectedOrder));
            }
        }

        public DisplayRootRegistry DisplayRootRegistry { get; }
        public AppDbContext Context { get; }
        #endregion Properties

        public MainViewModel()
        {
        }
        public MainViewModel(AppDbContext context, DisplayRootRegistry displayRootRegistry)
        {
            this.DisplayRootRegistry = displayRootRegistry;

            Context = context;

            Context.LoadData(Context);

            Orders = Context.Orders.Local.ToObservableCollection();

            SqlConnection con = (SqlConnection)Context.Database.GetDbConnection();
            con.InfoMessage += delegate (object sender, SqlInfoMessageEventArgs e)
            {
                var modalWVM = new ModalWindowViewModel(Context, e.Message, 1);
                var res = DisplayRootRegistry.ShowModalPresentation(modalWVM);
            };

            Context.Database.CloseConnection();
        }

        #region Commands

        public ICommand RollbackLinkedPayment => new DelegateCommand(obj =>
        {
            Context.Database.OpenConnection();

            Context.LoadData(Context);

            Task res;

            if (SelectedPayment == null)
            {
                var modalWVM = new ModalWindowViewModel(Context, "Выберите платёж в заказе", 1);
                res = DisplayRootRegistry.ShowModalPresentation(modalWVM);
                Context.Database.CloseConnection();
                return;
            }

            using (var trans = Context.Database.BeginTransaction())
            {
                Context.Payments.Remove(SelectedPayment);

                try
                {
                    SelectedOrder.Payments = new ObservableCollection<Payment>(Context.Payments.Where(w => w.NumberOrder == SelectedOrder.Number));

                    Context.SaveChanges();

                    Context.Orders.Where(w => w.Number == SelectedOrder.Number).Load();
                    Context.SaveChanges();

                    Context.Orders.Where(w => w.Number == SelectedOrder.Number).Load();
                    Context.SaveChanges();
                    SelectedOrder.SummPay = Context.Orders.Where(w => w.Number == SelectedOrder.Number).Select(s => s.SummPay).FirstOrDefault();


                    trans.Commit();
                }
                catch (Exception e)
                {


                    var modalWVM = new ModalWindowViewModel(Context, "Необработанная ошибка при попытке откатить платёж", 1, e.Message);
                    res = DisplayRootRegistry.ShowModalPresentation(modalWVM);
                    trans.Rollback();
                }   
            }

            Context.Database.CloseConnection();
        });

        public ICommand OpenLinkingWindow => new DelegateCommand(ogj =>
        {
            Context.Database.OpenConnection();

            Context.LoadData(Context);
            Task res;

            if (SelectedOrder == null)
            {
                ModalWindowViewModel modalWVM = new ModalWindowViewModel(Context, "Выберите заказ", 1);
                res = DisplayRootRegistry.ShowModalPresentation(modalWVM);
                Context.Database.CloseConnection();
                return;
            }


            foreach (var item in Context.Orders.ToArray())
            {
                Context.Orders.Local.Where(w => w.Number == item.Number).FirstOrDefault().SummPay = item.SummPay;
            }

            Context.Orders.Where(w => w.Number == SelectedOrder.Number).Load();
            Context.SaveChanges();
            SelectedOrder.SummPay = Context.Orders.Where(w => w.Number == SelectedOrder.Number).Select(s => s.SummPay).FirstOrDefault();

            Context.Payments.Where(w => w.NumberOrder == SelectedOrder.Number).Load();
            Context.SaveChanges();

            var linkingVM = new LinkingViewModel(Context, SelectedOrder, DisplayRootRegistry);
            res = DisplayRootRegistry.ShowModalPresentation(linkingVM);

            Context.Database.CloseConnection();
        });

        public ICommand AddOrder => new DelegateCommand(obj =>
        {
            Context.Database.OpenConnection();

            Context.LoadData(Context);
            try
            {
                Orders.Add(new Order
                {
                    DateOrder = DateTime.Now,
                    Summ = (decimal)OrderSumm,
                    SummPay = 0
                });

                Context.SaveChanges();
            }
            catch(InvalidOperationException)
            {
                var modalWVM = new ModalWindowViewModel(Context, "Введите сумму заказа", 1);
                var res = DisplayRootRegistry.ShowModalPresentation(modalWVM);
            }

            Context.Database.CloseConnection();
        });

        public ICommand RemoveOrder => new DelegateCommand(obj =>
        {
            
            Context.Database.OpenConnection();

            Context.LoadData(Context);

            Task res;

            if (SelectedOrder == null)
            {
                var modalWVM = new ModalWindowViewModel(Context, "Выберите заказ", 1);
                res = DisplayRootRegistry.ShowModalPresentation(modalWVM);
                Context.Database.CloseConnection();
                return;
            }

            if (SelectedOrder.Payments.Count() > 0) 
            {
                var modalWVM = new ModalWindowViewModel(Context, "Вы пытаетесь удалить оплаченный заказ", 1);
                res = DisplayRootRegistry.ShowModalPresentation(modalWVM);
                Context.Database.CloseConnection();
                return;
            }

            try
            {
                Context.Orders.Remove(SelectedOrder);
                Context.SaveChanges();
            }
            catch (DbUpdateConcurrencyException e)
            {
                var modalWVM = new ModalWindowViewModel(Context, "Вы пытаетесь удалить несуществующий заказ. Обновить данные?", 2, e.Message);
                res = DisplayRootRegistry.ShowModalPresentation(modalWVM);
            }

            Context.Database.CloseConnection();
        });

        #endregion Commands

        #region Handlers

        #endregion Handlers
    }
}

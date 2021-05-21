using LinkingPaymentsToTheOrder2.Commands;
using LinkingPaymentsToTheOrder2.Context;
using LinkingPaymentsToTheOrder2.Models;
using LinkingPaymentsToTheOrder2.Services;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.ObjectModel;
using Microsoft.Data.SqlClient;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Input;

namespace LinkingPaymentsToTheOrder2.ViewModels
{
    class LinkingViewModel : BaseViewModel
    {
        #region Properties

        private decimal? _summPayment;
        public decimal? SummPayment
        {
            get => _summPayment;
            set
            {
                _summPayment = value;
                OnPropertyChanged(nameof(SummPayment));
            }
        }

        private decimal? _summMoneyComing;
        public decimal? SummMoneyComing
        {
            get => _summMoneyComing;
            set
            {
                _summMoneyComing = value;
                OnPropertyChanged(nameof(SummMoneyComing));
            }
        }

        private ObservableCollection<MoneyComing> _notLinkedMoneyComing;
        public ObservableCollection<MoneyComing> NotLinkedMoneyComing
        {
            get => _notLinkedMoneyComing;
            set
            {
                _notLinkedMoneyComing = value;
                OnPropertyChanged(nameof(NotLinkedMoneyComing));
            }
        }

        private ObservableCollection<MoneyComing> _moneyComingsSelectedOrder;
        public ObservableCollection<MoneyComing> MoneyComingsSelectedOrder
        {
            get => _moneyComingsSelectedOrder;
            set
            {
                _moneyComingsSelectedOrder = value;
                OnPropertyChanged(nameof(MoneyComingsSelectedOrder));
            }
        }

        private bool isLinkingSelect;
        private MoneyComing _selectedLinkingMoneyComing;
        public MoneyComing SelectedLinkingMoneyComing
        {
            get => _selectedLinkingMoneyComing;
            set
            {
                _selectedLinkingMoneyComing = value;
                isLinkingSelect = true;
                OnPropertyChanged(nameof(SelectedLinkingMoneyComing));
            }
        }

        private MoneyComing _selectedMoneyComing;
        public MoneyComing SelectedMoneyComing
        {
            get => _selectedMoneyComing;
            set
            {
                _selectedMoneyComing = value;
                isLinkingSelect = false;
                OnPropertyChanged(nameof(SelectedMoneyComing));
            }
        }

        private decimal? _summPays;
        public decimal? SummPays
        {
            get => _summPays;
            set
            {
                _summPays = value;
                OnPropertyChanged(nameof(SummPays));
            }
        }

        private decimal? _balance;
        public decimal? Balance
        {
            get => _balance;
            set
            {
                _balance = value;
                OnPropertyChanged(nameof(Balance));
            }
        }

        private string _numberOrder;
        public string NumberOrder
        {
            get => _numberOrder;
            set
            {
                _numberOrder = "Информация по заказу " + value;
                OnPropertyChanged(nameof(NumberOrder));
            }
        }

        public Order SelectedOrder { get; set; }
        public DisplayRootRegistry DisplayRootRegistry { get; }
        AppDbContext Context { get; }
        #endregion Properties

        public LinkingViewModel()
        {
        }
        public LinkingViewModel(AppDbContext context, Order selectedOrder, DisplayRootRegistry displayRootRegistry)
        {
            this.Context = context;
            this.DisplayRootRegistry = displayRootRegistry;

            Context.LoadData(Context);

            SelectedOrder = Context.Orders.Find(selectedOrder.Number);

            Balance = SelectedOrder.Summ - SelectedOrder.SummPay;
            SummPays = SelectedOrder.SummPay;
            NumberOrder = SelectedOrder.Number.ToString();

            MoneyComingsSelectedOrder = new ObservableCollection<MoneyComing>(SelectedOrder.Payments.Select(s => s.NumberMoneyComingNavigation).Distinct());
            NotLinkedMoneyComing = new ObservableCollection<MoneyComing>(Context.MoneyComings.Local.Except(MoneyComingsSelectedOrder));

            Context.Database.CloseConnection();
        }

        #region Commands

        public ICommand LinkMoneyComing => new DelegateCommand(obj => 
        {

            Context.Database.OpenConnection();

            Context.LoadData(Context);

            if (SelectedLinkingMoneyComing == null && SelectedMoneyComing == null)
            {
                var modalWVM = new ModalWindowViewModel(Context, "Выберите приход денег", 1);
                var res = DisplayRootRegistry.ShowModalPresentation(modalWVM);
                Context.Database.CloseConnection();
                return;
            }

            if (SummPayment == null)
            {
                var modalWVM = new ModalWindowViewModel(Context, "Введите сумму оплаты", 1);
                var res = DisplayRootRegistry.ShowModalPresentation(modalWVM);
                Context.Database.CloseConnection();
                return;
            }

            using (var trans = Context.Database.BeginTransaction())
            {
                Payment pay = new Payment
                {
                    NumberOrder = SelectedOrder.Number,
                    NumberMoneyComing = isLinkingSelect ? SelectedLinkingMoneyComing.Number : SelectedMoneyComing.Number,
                    SummPay = (decimal)SummPayment
                };

                Context.Payments.Add(pay);
                try
                {
                    Context.SaveChanges();

                    Context.Orders.Where(w => w.Number == SelectedOrder.Number).Load();
                    Context.SaveChanges();
                    SelectedOrder.SummPay = Context.Orders.Where(w => w.Number == SelectedOrder.Number).Select(s => s.SummPay).FirstOrDefault();
                    

                    Balance = SelectedOrder.Summ - SelectedOrder.SummPay;
                    Context.MoneyComings.Where(w => w.Number == pay.NumberMoneyComing).Load();
                    Context.SaveChanges();
                    Context.MoneyComings.Find(pay.NumberMoneyComing).Balance -= pay.SummPay;
                    SummPays = SelectedOrder.SummPay;

                    Context.LoadData(Context);
                    Context.SaveChanges();

                    var swaps = Context.Payments.Where(w => w.NumberOrder == SelectedOrder.Number).Select(s => s.NumberMoneyComingNavigation).Distinct();

                    MoneyComingsSelectedOrder = new ObservableCollection<MoneyComing>(swaps);
                    NotLinkedMoneyComing = new ObservableCollection<MoneyComing>(Context.MoneyComings.Local.Except(swaps));
                    SelectedLinkingMoneyComing = null;
                    SelectedMoneyComing = null;

                    trans.Commit();
                }
                catch (Exception e)
                {
                    var modalWVM = new ModalWindowViewModel(Context, "Сумма выплат в заказе не может привышать его сумму или остаток прихода денег не может быть отрицательным", 1, e.Message);
                    var res = DisplayRootRegistry.ShowModalPresentation(modalWVM);

                    trans.Rollback();
                }
            }

            Context.Database.CloseConnection();
        });

        public ICommand AddMoneyComing => new DelegateCommand(obg =>
        {
            Context.Database.OpenConnection();

            Context.LoadData(Context);

            if (SummMoneyComing == null)
            {
                var modalWVM = new ModalWindowViewModel(Context, "Введите сумму платежа", 1, "");
                var res = DisplayRootRegistry.ShowModalPresentation(modalWVM);
                Context.Database.CloseConnection();
                return;
            }

            MoneyComing moneyComing = new MoneyComing
            {
                DatePayment = DateTime.Now,
                Summ = (decimal)SummMoneyComing,
                Balance = (decimal)SummMoneyComing
            };

            Context.MoneyComings.Add(moneyComing);
            try
            {
                Context.SaveChanges();
                var moneyComings = Context.Payments.Where(w => w.NumberOrder == SelectedOrder.Number)
                                                   .Select(s => s.NumberMoneyComingNavigation)
                                                   .Distinct();

                foreach (var item in Context.MoneyComings.Local.Except(moneyComings))
                {
                    NotLinkedMoneyComing.Add(item);
                }

            }
            catch (DbUpdateConcurrencyException) 
            { 
                Context.MoneyComings.Remove(moneyComing); 
            }

                Context.Database.CloseConnection();

        });

        #endregion Commands
    }
}

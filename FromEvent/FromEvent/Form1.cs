using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.Reactive;
using System.Reactive.Linq;
using System.Reactive.Threading;
using System.Reactive.Concurrency;
using System.Reactive.Disposables;
using System.Windows;
using System.Windows.Threading;
using System.Reactive.Joins;
using System.Reactive.PlatformServices;
using System.Reactive.Subjects;
using Microsoft.VisualBasic;
using Microsoft.VisualBasic.Devices;
using System.Threading;




namespace FromEvent
{
    public partial class Form1 : Form
    {
        public Form1()
        {
            InitializeComponent();
        }
        

        private void Form1_Load(object sender, EventArgs e)
        {

            var start = Observable.FromEventPattern<EventArgs>(button1, "Click");
            var stop = Observable.FromEventPattern<EventArgs>(button2, "Click");

            
            var clock2 = Observable.Interval(TimeSpan.FromSeconds(1)).SubscribeOn(Scheduler.NewThread).TakeUntil(stop);
            start.Subscribe(x =>{
                var clock = Observable.Interval(TimeSpan.FromSeconds(1)).Select((t, index) => Thread.CurrentThread.ManagedThreadId).TakeUntil(stop);
                clock.SubscribeOn(Scheduler.NewThread).ObserveOn(DispatcherScheduler.Instance).Subscribe(timeIndex =>
                label1.Text = String.Format("{0}", timeIndex.ToString()));
                clock2.ObserveOn(this).Subscribe(timeIndex =>
                label2.Text = String.Format("{0}", timeIndex.ToString()));
            });

            //clock.ObserveOn(DispatcherScheduler.Instance).Subscribe(timeIndex =>
               // label1.Text = String.Format("{0}" , timeIndex.Time));
            
        }
    }
    class TimeIndex
    {
        public TimeIndex(int index, DateTimeOffset time)
        {
            Index = index;
            Time = time;
        }
        public int Index { get; set; }
        public DateTimeOffset Time { get; set; }
    }
}
